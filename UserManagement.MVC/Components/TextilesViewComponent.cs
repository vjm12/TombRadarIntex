using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserManagement.MVC.Models;

namespace UserManagement.MVC.Components
{
    public class TextilesViewComponent :ViewComponent
    {
        private IFagElGamousRepository repo { get; set; }

        public TextilesViewComponent(IFagElGamousRepository temp)
        {
            repo = temp;
        }

        public IViewComponentResult Invoke()
        { 
            return View();
        }
    }

}
