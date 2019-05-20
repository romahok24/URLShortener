using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UrlShortenr.Data;
using UrlShortenr.Models;
using UrlShortenr.Services;

namespace UrlShortenr.Controllers
{
    public class HomeController : Controller
    {
        IUrlService urlService;

        public HomeController(IUrlService urlService)
        {
            this.urlService = urlService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            using (var context = new UrlContext())
            {
                return View(await context.Urls.ToListAsync());
            }
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Url url)
        {
            string referer = Request.Headers["Referer"].ToString() ?? string.Empty;

            Url shortUrl = await urlService.MakeShort(url.LongUrl, referer);

            using (var context = new UrlContext())
            {
                context.Urls.Add(shortUrl);
                await context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id != null)
            {
                using (var context = new UrlContext())
                {
                    var url = await context.Urls.Where(o => o.Id == id).FirstOrDefaultAsync();

                    if (url != null)
                        return View(url);
                }
            }

            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Url url)
        {
            using (var context = new UrlContext())
            {
                var item = await context.Urls.Where(o => o.Id == url.Id).FirstOrDefaultAsync();

                item = url;

                await context.SaveChangesAsync();
            }

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Click(string shortUrl)
        {
            string referer = Request.Headers["Referer"].ToString() ?? string.Empty;

            Statistic stat = await urlService.Click(shortUrl, referer, Request.HttpContext.Connection.RemoteIpAddress.ToString());

            return RedirectPermanent(stat.Url.LongUrl);
        }
    }
}
