using FirstProject.DataAccess;
using FirstProject.DataAccess.Repository;
using FirstProject.DataAccess.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using FirstProject.Utility;
using Stripe;
using System.Net;
using FirstProject.DataAccess.DbInitializer;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
 builder.Services.AddControllersWithViews();
string connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<ApplicationDbContext>
    (options => options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

builder.Services.Configure<StripeSettings>(builder.Configuration.GetSection("Stripe"));

builder.Services.AddIdentity<IdentityUser, IdentityRole>().AddDefaultTokenProviders()
    .AddEntityFrameworkStores<ApplicationDbContext>();
//builder.Services.AddIdentity<IdentityUser, IdentityRole>().AddDefaultTokenProviders().
//    AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddScoped<IUnitOfWork,UnitOfWork>();  //Tis is for replacing ApplicationDbContext with ICategoryRepository
builder.Services.AddScoped<IDbInitializer, DbInitializer>();
builder.Services.AddSingleton<IEmailSender, EmailSender>();
builder.Services.AddRazorPages();
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = $"/Identity/Account/Login";
    options.LogoutPath = $"/Identity/Account/Logout";
    options.AccessDeniedPath = $"/Identity/Account/AccessDenied";
});
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(100);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

//builder.Services.AddHttpsRedirection(options =>
//{
//    options.RedirectStatusCode = (int)HttpStatusCode.TemporaryRedirect;
//    options.HttpsPort = 443;
//});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseStaticFiles();

StripeConfiguration.ApiKey = builder.Configuration.GetSection("Stripe:SecretKey").Get<string>(); //here we have assigned the global api key insisde our pipeline

SeedDatabase();
app.UseAuthentication(); //this will always come before authorization & it's called Middleware
app.UseAuthorization();
app.UseSession();
app.MapRazorPages();
app.MapControllerRoute(
    name: "default",
    pattern: "{area=Customer}/{controller=Home}/{action=Index}/{id?}");

app.Run();

void SeedDatabase()
{
    using (var scope = app.Services.CreateScope())
    {
        var dbInitializer = scope.ServiceProvider.GetRequiredService<IDbInitializer>();
        dbInitializer.Initialize();
    }
}