using System;
using System.Collections.Generic;
using System.Linq;
using ApplicationCore.Entities;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;

namespace Web.Views.Shared.Components.Search
{
    public class SearchViewComponent : ViewComponent
    {
        ZemartDBContext _context;

        public SearchViewComponent(ZemartDBContext context)
        {
            _context = context;
        }
        public IViewComponentResult Invoke()
        {
            List<Catergory> model = _context.Catergories.ToList().Where((cat) => cat.ParrentId == null).ToList();

            return View(model); // Nếu khác Default.cshtml thì trả về View("abc", product) cho abc.cshtml
        }
    }
}
