using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Threading.Tasks;

namespace RedisDistributedCache.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IDistributedCache _distributedCache;
        public ProductsController(IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache;
        }

        public IActionResult Index()
        {
            DistributedCacheEntryOptions options = new DistributedCacheEntryOptions();
            options.AbsoluteExpiration=DateTime.Now.AddMinutes(1);
             _distributedCache.SetString("name", "alper", options);

            return View();
        }
        public IActionResult Show() 
        {
            ViewBag.Show =_distributedCache.GetString("name");
            return View();
        }
        public IActionResult Remove()
        {
            _distributedCache.RemoveAsync("name");
            return View();
        }
    }
}
