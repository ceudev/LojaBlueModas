﻿using BlueModas.Database;
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
    public class GetCart
    {
        private ISession _session;
        private ApplicationDbContext _ctx;

        public GetCart(ISession session, ApplicationDbContext ctx)
        {
            _session = session;
            _ctx = ctx;
        }

        public class Response
        {
            public string Name { get; set; }
            public string Price { get; set; }
            public decimal RealPrice { get; set; }
            public int Quantity { get; set; }

            public int StockId { get; set; }
        }

        public IEnumerable<Response> Do()
        {
            var stringObject = _session.GetString("cart");

            if (string.IsNullOrEmpty(stringObject))
                return new List<Response>();
            

            var cartList = JsonConvert.DeserializeObject<List<CartProduct>>(stringObject);

            var response = _ctx.Stock
                .Include(x => x.Product)
                .Where(x => cartList.Any(y => y.StockId == x.Id))
                .Select(x => new Response
                {
                    Name = x.Product.Name,
                    Price = $"R${x.Product.Price.ToString("N2")}",
                    RealPrice = x.Product.Price,
                    StockId = x.Id,
                    Quantity = cartList.FirstOrDefault(y => y.StockId == x.Id).Quantity
                })
                .ToList();

            return response;
        }
    }
}
