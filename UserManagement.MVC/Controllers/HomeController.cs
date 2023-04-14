using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using UserManagement.MVC.Models;
using UserManagement.MVC.Models.ViewModels;

namespace UserManagement.MVC.Controllers
{
    public class HomeController : Controller
    {
        private fagContext _fagContext { get; set; }

        
        private IFagElGamousRepository repo;

        private readonly ILogger<HomeController> _logger;

        public HomeController( fagContext fagContext, ILogger<HomeController> logger, IFagElGamousRepository context)
        {
            _fagContext = fagContext;
            repo = context;
            _logger = logger;
        }
        //Index View
        public IActionResult Index()
        {
            return View();
        }
        //Summary View
        public IActionResult Summary( string burialdirec, int pageNum = 1)//default will be 1
        {

            int pagesize = 25;
            var burial = new BurialMainViewModel
            {
                textiles = repo.textiles
                .OrderBy(t=>t.Id)
          
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

        //Edit Burial View -- Requires Authorization
        //[Authorize(Roles="SuperAdmin,Admin,Researcher")]
        [HttpGet]
        public IActionResult EditBurial(long id)
        {
            var specificburial = repo.burialmains.Single(x => x.Id == id);
            return View(specificburial);
        }

        //Edit Textile View -- Requires Authorization
        [Authorize(Roles = "SuperAdmin,Admin,Researcher")]
        [HttpGet]
        public IActionResult EditTextile(long id)
        {
            var specifictextile = repo.textiles.Single(x => x.Id == id);
            return View(specifictextile);

        }
        //Save Burial Changes -- Requires Authorization
        //[Authorize(Roles = "SuperAdmin,Admin,Researcher")]
        [HttpPost]
        public IActionResult EditBurial(Burialmain bm)
        {
            _fagContext.Update(bm);
            _fagContext.SaveChanges();

            return RedirectToAction("DetailedBurial", new { id = bm.Id });
        }
        //Delete Burial Confirmation Page -- Requires Authorization
        //[Authorize(Roles = "SuperAdmin,Admin,Researcher")]
        [HttpGet]
        public IActionResult DeleteBurialConfirmation(long id)
        {
            var specificburial = repo.burialmains.Single(x => x.Id == id);
            return View(specificburial);
        }
        //Save Burial Removal -- Requires Authorization
        //[Authorize(Roles = "SuperAdmin,Admin,Researcher")]
        [HttpPost]
        public IActionResult DeleteBurialConfirmation(Burialmain bm)
        {
            _fagContext.Burialmain.Remove(bm);
            _fagContext.SaveChanges();
            return RedirectToAction("Summary");
        }
        //Confirmation page for new records
        [Authorize(Roles="SuperAdmin,Admin,Researcher")]
        public IActionResult Confirmation()
        {
            return View();
        }
        //Add New burial
        [Authorize(Roles = "SuperAdmin,Admin,Researcher")]
        [HttpGet]
        public IActionResult NewBurial()
        {
            return View();
        }
        //Save new burial
        [Authorize(Roles = "SuperAdmin,Admin,Researcher")]
        public IActionResult NewBurial(Burialmain bm)
        {
            if (ModelState.IsValid)
            {
                _fagContext.Add(bm);
                _fagContext.SaveChanges();
                return View("Confirmation", bm);
            }
            else
            {
                return View(bm);
            }
        }
        //Add New Textile
        [Authorize(Roles ="SuperAdmin,Admin,Researcher")]
        [HttpGet]
        public IActionResult NewTextile()
        {
            return View();
        }
        //Save new textile
        [Authorize(Roles ="SuperAdmin,Admin,Researcher")]
        public IActionResult NewTextile(Textile t)
        {
            if (ModelState.IsValid)
            {
                _fagContext.Add(t);
                _fagContext.SaveChanges();
                return View("Confirmation", t);
            }
            else
            {
                return View(t);
            }
        }
        //Save Textile Changes - Requires authorization
        //[Authorize(Roles = "SuperAdmin,Admin,Researcher")]
        [HttpPost]
        public IActionResult EditTextile(Textile t)
        {
            _fagContext.Update(t);
            _fagContext.SaveChanges();

            return RedirectToAction("Summary");
        }
        //Delete Burial Confirmation Page -- Requires Authorization
        [Authorize(Roles = "SuperAdmin,Admin,Researcher")]
        [HttpGet]
        public IActionResult DeleteTextileConfirmation(long id)
        {
            var specifictextile = repo.textiles.SingleOrDefault(x => x.Id == id);
            return View(specifictextile);
        }

        //Save Textile Removal - Requires authorization
        //[Authorize(Roles = "SuperAdmin,Admin,Researcher")]
        [HttpPost]
        public IActionResult DeleteTextileConfirmation(Textile t)
        {
            _fagContext.Textile.Remove(t);
            _fagContext.SaveChanges();
            return RedirectToAction("Summary");
        }
        //Detailed Burial View
        public IActionResult DetailedBurial(long id)
        {
            var specificburial = repo.burialmains.Single(x => x.Id == id);
            return View(specificburial);
        }
        //Supervised Analysis View
        public IActionResult Analysis_Supervised()
        {
            return View();
        }
        //Unsupervised Analysis View
        public IActionResult Analysis_Unsupervised()
        {
            return View();
        }
        //Privacy View
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
