using Microsoft.AspNetCore.Mvc;
using System.Linq;
using FunWebPage.Models;
using FunWebPage.DataAccess.Data;
using FunWebPage_DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FunWebPage.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {

        private readonly IUnitOfWork _unitOfWork;
        public ProductController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            List<ProductModel> objProductList = _unitOfWork.Product.GetAll().ToList();
          

            return View(objProductList);
        }
        public IActionResult Create()
        {

            IEnumerable<SelectListItem> CategoryList = _unitOfWork.Category.GetAll().Select(u => new SelectListItem
            {
                Text = u.Name,
                Value = u.CategoryId.ToString()
            });

            ViewBag.CategoryList = CategoryList;

            return View();
        }
        [HttpPost]

        public IActionResult Create(ProductModel obj)
        {

            if (ModelState.IsValid)
            {
                _unitOfWork.Product.Add(obj);
                _unitOfWork.Save();
                TempData["success"] = "Product created successfully";
                return RedirectToAction("Index");
            }

            return View();

        }
        public IActionResult Edit(int? ProductId)
        {
            if (ProductId == null || ProductId == 0)
            {
                return NotFound();
            }
       
            ProductModel? ProductFromDb = _unitOfWork.Product.Get(u => u.ProductId == ProductId);
            if (ProductFromDb == null)
            {
                return NotFound();
            }
            return View(ProductFromDb);
        }

        [HttpPost]

        public IActionResult Edit(ProductModel obj)
        {
   
            if (ModelState.IsValid)
            {
                _unitOfWork.Product.Update(obj);
                _unitOfWork.Save();
                TempData["success"] = "Product updated successfully";

                return RedirectToAction("Index");
            }

            return View();

        }
        public IActionResult Delete(int? ProductId)
        {
            if (ProductId == null || ProductId == 0)
            {
                return NotFound();
            }


            ProductModel? ProductFromDb = _unitOfWork.Product.Get(u => u.ProductId == ProductId); ;
            if (ProductFromDb == null)
            {
                return NotFound();
            }
            return View(ProductFromDb);
        }

        [HttpPost, ActionName("Delete")]

        public IActionResult DeletePOST(int? ProductId)
        {
            ProductModel? obj = _unitOfWork.Product.Get(u => u.ProductId == ProductId);

            _unitOfWork.Product.Delete(obj);
            _unitOfWork.Save();
            TempData["success"] = "Product deleted successfully";
            return RedirectToAction("Index");


        }
    }
}
