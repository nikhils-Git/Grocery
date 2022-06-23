using GroceryShop.Infrastructure;
using GroceryShop.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GroceryShop.Areas.Admin.Controllers
{
    public class CartController : Controller
    {
        private readonly GroceryShopContext context;

        public CartController(GroceryShopContext context)
        {
            this.context = context;
        }

        // Get/cart
        public IActionResult Index()
        {
            List<Cartitem> cart = HttpContext.Session.GetJson<List<Cartitem>>("Cart") ?? new List<Cartitem>();

            CartViewModel cartVM = new CartViewModel
            {
                Cartitems = cart,
                GrandTotal = cart.Sum(x => x.Price * x.Quantity)
            };
            return View(cartVM);
        }
/*
        // Get/cart/add/5
        public async Task<IActionResult> Add(int id)
        {
            Product product = await context.Products.FindAsync(id);

            List<Cartitem> cart = HttpContext.Session.GetJson<List<Cartitem>>("Cart") ?? new List<Cartitem>();

            Cartitem cartitem = cart.Where(x => x.ProductId == id).FirstOrDefault();
           if(cartitem == null)
            {
                cart.Add(new Cartitem(product.Id));
            }
            else
            {
                cartitem.Quantity += 1;
            }

            HttpContext.Session.SetJson("Cart", cart);

            return RedirectToAction("Index");
        }
*/
        // GET /cart/add/5
        public async Task<IActionResult> Add(int id)
        {
            Product product = await context.Products.FindAsync(id);

            List<Cartitem> cart = HttpContext.Session.GetJson<List<Cartitem>>("Cart") ?? new List<Cartitem>();

            Cartitem cartitem = cart.Where(x => x.ProductId == id).FirstOrDefault();

            if (cartitem == null)
            {
                cart.Add(new Cartitem(product));
            }
            else
            {
                cartitem.Quantity += 1;
            }

            HttpContext.Session.SetJson("Cart", cart);

            if (HttpContext.Request.Headers["X-Requested-With"] != "XMLHttpRequest")
                return RedirectToAction("Index");

            return ViewComponent("SmallCart");
        }

        // GET /cart/decrease/5
        public IActionResult Decrease(int id)
        {
            List<Cartitem> cart = HttpContext.Session.GetJson<List<Cartitem>>("Cart");

            Cartitem cartitem = cart.Where(x => x.ProductId == id).FirstOrDefault();

            if (cartitem.Quantity > 1)
            {
                --cartitem.Quantity;
            }
            else
            {
                cart.RemoveAll(x => x.ProductId == id);
            }

            if (cart.Count == 0)
            {
                HttpContext.Session.Remove("Cart");
            }
            else
            {
                HttpContext.Session.SetJson("Cart", cart);
            }

            return RedirectToAction("Index");
        }

        // GET /cart/remove/5
        public IActionResult Remove(int id)
        {
            List<Cartitem> cart = HttpContext.Session.GetJson<List<Cartitem>>("Cart");

            cart.RemoveAll(x => x.ProductId == id);

            if (cart.Count == 0)
            {
                HttpContext.Session.Remove("Cart");
            }
            else
            {
                HttpContext.Session.SetJson("Cart", cart);
            }

            return RedirectToAction("Index");
        }

        // GET /cart/clear
        public IActionResult Clear()
        {
            HttpContext.Session.Remove("Cart");

            //return RedirectToAction("Page", "Pages");
            //return Redirect("/");
            if (HttpContext.Request.Headers["X-Requested-With"] != "XMLHttpRequest")
                return Redirect(Request.Headers["Referer"].ToString());

            return Ok();
        }

    }
}