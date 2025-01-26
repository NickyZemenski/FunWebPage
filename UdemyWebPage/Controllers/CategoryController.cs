using Microsoft.AspNetCore.Mvc;
using System.Linq;
using FunWebPage.Models;
using FunWebPage.DataAccess.Data;

namespace FunWebPage.Controllers
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
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]     
        
        public IActionResult Create(CategoryModel obj)
        {
            if (obj.Name == obj.DisplayOrder.ToString())      // custom validation
            {
                ModelState.AddModelError("name", "Name and Display Number Cant match");
            }
            if (ModelState.IsValid)
            {
                _db.Categories.Add(obj);
                _db.SaveChanges();
                TempData["success"] = "Category created successfully";
                return RedirectToAction("Index");
            }
           
                return View();
            
        }
        public IActionResult Edit(int? categoryId)
        {
            if (categoryId == null || categoryId == 0) { 
                return NotFound();
            }
            //  CategoryModel? categoryFromDb = _db.Categories.Find(categoryId);    Both this and next line do the same thing just different ways to do so.
            //   CategoryModel? categoryFromDb = _db.Categories.Where(u=>u.CategoryId==categoryId).FirstOrDefault(); 
            CategoryModel? categoryFromDb = _db.Categories.FirstOrDefault(u=>u.CategoryId== categoryId); 
            if (categoryFromDb == null)
            {
                return NotFound(); 
            }
            return View(categoryFromDb);
        }

        [HttpPost]

        public IActionResult Edit(CategoryModel obj)
        {
            if (obj.Name == obj.DisplayOrder.ToString())      // custom validation
            {
                ModelState.AddModelError("name", "Name and Display Number Cant match");
            }
            if (ModelState.IsValid)
            {
                _db.Categories.Update(obj);
                _db.SaveChanges();
                TempData["success"] = "Category updated successfully";

                return RedirectToAction("Index");
            }

            return View();

        }
        public IActionResult Delete(int? categoryId)
        {
            if (categoryId == null || categoryId == 0)
            {
                return NotFound();
            }
            

            CategoryModel? categoryFromDb = _db.Categories.FirstOrDefault(u => u.CategoryId == categoryId);
            if (categoryFromDb == null)
            {
                return NotFound();
            }
            return View(categoryFromDb);
        }

        [HttpPost, ActionName("Delete")]

        public IActionResult DeletePOST(int? categoryId)
        {
            CategoryModel? obj = _db.Categories.Find(categoryId);
            if (obj.Name == obj.DisplayOrder.ToString())      // custom validation
            {
                ModelState.AddModelError("name", "Name and Display Number Cant match");
            }
            _db.Categories.Remove(obj);
            _db.SaveChanges();
            TempData["success"] = "Category deleted successfully";
            return RedirectToAction("Index");
          

        }
    }
}
