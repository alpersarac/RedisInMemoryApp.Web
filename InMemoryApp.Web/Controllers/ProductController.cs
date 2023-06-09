using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System;

namespace InMemoryApp.Web.Controllers
{
    public class ProductController : Controller
    {
        private readonly IMemoryCache _memoryCache;
        public ProductController(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }
        public IActionResult Index()
        {

            MemoryCacheEntryOptions options = new MemoryCacheEntryOptions();
            options.AbsoluteExpiration = DateTime.Now.AddSeconds(10);
            //options.SlidingExpiration= TimeSpan.FromSeconds(10);
            options.Priority = CacheItemPriority.High;
            options.RegisterPostEvictionCallback((key, value, reason, state) =>
            {
                _memoryCache.Set<string>("callback", $"{key}->{value}=>{reason}");
            });
            _memoryCache.Set<string>("zaman", DateTime.Now.ToString(), options);

            return View();
        }
        public IActionResult Show()
        {
            _memoryCache.TryGetValue("zaman", out string value);
            _memoryCache.TryGetValue("callback", out string callback);
            ViewBag.zaman = _memoryCache.Get<string>("zaman");
            ViewBag.callback = _memoryCache.Get<string>("callback");

            return View();
        }
    }
}
