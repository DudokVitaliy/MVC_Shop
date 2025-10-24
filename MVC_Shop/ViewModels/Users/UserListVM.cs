namespace MVC_Shop.ViewModels.Users
{
    public class UserListVM
    {
        public List<UserVM> Users { get; set; } = new List<UserVM>();
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
    }
}