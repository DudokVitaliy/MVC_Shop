namespace MVC_Shop.ViewModels.Product
{
    public class ProductVM
    {
        public IEnumerable<Models.Product> Products { get; set; } = new List<Models.Product>();
        public int CurrentPage { get; set; } = 1;
        public int PageSize { get; set; } = Settings.PaginationPageSize;
        public int TotalItems { get; set; }

        public int TotalPages => (int)Math.Ceiling((double)TotalItems / PageSize);
    }
}
