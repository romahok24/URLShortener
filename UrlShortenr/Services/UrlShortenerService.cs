using System;
using System.Linq;
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
                    var url = context.Urls.Where(o => o.ShortUrl.Equals(shortUrl)).FirstOrDefault();

                    if (url == null)
                        throw new Exception("Short Not Found");

                    url.ClicksCount++;

                    Statistic stat = new Statistic
                    {
                        ClickDate = DateTime.Now,
                        Ip = ip,
                        Referer = referer,
                        Url = url
                    };

                    context.Statistics.Add(stat);
                    context.SaveChanges();

                    return stat;
                }
            });
        }

        public Task<Url> MakeShort(string longUrl, string ip, string shortUrl = "")
        {
            return Task.Run(() =>
            {
                using (var context = new UrlContext())
                {
                    var url = context.Urls.Where(o => o.LongUrl == longUrl).FirstOrDefault();

                    if (url != null)
                        return url;

                    if (!String.IsNullOrWhiteSpace(shortUrl))
                    {
                        if (context.Urls.Where(o => o.ShortUrl == shortUrl).Any())
                            throw new Exception("Conflict!");
                    }
                    else
                    {
                        shortUrl = this.NewShortUrl();
                    }

                    url = new Url()
                    {
                        Added = DateTime.Now,
                        Ip = ip,
                        LongUrl = longUrl,
                        ClicksCount = 0,
                        ShortUrl = shortUrl
                    };

                    context.Urls.Add(url);
                    context.SaveChanges();

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
