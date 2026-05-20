using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using EnergySportsClub.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EnergySportsClub.Areas.Admin.Pages.Managers
{
    [Authorize(Roles = "Admin")]
    public class EditModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public EditModel(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        [BindProperty]
        public InputModel Input { get; set; } = new InputModel();

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return NotFound();
            }

            var user = await _userManager.FindByIdAsync(id);
            if (user == null || !await _userManager.IsInRoleAsync(user, "Manager"))
            {
                return NotFound();
            }

            Input = new InputModel
            {
                UserId = user.Id,
                Email = user.Email ?? string.Empty,
                Name = user.Name,
                Surname = user.Surname,
                Age = user.Age,
                Gender = user.Gender
            };

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = await _userManager.FindByIdAsync(Input.UserId);
            if (user == null || !await _userManager.IsInRoleAsync(user, "Manager"))
            {
                return NotFound();
            }

            user.Name = Input.Name;
            user.Surname = Input.Surname;
            user.Age = Input.Age;
            user.Gender = Input.Gender;

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }

                return Page();
            }

            return RedirectToPage("Index");
        }

        public class InputModel
        {
            [Required]
            public string UserId { get; set; } = string.Empty;

            [Display(Name = "Email")]
            public string Email { get; set; } = string.Empty;

            [Required]
            [Display(Name = "Name")]
            public string Name { get; set; } = string.Empty;

            [Required]
            [Display(Name = "Surname")]
            public string Surname { get; set; } = string.Empty;

            [Range(1, 120)]
            [Display(Name = "Age")]
            public int Age { get; set; }

            [Required]
            [Display(Name = "Gender")]
            public string Gender { get; set; } = string.Empty;
        }
    }
}
