using FirstProject.DataAccess;
using FirstProject.DataAccess.Repository.IRepository;
using FirstProject.Models;
using FirstProject.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FirstProject.Areas.Admin.Controllers
{
    [Area("Admin")]
	[Authorize(Roles = SD.Role_Admin)]
	public class CoverTypeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public CoverTypeController(IUnitOfWork unitOfWork) //Implementation of ApplicationDbContext & will populate db with this implementation
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            IEnumerable<CoverType> objCoverTypeList = _unitOfWork.CoverType.GetAll(); //This will retrieve all the categories from the database 
            return View(objCoverTypeList);
        }

        //GET
        public IActionResult Create()
        {
            return View();
        }
        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CoverType obj)
        {
          
            if (ModelState.IsValid)
            {
                _unitOfWork.CoverType.Add(obj);
                _unitOfWork.Save();
                TempData["success"] = "CoverType created Successfully";
                return RedirectToAction("Index");
            }
            return View(obj);
        }

        //EDIT
        public IActionResult Edit(int? id) //Here we will retrieve the Id
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            // var categoryFromDb = _db.Categories.Find(id);
            var CoverTypeFromDbFirst = _unitOfWork.CoverType.GetFirstOrDefault(u => u.Id == id); //Here we are passing the generi Object(u)
                                                                                               // var categoryFromDbSingle = _db.Categories.SingleOrDefault(u => u.Id == id);

            if (CoverTypeFromDbFirst == null)
            {
                return NotFound();
            }
            return View(CoverTypeFromDbFirst);
        }

        //UPDATE
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(CoverType obj)
        {
           
            if (ModelState.IsValid)
            {
                _unitOfWork.CoverType.Update(obj);
                _unitOfWork.Save();
                TempData["success"] = "CoverType updated Successfully";
                return RedirectToAction("Index");
            }
            return View(obj);
        }

        //DELETE
        public IActionResult Delete(int? id)
        {

            if (id == null || id == 0)
            {
                return NotFound();
            }
            // var categoryFromDb = _db.Categories.Find(id);
            var CoverTypeFromDbFirst = _unitOfWork.CoverType.GetFirstOrDefault(u => u.Id == id); //Here we are passing the generi Object(u)
            // var categoryFromDbSingle = _db.Categories.SingleOrDefault(u => u.Id == id);

            if (CoverTypeFromDbFirst == null)
            {
                return NotFound(CoverTypeFromDbFirst);
            }
            return View(CoverTypeFromDbFirst);
        }
        //DELETE
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePOST(int? id)
        {
            var obj = _unitOfWork.CoverType.GetFirstOrDefault(u => u.Id == id);
            if (obj == null)
            {
                return NotFound();
            }
            _unitOfWork.CoverType.Remove(obj);
            _unitOfWork.Save();
            TempData["success"] = "CoverType deleted Successfully";
            return RedirectToAction("Index");
        }
    }
}
