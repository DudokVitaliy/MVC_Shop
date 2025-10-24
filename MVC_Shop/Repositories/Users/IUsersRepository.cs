using MVC_Shop.Models;

namespace MVC_Shop.Repositories.Users
{
    public interface IUsersRepository
    {
        IQueryable<ApplicationUser> Users { get; }
        Task DeleteAsync(string id);
        Task CreateAsync(ApplicationUser user, string password);
        Task<bool> IsExistAsync(string name);
    }
}
