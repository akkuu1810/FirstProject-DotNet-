using FirstProject.DataAccess.Repository.IRepository;
using FirstProject.Utility;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FirstProject.ViewComponents
{
    public class ShoppingCartViewComponent : ViewComponent
    {
        private readonly IUnitOfWork _unitOfwork;
        public ShoppingCartViewComponent(IUnitOfWork unitOfWork)
        {
            _unitOfwork = unitOfWork;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            if(claim != null)
            {
                if (HttpContext.Session.GetInt32(SD.SessionCart) != null)
                {
                    return View(HttpContext.Session.GetInt32(SD.SessionCart));
                }
                else
                {
                    HttpContext.Session.SetInt32(SD.SessionCart,
                        _unitOfwork.ShoppingCart.GetAll(u => u.ApplicationUserId == claim.Value).ToList().Count);
                    return View(HttpContext.Session.GetInt32(SD.SessionCart));
                }
            }
            else
            {
                HttpContext.Session.Clear();
                return View(0);
            }
        }
    }
}
