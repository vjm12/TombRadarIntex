using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UserManagement.MVC.Models.ViewModels
{
    public class BurialMainViewModel
    {
        public IQueryable<Burialmain> burialmains { get; set; }
        public PageInfo pageInfo { get; set; }

    }
}
