using BlueModas.Database;
using BlueModas.Domain.Models;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueModas.Application.Cart
{
    public class AddToCart
    {
        private ApplicationDbContext _ctx;
        private ISession _session;

        public AddToCart(ISession session, ApplicationDbContext ctx)
        {
            _ctx = ctx;
            _session = session;
        }

        public class Request
        {
            public int StockId { get; set; }
            public int Quantity { get; set; }
        }

        public async Task<bool> Do(Request request)
        {
            var stockOnHold = _ctx.StocksOnHold.Where(x => x.SessionId == _session.Id).ToList();
            var stockToHold = _ctx.Stock.Where(x => x.Id == request.StockId).FirstOrDefault();

            if (stockToHold.Quantity < request.Quantity)
            {
                return false;
            }

            _ctx.StocksOnHold.Add(new StockOnHold
            {
                StockId = stockToHold.Id,
                SessionId = _session.Id,
                Quantity = request.Quantity,
                ExpiresAt = DateTime.Now.AddMinutes(20)
            });

            stockToHold.Quantity -= request.Quantity;

            foreach (var stock in stockOnHold)
            {
                stock.ExpiresAt = DateTime.Now.AddMinutes(20);
            }

            await _ctx.SaveChangesAsync();

            var cartList = new List<CartProduct>();
            var stringObject = _session.GetString("cart");

            if (!string.IsNullOrEmpty(stringObject))
            {
                cartList = JsonConvert.DeserializeObject<List<CartProduct>>(stringObject);
            }

            if (cartList.Any(x => x.StockId == request.StockId))
            {
                cartList.Find(x => x.StockId == request.StockId).Quantity += request.Quantity;
            }
            else
            {
                cartList.Add(new CartProduct
                {
                    StockId = request.StockId,
                    Quantity = request.Quantity
                });
            }

            stringObject = JsonConvert.SerializeObject(cartList);            

            _session.SetString("cart", stringObject);

            return true;
        }
    }
}
