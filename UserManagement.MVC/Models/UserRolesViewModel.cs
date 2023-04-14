using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace UserManagement.MVC.Models
{
    //keeps track of user information
    public class UserRolesViewModel
    {
        public List<User> Users { get; set; }
        [Key]
        public string UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        [NotMapped]
        public IEnumerable<string> Roles { get; set; }

        public class User
        {
            public string FirstName { get; set; }
            public string UserId { get; set; }
            public string LastName { get; set; }
            public string Email { get; set; }
            public List<string> Roles { get; set; }
        }

    }
}
