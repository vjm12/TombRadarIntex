using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserManagement.MVC.Models;

namespace UserManagement.MVC.Controllers
{
    public class UserRolesController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserRolesController(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<IActionResult> Index()
        {
            var users = await _userManager.Users.ToListAsync();

            var userRolesViewModel = new List<UserRolesViewModel>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                var userRoles = new UserRolesViewModel
                {
                    UserId = user.Id,
                    Email = user.Email,
                    Roles = roles
                };

                userRolesViewModel.Add(userRoles);
            }

            return View(userRolesViewModel);
        }



        public async Task<IActionResult> Manage(string userId)
        {
            // Get the user by id
            var user = await _userManager.FindByIdAsync(userId);

            // If the user doesn't exist, return a 404 not found error
            if (user == null)
            {
                return NotFound();
            }

            // Get the roles for the user
            var roles = await _userManager.GetRolesAsync(user);

            // Create a list of ManageUserRolesViewModel objects for the roles
            var model = new List<ManageUserRolesViewModel>();
            foreach (var role in _roleManager.Roles)
            {
                var userRolesViewModel = new ManageUserRolesViewModel
                {
                    RoleId = role.Id,
                    RoleName = role.Name,
                    Selected = roles.Contains(role.Name)
                };

                model.Add(userRolesViewModel);
            }

            // Set the ViewBag properties
            ViewBag.userId = userId;
            ViewBag.UserName = user.UserName;

            // Return the view with the model
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Manage(string userId, List<ManageUserRolesViewModel> model)
        {
            // Get the user by id
            var user = await _userManager.FindByIdAsync(userId);

            // If the user doesn't exist, return a 404 not found error
            if (user == null)
            {
                return NotFound();
            }

            // Get the roles for the user
            var roles = await _userManager.GetRolesAsync(user);

            // Loop through the model and add or remove roles based on the Selected property
            foreach (var userRole in model)
            {
                var role = await _roleManager.FindByIdAsync(userRole.RoleId);

                if (userRole.Selected && !roles.Contains(role.Name))
                {
                    await _userManager.AddToRoleAsync(user, role.Name);
                }
                else if (!userRole.Selected && roles.Contains(role.Name))
                {
                    await _userManager.RemoveFromRoleAsync(user, role.Name);
                }
            }

            // Redirect back to the Index action
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> DeleteUser(string userId)
        {
            if (userId == null)
            {
                return NotFound();
            }

            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteUserConfirmed(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return View("Index");
            }

            var result = await _userManager.DeleteAsync(user);

            if (result.Succeeded)
            {
                return RedirectToAction("Index");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View("Index");
        }




    }
}
