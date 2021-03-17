using BlueModas.Database;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BlueModas.Application.Orders
{
    public class GetOrder
    {
        private ApplicationDbContext _ctx;

        public GetOrder(ApplicationDbContext ctx)
        {
            _ctx = ctx;
        }

        public class Response
        {
            public string OrderRef { get; set; }

            public string Name { get; set; }
            public string Email { get; set; }
            public string PhoneNumber { get; set; }

            public string Address { get; set; }
            public string City { get; set; }
            public string State { get; set; }
            public string Neighborhood { get; set; }
            public string ZipCode { get; set; }

            public IEnumerable<Product> Products { get; set; }

            public string TotalValue { get; set; }
        }

        public class Product
        {
            public string Name { get; set; }
            public string Description { get; set; }
            public string Price { get; set; }
            public int Quantity { get; set; }
            public string StockDescription { get; set; }
        }

        public Response Do(string reference) =>
            _ctx.Orders
                .Where(x => x.OrderRef == reference)
                .Include(x => x.OrderStocks)
                .ThenInclude(x => x.Stock)
                .ThenInclude(x => x.Product)
                .Select(x => new Response
                {
                    OrderRef = x.OrderRef,

                    Name = x.Name,
                    Email = x.Email,
                    PhoneNumber = x.PhoneNumber,
                    Address = x.Address,
                    City = x.City,
                    Neighborhood = x.Neighborhood,
                    State = x.State,
                    ZipCode = x.ZipCode,

                    Products = x.OrderStocks.Select(y => new Product
                    {
                        Name = y.Stock.Product.Name,
                        Description = y.Stock.Product.Description,
                        Price = $"R${y.Stock.Product.Price.ToString("N2")}",
                        Quantity = y.Quantity,
                        StockDescription = y.Stock.Description,
                    }),

                    TotalValue = x.OrderStocks.Sum(y => y.Stock.Product.Price).ToString("N2")
                })
                .FirstOrDefault();
        
    }
}
