using Microsoft.AspNetCore.Mvc;
using System.Linq;
using FunWebPage.Models;
using FunWebPage.DataAccess.Data;
using FunWebPage_DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Mvc.Rendering;
using FunWebPage.Models.ViewModels;
using Microsoft.CodeAnalysis;
using FunWebPage.Utility;
using Microsoft.AspNetCore.Authorization;

namespace FunWebPage.Areas.Admin.Controllers
{
    [Area("Admin")]
  //  [Authorize(Roles = SD.Role_Admin)]
    public class CompanyController : Controller
    {

        private readonly IUnitOfWork _unitOfWork;

        
        public CompanyController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;            
        }
        public IActionResult Index()
        {
            List<CompanyModel> objCompanyList = _unitOfWork.Company.GetAll().ToList();


            return View(objCompanyList);
        }
        public IActionResult Create(int? CompanyId)  // can call this Upsert, but havent seen this professionally so ill leave Create for now
        {

        
      
            if (CompanyId == null || CompanyId == 0)
            {
                //create
                return View(new CompanyModel());
            }
            else
            {
                // update
               CompanyModel company = _unitOfWork.Company.Get(u => u.CompanyId == CompanyId);

                return View(company);
            }

            }
        [HttpPost]

        public IActionResult Create(CompanyModel Company)
        {

            if (ModelState.IsValid)
            {
                if (Company.CompanyId == 0)
                {
                    _unitOfWork.Company.Add(Company);
                }
                else
                {
                    _unitOfWork.Company.Update(Company);
                }
                _unitOfWork.Save();
                TempData["success"] = "Company created successfully";
                return RedirectToAction("Index");
           
            }
            else
            {
                return View(Company);
            }
           

        }
        public IActionResult Delete(int? CompanyId)
        {
            if (CompanyId == null || CompanyId == 0)
            {
                return NotFound();
            }


            CompanyModel? CompanyFromDb = _unitOfWork.Company.Get(u => u.CompanyId == CompanyId); ;
            if (CompanyFromDb == null)
            {
                return NotFound();
            }
            return View(CompanyFromDb);
        }

        [HttpPost, ActionName("Delete")]

        public IActionResult DeletePOST(int? CompanyId)
        {
            CompanyModel? obj = _unitOfWork.Company.Get(u => u.CompanyId == CompanyId);

           

            _unitOfWork.Company.Delete(obj);
            _unitOfWork.Save();
            TempData["success"] = "Company deleted successfully";
            return RedirectToAction("Index");


        }
    }
}
