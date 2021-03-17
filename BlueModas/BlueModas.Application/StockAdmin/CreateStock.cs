using BlueModas.Database;
using BlueModas.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BlueModas.Application.StockAdmin
{
    public class CreateStock
    {
        private ApplicationDbContext _ctx;

        public CreateStock(ApplicationDbContext ctx)
        {
            _ctx = ctx;
        }

        public async Task<Response> Do(Request request)
        {
            var stock = new Stock
            {
                Quantity = request.Quantity,
                Description = request.Description,
                ProductId = request.ProductId
            };

            _ctx.Stock.Add(stock);
            await _ctx.SaveChangesAsync();

            return new Response
            {
                Id = stock.Id,
                Description = stock.Description,
                Quantity = stock.Quantity
            };
        }

        public class Request
        {
            public int ProductId { get; set; }
            public string Description { get; set; }
            public int Quantity { get; set; }
        }

        public class Response
        {
            public int Id { get; set; }
            public string Description { get; set; }
            public int Quantity { get; set; }
        }
    }
}
