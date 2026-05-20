using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EnergySportsClub.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EnergySportsClub.Areas.Admin.Pages.Users
{
    [Authorize(Roles = "Admin")]
    public class IndexModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public IndexModel(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public IList<UserItem> Users { get; private set; } = new List<UserItem>();

        public async Task OnGetAsync()
        {
            var users = _userManager.Users.ToList();
            var items = new List<UserItem>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                items.Add(new UserItem
                {
                    Id = user.Id,
                    Email = user.Email ?? string.Empty,
                    Name = user.Name,
                    Surname = user.Surname,
                    Roles = roles.ToList()
                });
            }

            Users = items.OrderBy(user => user.Email).ToList();
        }



        public async Task<IActionResult> OnPostSetRoleAsync(string userId, string role)
        {
            if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(role))
            {
                return RedirectToPage();
            }

            if (role != "User" && role != "Manager")
            {
                return RedirectToPage();
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return RedirectToPage();
            }

            if (await _userManager.IsInRoleAsync(user, "Admin"))
            {
                return RedirectToPage();
            }

            var existingRoles = await _userManager.GetRolesAsync(user);
            var rolesToRemove = existingRoles.Where(existingRole => existingRole == "User" || existingRole == "Manager").ToList();

            if (rolesToRemove.Count > 0)
            {
                var removeResult = await _userManager.RemoveFromRolesAsync(user, rolesToRemove);
                if (!removeResult.Succeeded)
                {
                    foreach (var error in removeResult.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }

                    await OnGetAsync();
                    return Page();
                }
            }

            var addResult = await _userManager.AddToRoleAsync(user, role);
            if (!addResult.Succeeded)
            {
                foreach (var error in addResult.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }

                await OnGetAsync();
                return Page();
            }

            return RedirectToPage();
        }

        public class UserItem
        {
            public string Id { get; set; } = string.Empty;
            public string Email { get; set; } = string.Empty;
            public string Name { get; set; } = string.Empty;
            public string Surname { get; set; } = string.Empty;
            public List<string> Roles { get; set; } = new();
        }
    }
}
