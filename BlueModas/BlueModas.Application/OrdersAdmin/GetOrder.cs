using BlueModas.Database;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace Shop.Application.OrdersAdmin
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
            public int Id { get; set; }
            public string OrderRef { get; set; }
            public string StripeReference { get; set; }

            public string Name { get; set; }
            public string Email { get; set; }
            public string PhoneNumber { get; set; }

            public string Address { get; set; }
            public string City { get; set; }
            public string State { get; set; }
            public string Neighborhood { get; set; }
            public string ZipCode { get; set; }

            public IEnumerable<Product> Products { get; set; }
        }

        public class Product
        {
            public string Name { get; set; }
            public string Description { get; set; }
            public int Quantity { get; set; }
            public string StockDescription { get; set; }
        }

        public Response Do(int id) =>
            _ctx.Orders
                .Where(x => x.Id == id)
                .Include(x => x.OrderStocks)
                .ThenInclude(x => x.Stock)
                .ThenInclude(x => x.Product)
                .Select(x => new Response
                {
                    Id = x.Id,
                    OrderRef = x.OrderRef,
                    StripeReference = x.StripeReference,

                    Name = x.Name,                    
                    Email = x.Email,
                    PhoneNumber = x.PhoneNumber,
                    Address = x.Address,
                    City = x.City,
                    State = x.State,
                    Neighborhood = x.Neighborhood,
                    ZipCode = x.ZipCode,

                    Products = x.OrderStocks.Select(y => new Product
                    {
                        Name = y.Stock.Product.Name,
                        Description = y.Stock.Product.Description,
                        Quantity = y.Quantity,
                        StockDescription = y.Stock.Description
                    }),
                })
                .FirstOrDefault();
    }
}
