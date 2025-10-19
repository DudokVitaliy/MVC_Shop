namespace MVC_Shop.ViewModels.Home
{
    public class ProductListVM
    {
        public IEnumerable<Models.Product> Products { get; set; } = [];
        public int ProductsCount => Products.Count();
        public Pagination? Pagination { get; set; } = new Pagination();

    }
}
