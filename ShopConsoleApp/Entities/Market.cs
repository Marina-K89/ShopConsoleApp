using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopConsoleApp.Entities
{
    public class Market
    {
        public List<Product> Products { get; set; }
        public Order Order { get; set; }

        public Market()
        {
            Products = GetAllProducts();
            Order = new Order();
        }

        public static List<Product> GetAllProducts()
        {
            string jsonFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "products.json");
            string json = File.ReadAllText(jsonFilePath);

            var products = JsonConvert.DeserializeObject<List<Product>>(json);

            return products;
        }

        public Product GetProductById(int productId)
        {
            var product = Products.FirstOrDefault(p => p.Id == productId);
            if (product == null)
                throw new Exception("Указан неверный Id");

            return product;
        }

        public void PrintAllProducts()
        {
            Console.WriteLine("   Id |                          Name  |      Price | Currency | Quantity | ");
            Console.WriteLine("---------------------------------------------------------------------------");
            foreach (var product in Products)
            {
                Console.WriteLine($"{product.Id,5} | {product.Name,30} | {product.Price,10} | {product.Currency,8} | {product.StockQuantity,8} |");
            } 
        }

        public void PrintOrder()
        {
            decimal total = 0m;
            Console.WriteLine("   Id |                          Name  |      Price | Currency | Quantity | ");
            Console.WriteLine("---------------------------------------------------------------------------");
            foreach (var orderItem in Order.OrderItems)
            {
                var product = GetProductById(orderItem.ProductId);
                Console.WriteLine($"{product.Id,5} | {product.Name,30} | {product.Price,10} | {product.Currency,8} | {orderItem.Quantity,8} |");
                total += (orderItem.Quantity * product.Price);
            }
            Console.WriteLine("---------------------------------------------------------------------------");
            Console.WriteLine($"Итого: {total, 55}");

        }

    }
}
