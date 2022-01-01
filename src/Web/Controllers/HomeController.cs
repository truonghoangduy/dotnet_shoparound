using ApplicationCore.Entities;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        private readonly ShopDBContext _context;

        public HomeController(ILogger<HomeController> logger, ShopDBContext context)
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
            // ViewBag.Products = Products;
            ViewBag.Catergories = Catergories;
            return View(Products);
        }
        [HttpGet]
        [Route("query")]
        public IActionResult Query(int catergoryId, string keyword = "")
        {
            IQueryable<Product> query = _context.Products;
            bool hasCatID = ((catergoryId > 0) ? true : false);
            bool haskeyWord = ((keyword != null && keyword.Length > 0) ? true : false);

            if (hasCatID && haskeyWord)
            {
                query = from s in _context.Products
                        where EF.Functions.Like(s.Name.ToLower(), "%" + keyword.ToLower() + "%") && s.CatergoryID == catergoryId
                        select s;
            }
            else if (haskeyWord)
            {
                query = from s in _context.Products
                        where EF.Functions.Like(s.Name.ToLower(), "%" + keyword.ToLower() + "%")
                        select s;
            }
            else if (hasCatID)
            {
                query = _context.Products.Where(x => x.Catergory.ID == catergoryId);
            }

            var Product = query.ToList();
            var Catergories = _context.Catergories.ToList().Where((cat) => cat.ParrentId == null).ToList();
            ViewBag.Catergories = Catergories;
            return View("Index", Product);
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
