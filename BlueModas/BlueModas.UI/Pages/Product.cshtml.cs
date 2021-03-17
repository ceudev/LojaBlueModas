using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlueModas.Application.Cart;
using BlueModas.Application.Products;
using BlueModas.Database;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BlueModas.UI.Pages
{
    public class ProductModel : PageModel
    {
        private ApplicationDbContext _ctx;

        public ProductModel(ApplicationDbContext ctx)
        {
            _ctx = ctx;
        }

        [BindProperty]
        public AddToCart.Request CartViewModel { get; set; }

        public GetProduct.ProductViewModel Product { get; set; }

        public async Task<IActionResult> OnGet(string name)
        {
            Product = await new GetProduct(_ctx).Do(name.Replace("-", " "));
            if (Product == null)
                return RedirectToPage("Index");            
            else
                return Page();
        }

        public async Task<IActionResult> OnPost()
        {
           var stockAdded = await new AddToCart(HttpContext.Session, _ctx).Do(CartViewModel);

            if (stockAdded)
                return RedirectToPage("Cart");
            else
                return Page();
        }
    }
}