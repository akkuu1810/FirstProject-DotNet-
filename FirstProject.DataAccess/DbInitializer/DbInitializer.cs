using FirstProject.DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using FirstProject.Utility;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FirstProject.Models;

namespace FirstProject.DataAccess.DbInitializer
{
    public class DbInitializer : IDbInitializer
    {   
            private readonly UserManager<IdentityUser> _userManager;
            private readonly RoleManager<IdentityRole> _roleManager;
            private readonly ApplicationDbContext _db;

            public DbInitializer(
                UserManager<IdentityUser> userManager,
                RoleManager<IdentityRole> roleManager,
                ApplicationDbContext db)
            {
            
                _userManager = userManager;
                _db = db;
                _roleManager = roleManager;
            }
            public void Initialize()
        {
            try
            {
                if (_db.Database.GetPendingMigrations().Count() > 0)
                {
                    _db.Database.Migrate();
                }
            }  
            catch (Exception ex)
            {

            }

            if (!_roleManager.RoleExistsAsync(SD.Role_Admin).GetAwaiter().GetResult())
            {
                _roleManager.CreateAsync(new IdentityRole(SD.Role_Admin)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(SD.Role_Employee)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(SD.Role_User_Indi)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(SD.Role_User_Comp)).GetAwaiter().GetResult();

                _userManager.CreateAsync(new ApplicationUser
                {
                    UserName = "admin2@gmail.com",
                    Email = "admin2@gmail.com",
                    Name = "Admin",
                    PhoneNumber = "1111122222",
                    StreetAddress = "Amd",
                    State = "Gujarat",
                    PostalCode = "380015",
                    City = "Amd"
                }, "Abc@123").GetAwaiter().GetResult();

                ApplicationUser user = _db.ApplicationUsers.FirstOrDefault(u => u.Email == "admin2@gmail.com");

                _userManager.AddToRoleAsync(user, SD.Role_Admin).GetAwaiter().GetResult();
            }
            return;

        }
    }
}
