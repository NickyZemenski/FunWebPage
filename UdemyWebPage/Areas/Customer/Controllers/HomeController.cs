using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using FunWebPage.Models;
using FunWebPage_DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace FunWebPage.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly IUnitOfWork _unitOfWork;

        public HomeController(ILogger<HomeController> logger,IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            IEnumerable<ProductModel> productList = _unitOfWork.Product.GetAll(includeProperties: "Category");
            return View(productList);
        }
        public IActionResult Details(int productId)
        {
            ShoppingCart cart = new()
            {
                Product = _unitOfWork.Product.Get(u => u.ProductId == productId, includeProperties: "Category"),
                Count = 1,
                ProductId = productId
            };


           
            return View(cart);
        }
        [HttpPost]
        [Authorize]
        public IActionResult Details(ShoppingCart shoppingCart)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
            shoppingCart.ApplicationUserId = userId;

            ShoppingCart shoppingCartFromDb = _unitOfWork.ShoppingCart.Get(
                u=> u.ApplicationUserId == userId
                && u.ProductId==shoppingCart.ProductId);

            if (shoppingCartFromDb != null)
            {
                shoppingCartFromDb.Count += shoppingCart.Count;
                _unitOfWork.ShoppingCart.Update(shoppingCartFromDb);
            }
            else
            {
                _unitOfWork.ShoppingCart.Add(shoppingCart);
            }

            TempData["success"] = "Shopping Cart Updated Successfully";
            _unitOfWork.Save();

            return RedirectToAction(nameof(Index));
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
