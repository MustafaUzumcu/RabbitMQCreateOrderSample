using OrderDB.Models;
using System.Collections.Generic;

namespace OrderDB.Context
{
    public class OrderContext
    {
        private static List<Product> _products;

        public static List<Product> Products
        {
            get { return GetProduts(); }
            set { _products = value; }
        }

        public OrderContext() { }

        private static List<Product> GetProduts()
        {
            return new List<Product>
                {
                    new Product
                    {
                        Id = 1,
                        Name = "X Drone",
                        AvailableStock = 8
                    },
                    new Product
                    {
                        Id = 2,
                        Name = "Y Phone",
                        AvailableStock = 12
                    },
                    new Product
                    {
                        Id = 3,
                        Name = "Z Laptop",
                        AvailableStock = 5
                    }
                };
        }
    }
}