namespace MVC_Shop.ViewModels.Home
{
    public class HomeVM
    {
        public IEnumerable<Models.Product> Products { get; set; } = new List<Models.Product>();
        public IEnumerable<Models.Category> Categories { get; set; } = new List<Models.Category>();

    }
}
