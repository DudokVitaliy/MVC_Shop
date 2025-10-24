using Microsoft.AspNetCore.Identity;
using MVC_Shop.Models;

namespace MVC_Shop.Repositories.Users
{
    public class UsersRepository : IUsersRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UsersRepository(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public IQueryable<ApplicationUser> Users => _userManager.Users;

        public async Task CreateAsync(ApplicationUser user, string password)
        {
            await _userManager.CreateAsync(user, password);
        }

        public async Task DeleteAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                await _userManager.DeleteAsync(user);
            }
        }

        public async Task<bool> IsExistAsync(string name)
        {
            var user = await _userManager.FindByNameAsync(name);
            return user != null;
        }
    }
}
