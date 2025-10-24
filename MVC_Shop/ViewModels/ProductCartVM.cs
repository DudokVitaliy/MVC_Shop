namespace MVC_Shop.ViewModels
{
    public class ProductCartVM
    {
        public required Models.Product Product { get; set; }
        public int Count { get; set; } = 1;

        public double TotalPrice { get => Product == null ? 0 : Product.Price * Count; }
    }
}
