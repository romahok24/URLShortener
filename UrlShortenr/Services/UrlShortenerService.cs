using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using UrlShortenr.Data;
using UrlShortenr.Models;

namespace UrlShortenr.Services
{
    public class UrlShortenerService : IUrlService
    {
        public Task<Statistic> Click(string shortUrl, string referer, string ip)
        {
            return Task.Run(() =>
            {
                using (var context = new UrlContext())
                {


                    var url = context.Urls.Include(r => r.Statistics).Where(o => o.ShortUrl.Equals(shortUrl)).FirstOrDefault();

                    if (url == null)
                        throw new Exception("Short Not Found");

                    url.ClicksCount +=1;

                    Statistic stat = new Statistic
                    {
                        ClickDate = DateTime.Now,
                        Ip = ip,
                        Referer = referer,
                        Url = url
                    };

                    url.Statistics.Add(stat);
                    context.Urls.Update(url);
                    context.Statistics.Add(stat);
                    context.SaveChanges();

                    return stat;
                }
            });
        }

        public Task<Url> MakeShort(string longUrl, string ip)
        {
            return Task.Run(() =>
            {
                if (!String.IsNullOrWhiteSpace(longUrl))
                {
                    if (!longUrl.StartsWith("http://") && !longUrl.StartsWith("https://"))
                    {
                        throw new ArgumentException("Invalid URL format");
                    }
                }
                else
                {
                    throw new Exception("Url is null");
                }

                Uri urlCheck = new Uri(longUrl);
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(urlCheck);
                request.Timeout = 10000;

                try
                {
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                }
                catch (Exception)
                {
                    throw new Exception();
                }

                using (var context = new UrlContext())
                {
                    string shortUrl = NewShortUrl();
                    var url = context.Urls.Where(o => o.LongUrl == longUrl).FirstOrDefault();

                    if (url != null)
                    {
                        url.ShortUrl = shortUrl;
                        return url;
                    }

                    url = new Url()
                    {
                        Added = DateTime.Now,
                        Ip = ip,
                        LongUrl = longUrl,
                        ClicksCount = 0,
                        ShortUrl = shortUrl
                    };

                    return url;
                }
            });
        }

        private string NewShortUrl()
        {
            using (var context = new UrlContext())
            {
                while (true)
                {

                    string url = Guid.NewGuid().ToString().Substring(0, 6);

                    if (context.Urls.Where(o => o.ShortUrl.Equals(url)).Count() == 0)
                        return url;
                }
            }
        }
    }
}
