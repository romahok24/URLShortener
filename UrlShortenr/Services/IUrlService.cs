using System.Threading.Tasks;
using UrlShortenr.Models;

namespace UrlShortenr.Services
{
    public interface IUrlService
    {
        Task<Url> MakeShort(string longUrl, string ip, string shortUrl = "");

        Task<Statistic> Click(string shortUrl, string id, string ip);
    }
}
