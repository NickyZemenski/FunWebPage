using FunWebPage.Models;
using FunWebPage.Models.ViewModels;
using FunWebPage.Utility;
using FunWebPage_DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FunWebPage.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize]
    public class ShoppingCartController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        [BindProperty]
        public ShoppingCartVM ShoppingCartVM { get; set; }

        public ShoppingCartController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }



        public IActionResult Index()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            ShoppingCartVM = new()
            {
                ShoppingCartList = _unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == userId, includeProperties: "Product"),
                OrderHeader = new()
            };


            foreach (var shoppingCart in ShoppingCartVM.ShoppingCartList)
            {
                shoppingCart.Price = GetPriceBasedOnQuantity(shoppingCart);
                ShoppingCartVM.OrderHeader.OrderTotal += (shoppingCart.Price * shoppingCart.Count);
            }


            return View(ShoppingCartVM);
        }

        public IActionResult Summary()
        {

                var claimsIdentity = (ClaimsIdentity)User.Identity;
                var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

                ShoppingCartVM = new()
                {
                    ShoppingCartList = _unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == userId, includeProperties: "Product"),
                    OrderHeader = new()
                };

            ShoppingCartVM.OrderHeader.ApplicationUser = _unitOfWork.ApplicationUser.Get(u => u.Id == userId);


            ShoppingCartVM.OrderHeader.Name =ShoppingCartVM.OrderHeader.ApplicationUser.Name;
            ShoppingCartVM.OrderHeader.PhoneNumber =ShoppingCartVM.OrderHeader.ApplicationUser.PhoneNumber;
            ShoppingCartVM.OrderHeader.StreetAddress =ShoppingCartVM.OrderHeader.ApplicationUser.StreetAddress;
            ShoppingCartVM.OrderHeader.City =ShoppingCartVM.OrderHeader.ApplicationUser.City;
            ShoppingCartVM.OrderHeader.State =ShoppingCartVM.OrderHeader.ApplicationUser.State;
            ShoppingCartVM.OrderHeader.PostalCode =ShoppingCartVM.OrderHeader.ApplicationUser.PostalCode;

                foreach (var shoppingCart in ShoppingCartVM.ShoppingCartList)
                {
                    shoppingCart.Price = GetPriceBasedOnQuantity(shoppingCart);
                    ShoppingCartVM.OrderHeader.OrderTotal += (shoppingCart.Price * shoppingCart.Count);
                }

                return View(ShoppingCartVM);
        }

        [HttpPost]
        [ActionName("Summary")]
        public IActionResult SummaryPost()
        {

            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            ShoppingCartVM.ShoppingCartList = _unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == userId, includeProperties: "Product");
               
            

            ShoppingCartVM.OrderHeader.OrderDate = System.DateTime.Now;
            ShoppingCartVM.OrderHeader.ApplicationUserId = userId;

            ShoppingCartVM.OrderHeader.ApplicationUser = _unitOfWork.ApplicationUser.Get(u => u.Id == userId);


            foreach (var shoppingCart in ShoppingCartVM.ShoppingCartList)
            {
                shoppingCart.Price = GetPriceBasedOnQuantity(shoppingCart);
                ShoppingCartVM.OrderHeader.OrderTotal += (shoppingCart.Price * shoppingCart.Count);
            }

            if (ShoppingCartVM.OrderHeader.ApplicationUser.CompanyId.GetValueOrDefault() == 0)
            {
                ShoppingCartVM.OrderHeader.PaymentStatus = SD.PaymentStatusPending;
                ShoppingCartVM.OrderHeader.OrderStatus = SD.StatusPending;
            }
            else
            {
                ShoppingCartVM.OrderHeader.PaymentStatus = SD.PaymentStatusDelayedPayment;
                ShoppingCartVM.OrderHeader.OrderStatus = SD.StatusApproved;
            }
            _unitOfWork.OrderHeader.Add(ShoppingCartVM.OrderHeader);
            _unitOfWork.Save();

            foreach(var shoppingCart in ShoppingCartVM.ShoppingCartList)
            {
                OrderDetails orderDetail = new()
                {
                    ProductId = shoppingCart.ProductId,
                    OrderHeaderId = ShoppingCartVM.OrderHeader.OrderHeaderId,
                    Price = shoppingCart.Price,
                    Count = shoppingCart.Count

                };
                _unitOfWork.OrderDetail.Add(orderDetail);
                _unitOfWork.Save();
            }



            return View(ShoppingCartVM);
        }

        public IActionResult addSingleItemInCart(int shoppingCartId)
        {
            var shoppingCartFromDb = _unitOfWork.ShoppingCart.Get(u => u.ShoppingCartId == shoppingCartId);
            shoppingCartFromDb.Count += 1;
            _unitOfWork.ShoppingCart.Update(shoppingCartFromDb);
            _unitOfWork.Save();
            return RedirectToAction(nameof(Index));


        }

        public IActionResult removeSingleItemInCart(int shoppingCartId)
        {
            var shoppingCartFromDb = _unitOfWork.ShoppingCart.Get(u => u.ShoppingCartId == shoppingCartId);
            if (shoppingCartFromDb.Count <= 1)
            {
                _unitOfWork.ShoppingCart.Delete(shoppingCartFromDb);
            }
            else
            {
                shoppingCartFromDb.Count -= 1;
                _unitOfWork.ShoppingCart.Update(shoppingCartFromDb);
            }

            _unitOfWork.Save();
            return RedirectToAction(nameof(Index));


        }

        public IActionResult removeItemFromCart(int shoppingCartId)
        {
            var shoppingCartFromDb = _unitOfWork.ShoppingCart.Get(u => u.ShoppingCartId == shoppingCartId);

            _unitOfWork.ShoppingCart.Delete(shoppingCartFromDb);

            _unitOfWork.Save();
            return RedirectToAction(nameof(Index));


        }



        private double GetPriceBasedOnQuantity(ShoppingCart shoppingCart)
        {
            if (shoppingCart.Count <= 49)
            {
                return shoppingCart.Product.Price;
            }
            else if (shoppingCart.Count <= 99)
            {
                return shoppingCart.Product.Price50;
            }
            else
            {
                return shoppingCart.Product.Price100;
            }
        }

    }
}
