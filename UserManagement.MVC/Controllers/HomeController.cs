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
        public IActionResult Summary( string burialdirec, int pageNum = 1)//default will be 1
        {

            int pagesize = 25;
            var burial = new BurialMainViewModel
            {
                textiles = repo.textiles
                .OrderBy(t=>t.Id)
                .Skip((pageNum - 1) * pagesize)
                .Take(pagesize)
                ,
                burialmains = repo.burialmains
                .Where(b => b.Squarenorthsouth == burialdirec | burialdirec == null)
                .OrderBy(b => b.Dateofexcavation)
                //for pagination
                .Skip((pageNum - 1) * pagesize)
                .Take(pagesize),
                pageInfo = new PageInfo
                {
                    TotalNumItems = (burialdirec == null? 
                    repo.burialmains.Count()
                    : repo.burialmains.Where(x=>x.Squarenorthsouth == burialdirec).Count()),
                    ItemsPerPage = pagesize,
                    CurrentPage = pageNum
                }
            }; 

            return View(burial);
        }

        [HttpGet]
        public IActionResult EditBurial(long id)
        {
            var specificburial = repo.burialmains.Single(x => x.Id == id);
            return View(specificburial);
        }
      //  [HttpPost]
      //  public IActionResult EditBurial(Burialmain bm)
      //  {
      //      fagContext.Update(bm);
      //      fagContext.saveChanges();
      //ZZZZZ

      //      return RedirectToAction("DisplayMovie");
      //  }
        public IActionResult DetailedBurial(long id)
        {
            var specificburial = repo.burialmains.Single(x => x.Id == id);
            return View(specificburial);
        }
        [HttpGet]
        public IActionResult DeleteBurial(long id)
        {
            var specificburial = repo.burialmains.Single(x => x.Id == id);
            return View(specificburial);
        }

        //[HttpPost]
        //public IActionResult DeleteBurial(long id)
        //{
        //    var specificburial = repo.burialmains.Single(x => x.Id == id);
        //    repo.burialmains.Remove(specificburial);
        //    repo.SaveChanges();
        //    return RedirectToAction("Summary");
        //}

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
