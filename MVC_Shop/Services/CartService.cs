using MVC_Shop;
using MVC_Shop.ViewModels.Cart;
using System.Text.Json;

namespace MVC_Shop.Services
{
    public static class CartService
    {
        public static List<CartItemVM> GetCartItems(this ISession session)
        {
            var data = session.GetString(Settings.CartKey);
            var items = data == null
                ? new List<CartItemVM>()
                : JsonSerializer.Deserialize<List<CartItemVM>>(data);
            items ??= new List<CartItemVM>();
            return items;
        }

        public static void AddToCart(this ISession session, int productId)
        {
            if (!session.ItemInCart(productId))
            {
                var items = session.GetCartItems();
                items.Add(new CartItemVM { ProductId = productId });
                var json = JsonSerializer.Serialize(items);
                session.SetString(Settings.CartKey, json);
            }
        }

        public static void RemoveFromCart(this ISession session, int productId)
        {
            if (session.ItemInCart(productId))
            {
                var items = session.GetCartItems();
                var itemIndex = items.FindIndex(i => i.ProductId == productId);
                if (itemIndex >= 0)
                {
                    items.RemoveAt(itemIndex);
                    var json = JsonSerializer.Serialize(items);
                    session.SetString(Settings.CartKey, json);
                }
            }
        }

        public static int CartCount(this ISession session)
        {
            return session.GetCartItems().Count();
        }

        public static bool ItemInCart(this ISession session, int productId)
        {
            var items = session.GetCartItems();
            return items.FirstOrDefault(i => i.ProductId == productId) != null;
        }
    }
}