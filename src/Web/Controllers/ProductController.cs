using System;
using System.Linq;
using ApplicationCore.Entities;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Web.Controllers
{
    public class ProductController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ShopDBContext _context;

        public ProductController(ILogger<HomeController> logger, ShopDBContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index(int id)
        {
            var Product = _context.Products.Find(id);
            return View(Product);
        }




        public IActionResult ProductByCategory(int catergoryId)
        {
            // Filter by cateroryID
            // TODO LearnInclude Eger Loading
            var ProductsByCat = _context.Products
            .Where(product => product.CatergoryID == catergoryId).Include(product => product.Catergory).ToList();
            // .ToList();
            // Pro

            // UI Things
            ViewBag.CurrentCatergoryID = catergoryId;
            var Catergories = _context.Catergories.ToList().Where((cat) => cat.ParrentId == null).ToList();
            ViewBag.Catergories = Catergories;
            //TODO change to using ViewModel [including variable using by this scope]
            return View(ProductsByCat);
        }



    }
}
