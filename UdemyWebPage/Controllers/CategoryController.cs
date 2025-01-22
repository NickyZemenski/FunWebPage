using Microsoft.AspNetCore.Mvc;
using UdemyWebPage.Data;
using UdemyWebPage.Models;

namespace UdemyWebPage.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContect _db;
        public CategoryController(ApplicationDbContect db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            List<CategoryModel> objCategoryList = _db.Categories.ToList();
            return View(objCategoryList);
        }
    }
}
