using MVC_Shop;
using MVC_Shop.Models;

public class CategoryListVM
{
    public IEnumerable<Category> Categories { get; set; } = new List<Category>();
    public int CurrentPage { get; set; } = 1;
    public int PageSize { get; set; } = Settings.PaginationPageSize;
    public int TotalItems { get; set; }
    public int TotalPages => (int)Math.Ceiling((double)TotalItems / PageSize);
}
