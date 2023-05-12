﻿using FirstProject.DataAccess;
using FirstProject.DataAccess.Repository;
using FirstProject.DataAccess.Repository.IRepository;
using FirstProject.Models;
using FirstProject.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FirstProject.Areas.Admin.Controllers
   
{
    [Area("Admin")]
    public class CompanyController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
      
        public CompanyController(IUnitOfWork unitOfWork) //Implementation of ApplicationDbContext & will populate db with this implementation
        {
            _unitOfWork = unitOfWork;
            
        }
        public IActionResult Index()
        {
           
            return View();
        }
        //EDIT
        public IActionResult Upsert(int? id) //Here we will retrieve the Id
        {
            Company company = new();
            
            if (id == null || id == 0)
            {
                //create product
               // ViewBag.CategoryList = CategoryList;
               // ViewData["CoverTypeList"] = CoverTypeList;
                return View(company);
            }
            else
            {
                company = _unitOfWork.Company.GetFirstOrDefault(u=>u.Id==id);
                return View(company);
                //update product

            }
        
            
        }

        //UPDATE
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(Company obj, IFormFile? file)
        {
           
            if (ModelState.IsValid)
            {
              
                if (obj.Id == 0)
                {

                    _unitOfWork.Company.Add(obj);
                    TempData["success"] = "Company created Successfully";
                }
                else
                {
                    _unitOfWork.Company.Update(obj);
                    TempData["success"] = "Company updated Successfully";
                }
                _unitOfWork.Save();
               
                return RedirectToAction("Index");
            }
            return View(obj);
        }

       
       
        #region API CALLS
        [HttpGet]
        public IActionResult GetAll()
        {
            var companyList = _unitOfWork.Company.GetAll();
            return Json(new { data = companyList });
        }

        [HttpDelete]
        
        public IActionResult Delete(int? id)
        {
           var obj = _unitOfWork.Company.GetFirstOrDefault(u => u.Id==id);
            if(obj == null)
            {
                return Json(new {suceess =false, message ="Error while deleting"});
            }

            _unitOfWork.Company.Remove(obj);
            _unitOfWork.Save();
            return Json(new { success = true, message = "Delete Successful" });
           
        }
        #endregion
    }
}
