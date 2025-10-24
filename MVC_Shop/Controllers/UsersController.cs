using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MVC_Shop.Models;
using MVC_Shop.Repositories.Users;
using MVC_Shop.ViewModels.Users;

namespace MVC_Shop.Controllers
{
    public class UsersController : Controller
    {
        private readonly IUsersRepository _usersRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private const int PageSize = Settings.PaginationPageSize;

        public UsersController(IUsersRepository usersRepository, UserManager<ApplicationUser> userManager)
        {
            _usersRepository = usersRepository;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index(int page = 1)
        {
            var usersQuery = _usersRepository.Users;

            int totalUsers = usersQuery.Count();
            var users = usersQuery
                .OrderBy(u => u.UserName)
                .Skip((page - 1) * PageSize)
                .Take(PageSize)
                .ToList();


            var userVMs = new List<UserVM>();
            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                userVMs.Add(new UserVM
                {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    UserName = user.UserName,
                    Email = user.Email,
                    Role = roles.FirstOrDefault() ?? "No Role"
                });
            }

            var vm = new UserListVM
            {
                Users = userVMs,
                CurrentPage = page,
                TotalPages = (int)Math.Ceiling(totalUsers / (double)PageSize)
            };

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(string id)
        {
            if (string.IsNullOrEmpty(id))
                return BadRequest();

            await _usersRepository.DeleteAsync(id);

            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangeRole(string userId, string newRole)
        {
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(newRole))
                return BadRequest();

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return NotFound();

            var currentRoles = await _userManager.GetRolesAsync(user);

            var removeResult = await _userManager.RemoveFromRolesAsync(user, currentRoles);
            if (!removeResult.Succeeded)
            {
                TempData["Error"] = "Не вдалося видалити старі ролі.";
                return RedirectToAction(nameof(Index));
            }

            var addResult = await _userManager.AddToRoleAsync(user, newRole);
            if (!addResult.Succeeded)
            {
                TempData["Error"] = "Не вдалося додати нову роль.";
            }
            else
            {
                TempData["Success"] = "Роль користувача успішно змінена.";
            }

            return RedirectToAction(nameof(Index));
        }

    }
}
