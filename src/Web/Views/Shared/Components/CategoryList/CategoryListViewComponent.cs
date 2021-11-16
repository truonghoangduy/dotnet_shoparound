using System;
using Microsoft.AspNetCore.Mvc;
using Infrastructure.Data;
using ApplicationCore.Entities;
using System.Collections.Generic;
using System.Linq;

namespace Web.Views.Shared.Components.CategoryListViewComponent
{
    public class CategoryListViewComponent : ViewComponent
    {
        ZemartDBContext _context;

        public CategoryListViewComponent(ZemartDBContext context)
        {
            _context = context;
        }
        public IViewComponentResult Invoke()
        {
            List<Catergory> subCategory = _context.Catergories.Where((cat) => cat.ParrentId != null).ToList();
            List<Catergory> model = _context.Catergories.ToList().Where((cat) => cat.ParrentId == null).ToList();
            ViewBag.subCategory = subCategory;
            return View(model); // Nếu khác Default.cshtml thì trả về View("abc", product) cho abc.cshtml
        }
    }
}
