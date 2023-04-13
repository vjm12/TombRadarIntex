using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UserManagement.MVC.Models
{
    public class EFFagELGamousRepository :IFagElGamousRepository
    {
        private fagContext context { get; set; }
        public EFFagELGamousRepository(fagContext temp)
        {
            context = temp;
        }
        public IQueryable<Burialmain> burialmains => context.Burialmain;
        public IQueryable<Textile> textiles => context.Textile;

    }
}