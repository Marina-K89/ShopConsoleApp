using System;
using System.IO;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using ShopConsoleApp.Entities;

namespace ShopConsoleApp
{
    public class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            const string operationCodes =
                    "\n\n" +
                    " (1) Просмотр списка товаров в магазине\n" +
                    " (2) Добавление товара в корзину\n" +
                    " (3) Удаление товара из корзины\n" +
                    " (4) Просмотр текущего состояния корзины\n\n";

            Market market = new Market();

            while (true)
            {
                Console.WriteLine(operationCodes);
                try
                {
                    ProcessOperationCode(market);
                }
                catch (Exception ex)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(ex.Message);
                    Console.ResetColor();
                }
                
            }
        }

        private static void ProcessOperationCode(Market market)
        {
            var products = market.Products;

            const string PRODUCTS_LIST = "1";
            const string ADD_PRODUCT_TO_BASKET = "2";
            const string REMOVE_PRODUCT_FROM_BASKET = "3";
            const string VIEW_BASKET = "4";

            Console.Write("Код операции : ");
            string operationCode = Console.ReadLine();
            Console.WriteLine();

            switch (operationCode)
            {
                case PRODUCTS_LIST:
                    {
                        market.PrintAllProducts();
                        break;
                    }
                case ADD_PRODUCT_TO_BASKET:
                    {
                        Console.Write("Введите Id товара : ");

                        if(!Int32.TryParse(Console.ReadLine(), out int productId))
                            throw new Exception("Указан неверный Id");

                        Product product;
                        try
                        {
                            product = market.GetProductById(productId);
                            int qtyInStore = product.StockQuantity;

                            if (qtyInStore == 0)
                            {
                                throw new Exception("Товара с данным Id нет в наличии.");
                            }
                            else
                            {
                                Console.Write($"Введите количество товара (количество на складе - {qtyInStore}) : ");
                               
                                if (!Int32.TryParse(Console.ReadLine(), out int qty))
                                    throw new Exception("Указано неверное количество.");

                                if (qty <= 0)
                                {
                                    throw new Exception("Указано неверное количество.");
                                }
                                else if (qty > qtyInStore)
                                {
                                    throw new Exception($"Указанное количество {qty} превышает количество на складе : {qtyInStore}.");
                                }

                                market.Order.AddOrderItem(productId, qty, products); 
                            }

                        }
                        catch (Exception ex)
                        {
                            throw ex;
                        }
                        break;
                    }
                case REMOVE_PRODUCT_FROM_BASKET:
                    {
                        Console.Write("Введите Id товара : ");

                        if (!Int32.TryParse(Console.ReadLine(), out int productId))
                            throw new Exception("Указан неверный Id");

                        try
                        {
                            Console.Write($"Введите количество товара : ");

                            if (!Int32.TryParse(Console.ReadLine(), out int qty))
                                throw new Exception("Указано неверное количество.");

                            if (qty <= 0)
                            {
                                throw new Exception("Указано неверное количество.");
                            }

                            market.Order.RemoveOrderItem(productId, qty, products);

                        }
                        catch (Exception ex)
                        {
                            throw ex;
                        }
                        break;
                    }
                case VIEW_BASKET:
                    {
                        market.PrintOrder();
                        break;
                    }
                default:
                    {
                        throw new Exception("Вы ввели несуществующий код операции");                        
                    }
            }

        }       
    }
}