using Microsoft.AspNetCore.Mvc;
using System.Linq;
using FunWebPage.Models;
using FunWebPage.DataAccess.Data;
using FunWebPage_DataAccess.Repository.IRepository;

namespace FunWebPage.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly IProductRepository _ProductRepository;
        public ProductController(IProductRepository db)
        {
            _ProductRepository = db;
        }
        public IActionResult Index()
        {
            List<ProductModel> objProductList = _ProductRepository.GetAll().ToList();
            return View(objProductList);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]

        public IActionResult Create(ProductModel obj)
        {

            if (ModelState.IsValid)
            {
                _ProductRepository.Add(obj);
                _ProductRepository.Save();
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
       
            ProductModel? ProductFromDb = _ProductRepository.Get(u => u.ProductId == ProductId);
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
                _ProductRepository.Update(obj);
                _ProductRepository.Save();
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


            ProductModel? ProductFromDb = _ProductRepository.Get(u => u.ProductId == ProductId); ;
            if (ProductFromDb == null)
            {
                return NotFound();
            }
            return View(ProductFromDb);
        }

        [HttpPost, ActionName("Delete")]

        public IActionResult DeletePOST(int? ProductId)
        {
            ProductModel? obj = _ProductRepository.Get(u => u.ProductId == ProductId);
 
            _ProductRepository.Delete(obj);
            _ProductRepository.Save();
            TempData["success"] = "Product deleted successfully";
            return RedirectToAction("Index");


        }
    }
}
