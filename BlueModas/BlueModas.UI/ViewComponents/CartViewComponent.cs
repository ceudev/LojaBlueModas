using BlueModas.Application.Cart;
using BlueModas.Database;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlueModas.UI.ViewComponents
{
    public class CartViewComponent : ViewComponent
    {
        private ApplicationDbContext _ctx;

        public CartViewComponent(ApplicationDbContext ctx)
        {
            _ctx = ctx;
        }

        public IViewComponentResult Invoke(string view = "Default")
        {
            if (view == "Small")
            {
                var totalPrice = new GetCart(HttpContext.Session, _ctx).Do().Sum(x => x.RealPrice * x.Quantity);
                return View(view, $"R${totalPrice}");
            }

            return View(view, new GetCart(HttpContext.Session, _ctx).Do());
        }
    }
}
