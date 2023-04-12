using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UserManagement.MVC.Models.ViewModels
{
    public class PageInfo
    {
        public int TotalNumItems { get; set; }
        public int ItemsPerPage { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPagesNeeded => (int) Math.Ceiling((double)TotalNumItems / ItemsPerPage);
    }
}
