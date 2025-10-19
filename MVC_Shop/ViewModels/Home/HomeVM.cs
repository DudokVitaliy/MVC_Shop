namespace MVC_Shop.ViewModels.Home
{
    public class HomeVM
    {
        public ProductListVM ProductList { get; set; } = new ProductListVM();
        public IEnumerable<Models.Category> Categories { get; set; } = [];
        public string? CategoryName { get; set; }


    }
}
