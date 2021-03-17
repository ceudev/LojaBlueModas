using BlueModas.Database;
using BlueModas.Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BlueModas.Application.Cart
{
    public class GetOrder
    {
        private ISession _session;
        private ApplicationDbContext _ctx;

        public GetOrder(ISession session, ApplicationDbContext ctx)
        {
            _session = session;
            _ctx = ctx;
        }

        public class Response
        {
            public IEnumerable<Product> Products { get; set; }
            public CustomerInformation CustomerInformation { get; set; }

            public int GetTotalCharge() => Products.Sum(x => x.Price * x.Quantity);
        }

        public class Product
        {
            public int ProductId { get; set; }
            public int Quantity { get; set; }
            public int StockId { get; set; }
            public int Price { get; set; }
        }

        public class CustomerInformation
        {

            public string Name { get; set; }
            public string Email { get; set; }
            public string PhoneNumber { get; set; }


            public string Address { get; set; }
            public string City { get; set; }
            public string State { get; set; }
            public string Neighborhood { get; set; }
            public string ZipCode { get; set; }
        }

        public Response Do()
        {
            var cart = _session.GetString("cart");            

            var cartList = JsonConvert.DeserializeObject<List<CartProduct>>(cart);

            var listOfProducts = _ctx.Stock
                .Include(x => x.Product)
                .Where(x => cartList.Any(y => y.StockId == x.Id))
                .Select(x => new Product
                {
                    ProductId = x.ProductId,
                    StockId = x.Id,
                    Price = (int) (x.Product.Price * 100),
                    Quantity = cartList.FirstOrDefault(y => y.StockId == x.Id).Quantity
                }).ToList();

            var customerInfoString = _session.GetString("customer-info");

            var customerInformation = JsonConvert.DeserializeObject<BlueModas.Domain.Models.CustomerInformation>(customerInfoString);

            return new Response
            {
                Products = listOfProducts,
                CustomerInformation = new CustomerInformation
                {
                    Name = customerInformation.Name,
                    Email = customerInformation.Email,
                    PhoneNumber = customerInformation.PhoneNumber,
                    Address = customerInformation.Address,
                    City = customerInformation.City,
                    State = customerInformation.State,
                    Neighborhood = customerInformation.Neighborhood,
                    ZipCode = customerInformation.ZipCode
                }
            };
        }
    }
}
