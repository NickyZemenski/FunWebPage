using Microsoft.AspNetCore.Mvc;
using System.Linq;
using FunWebPage.Models;
using FunWebPage.DataAccess.Data;
using FunWebPage_DataAccess.Repository.IRepository;

namespace FunWebPage.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public CategoryController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            List<CategoryModel> objCategoryList = _unitOfWork.Category.GetAll().ToList();
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
                _unitOfWork.Category.Add(obj);
                _unitOfWork.Save();
                TempData["success"] = "Category created successfully";
                return RedirectToAction("Index");
            }

            return View();

        }
        public IActionResult Edit(int? categoryId)
        {
            if (categoryId == null || categoryId == 0)
            {
                return NotFound();
            }
            //  CategoryModel? categoryFromDb = _db.Categories.Find(categoryId);    Both this and next line do the same thing just different ways to do so.
            //   CategoryModel? categoryFromDb = _db.Categories.Where(u=>u.CategoryId==categoryId).FirstOrDefault(); 
            CategoryModel? categoryFromDb = _unitOfWork.Category.Get(u => u.CategoryId == categoryId);
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
                _unitOfWork.Category.Update(obj);
                _unitOfWork.Save();
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


            CategoryModel? categoryFromDb = _unitOfWork.Category.Get(u => u.CategoryId == categoryId); ;
            if (categoryFromDb == null)
            {
                return NotFound();
            }
            return View(categoryFromDb);
        }

        [HttpPost, ActionName("Delete")]

        public IActionResult DeletePOST(int? categoryId)
        {
            CategoryModel? obj = _unitOfWork.Category.Get(u => u.CategoryId == categoryId);
            if (obj.Name == obj.DisplayOrder.ToString())      // custom validation
            {
                ModelState.AddModelError("name", "Name and Display Number Cant match");
            }
            _unitOfWork.Category.Delete(obj);
            _unitOfWork.Save();
            TempData["success"] = "Category deleted successfully";
            return RedirectToAction("Index");


        }
    }
}
