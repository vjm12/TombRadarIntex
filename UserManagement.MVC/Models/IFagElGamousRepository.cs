using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UserManagement.MVC.Models
{
    public interface IFagElGamousRepository
    {
        IQueryable<Burialmain> burialmains { get; }
    }
}
