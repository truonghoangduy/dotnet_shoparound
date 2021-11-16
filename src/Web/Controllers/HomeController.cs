using ApplicationCore.Entities;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;


namespace Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ZemartDBContext _context;

        public HomeController(ILogger<HomeController> logger, ZemartDBContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            var Catergories = _context.Catergories.ToList().Where((cat) => cat.ParrentId == null).ToList();
            var FeatureProduct = _context.Products.ToList().TakeLast(3);
            var Products = _context.Products.ToList();
            ViewBag.FeatureProduct = FeatureProduct;
            ViewBag.Products = Products;
            ViewBag.Catergories = Catergories;
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
