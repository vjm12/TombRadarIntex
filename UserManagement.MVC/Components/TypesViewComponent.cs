using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserManagement.MVC.Models;

namespace UserManagement.MVC.Views.Components
{
    public class TypesViewComponent : ViewComponent
    {
        private IFagElGamousRepository repo { get; set; }

        public TypesViewComponent(IFagElGamousRepository temp)
        {
            repo = temp;
        }

        public IViewComponentResult Invoke()
        {

            ViewBag.SelectedSex = RouteData?.Values["burialsex"];
            var types = repo.burialmains
                .Select(x => x.Sex)
                .Distinct()
                .OrderBy(x => x);

            return View(types);
        }
    }
}
