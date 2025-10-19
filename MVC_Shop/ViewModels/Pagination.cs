namespace MVC_Shop.ViewModels
{
    public class Pagination
    {
        public int Page { get; set; } = 1;
        public int PageCount { get; set; } = 1;
        public int PageSize { get; set; } = Settings.PaginationPageSize;
    }
}
