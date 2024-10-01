using ShopConsoleApp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopConsoleApp.Entities
{
    public class Order
    {
        public int Id { get; set; }
        public List<OrderItem> OrderItems { get; set; } = new List<OrderItem> { };
        public decimal TotalAmount { get; set; }

        public void AddOrderItem(int productId, int qty, List<Product> products)
        {
            var item = OrderItems.Where(x => x.ProductId == productId).FirstOrDefault();

            if (item == null)
            {
                OrderItem itemNew = new OrderItem()
                {
                    ProductId = productId,
                    Quantity = qty
                };

                this.OrderItems.Add(itemNew);
            }
            else 
            {
                item.Quantity += qty;
            }
            
            var product = products.Where(x => x.Id == productId).FirstOrDefault();
            product.StockQuantity -= qty;
        }

        public void RemoveOrderItem(int productId, int qty, List<Product> products)
        {
            var item = OrderItems.Where(x => x.ProductId == productId).FirstOrDefault();

            if (item == null)
            {
                throw new Exception("В корзине нет такого товара.");
            }
            else
            {
                if (item.Quantity < qty)
                    throw new Exception("В корзине нет такого количества указанного товара.");
                else if (item.Quantity == qty)
                    OrderItems.Remove(item);
                else
                    item.Quantity -= qty;
            }

            var product = products.Where(x => x.Id == productId).FirstOrDefault();
            product.StockQuantity += qty;
        }      

    }
}
