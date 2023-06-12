using Microsoft.AspNetCore.Mvc;
using RedisExchangeAPI.Web.Service;
using StackExchange.Redis;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace RedisExchangeAPI.Web.Controllers
{
    public class ListController : Controller
    {
        public readonly RedisService _redisService;
        public readonly IDatabase _database;
        public readonly string _myList = "names";
        public ListController(RedisService redisService)
        {
            _redisService= redisService;
            _database = redisService.GetDB(0);
        }
        public IActionResult Index()
        {
            List<string> list = new List<string>();
            if (_database.KeyExists(_myList))
            {
                _database.ListRange(_myList).ToList().ForEach(x => {
                    list.Add(x.ToString());
                });
            }
            return View(list);
        }
        [HttpPost]
        public IActionResult Add(string name)
        {
            _database.ListLeftPush(_myList, name);
            return RedirectToAction("Index");
        }
        
        public IActionResult Delete(string name)
        {
            _database.ListRemove(_myList, name);
            return RedirectToAction("Index");
        }
    }
}
