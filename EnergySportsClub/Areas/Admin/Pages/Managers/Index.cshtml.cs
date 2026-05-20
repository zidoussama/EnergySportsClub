using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EnergySportsClub.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EnergySportsClub.Areas.Admin.Pages.Managers
{
    [Authorize(Roles = "Admin")]
    public class IndexModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public IndexModel(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public IList<ManagerItem> Managers { get; private set; } = new List<ManagerItem>();

        public async Task OnGetAsync()
        {
            var managers = await _userManager.GetUsersInRoleAsync("Manager");
            Managers = managers
                .OrderBy(user => user.Email)
                .Select(user => new ManagerItem
                {
                    Id = user.Id,
                    Email = user.Email ?? string.Empty,
                    Name = user.Name,
                    Surname = user.Surname
                })
                .ToList();
        }


        public async Task<IActionResult> OnPostRemoveAsync(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                return RedirectToPage();
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return RedirectToPage();
            }

            var result = await _userManager.RemoveFromRoleAsync(user, "Manager");
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }

                await OnGetAsync();
                return Page();
            }

            return RedirectToPage();
        }

        public class ManagerItem
        {
            public string Id { get; set; } = string.Empty;
            public string Email { get; set; } = string.Empty;
            public string Name { get; set; } = string.Empty;
            public string Surname { get; set; } = string.Empty;
        }
    }
}
