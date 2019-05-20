using System;
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
                return View(await context.Urls.Include(s => s.Statistics).ToListAsync());
            }
        }

        /*[HttpGet]
        public IActionResult Create(Url url)
        {
            return View(url);
        }

        /*[HttpPost]
        /*public async Task<IActionResult> Create(string longUrl)
        {
            string referer = Request.Headers["Referer"].ToString() ?? string.Empty;

            Url shortUrl = await urlService.MakeShort(longUrl, referer);

            try
            {
                using (var context = new UrlContext())
                {
                    context.Urls.Add(shortUrl);
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }

            return RedirectToAction("Index");
        }*/

        public async Task<IActionResult> Create(int? id)
        {
            if (id != null)
            {
                using (var context = new UrlContext())
                {
                    var url = await context.Urls.Include(s => s.Statistics).Where(o => o.Id == id).FirstOrDefaultAsync();

                    if (url != null)
                        return View(url);
                }
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Url url)
        {
            using (var context = new UrlContext())
            {
                string referer = Request.Headers["Referer"].ToString() ?? string.Empty;

                var item = await context.Urls.Where(o => o.Id == url.Id).FirstOrDefaultAsync();

                if (item != null)
                {
                    Url shortUrl = await urlService.MakeShort(item.LongUrl, referer);

                    item.ShortUrl = shortUrl.ShortUrl;
                    item.ShortUrl = string.Format("{0}://{1}{2}.co", Request.Scheme, Url.Content("~"), item.ShortUrl);
                    context.Urls.Update(item);
                    await context.SaveChangesAsync();
                }
                else
                {
                    try
                    {
                        Url shortUrl = await urlService.MakeShort(url.LongUrl, referer);
                        shortUrl.ShortUrl = string.Format("{0}://{1}{2}.co", Request.Scheme, Url.Content("~"), shortUrl.ShortUrl);
                        context.Urls.Add(shortUrl);                        
                        await context.SaveChangesAsync();
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError("", ex.Message);
                    }
                }
                
            }

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Click(string shortUrl)
        {
            string referer = Request.Headers["Referer"].ToString() ?? string.Empty;

            Statistic stat = await urlService.Click(shortUrl, referer, Request.HttpContext.Connection.RemoteIpAddress.ToString());

            return RedirectPermanent(stat.Url.LongUrl);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            using (var context = new UrlContext())
            {
                var rector = await context.Urls.Include(r => r.Statistics)
                                    .SingleOrDefaultAsync(m => m.Id == id);

                if (rector == null)
                {
                    return NotFound();
                }

                return View(rector);
            }
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            using (var context = new UrlContext())
            {
                var url = await context.Urls.Include(s => s.Statistics).SingleOrDefaultAsync(m => m.Id == id);

                context.Statistics.RemoveRange(url.Statistics);
                context.Urls.Remove(url);
                await context.SaveChangesAsync();

                return RedirectToAction("Index");
            }
        }
    }
}
