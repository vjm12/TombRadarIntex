using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using UserManagement.MVC.Models;
using UserManagement.MVC.Models.ViewModels;

namespace UserManagement.MVC.Controllers
{
    public class HomeController : Controller
    {
        private IFagElGamousRepository repo;

        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, IFagElGamousRepository context)
        {
            repo = context;
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Summary( string burialsex, int pageNum = 1)//default will be 1
        {

            int pagesize = 25;
            var burial = new BurialMainViewModel
            {
                burialmains = repo.burialmains
                .Where(b => b.Sex == burialsex | burialsex == null)
                .OrderBy(b => b.Dateofexcavation)
                //for pagination
                .Skip((pageNum - 1) * pagesize)
                .Take(pagesize),
                pageInfo = new PageInfo
                {
                    TotalNumItems = (burialsex == null? 
                    repo.burialmains.Count()
                    : repo.burialmains.Where(x=>x.Sex == burialsex).Count()),
                    ItemsPerPage = pagesize,
                    CurrentPage = pageNum
                }
            }; 

            return View(burial);
        }

        public IActionResult DetailedBurial(long id)
        {
            var specificburial = repo.burialmains.Single(x => x.Id == id);
            return View(specificburial);
        }


        public IActionResult Analysis_Supervised()
        {
            return View();
        }

        public IActionResult Analysis_Unsupervised()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
