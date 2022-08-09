using DataAccessLayer.Concrete;
using EntityLayer.Concrete;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//identity i�in gerekli konfig�rasyon.
builder.Services.AddDbContext<Context>();
builder.Services.AddIdentity<WriterUser, WriterRole>().AddEntityFrameworkStores<Context>();
builder.Services.AddControllersWithViews();

//admin i�in Authenticated �zelli�ini gerekli k�lan konf. ayar�
builder.Services.AddMvc(config =>
{
    var policy = new AuthorizationPolicyBuilder()
    .RequireAuthenticatedUser()
    .Build();
    config.Filters.Add(new AuthorizeFilter(policy));
});

//admin sisteme authentica olmad�ysa istenilen sayfaya y�nlendirilmesi icin gerekli konf.
builder.Services.AddMvc();
//builder.Services.AddAuthentication(
//    CookieAuthenticationDefaults.AuthenticationScheme)
//    .AddCookie(x =>
//    {
//        x.LoginPath = "/AdminLogin/Index/";
//    });
//sisteme giren kullan�c�n�n ilgili ayarlar�
builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.HttpOnly = true;
    options.ExpireTimeSpan = TimeSpan.FromMinutes(100); //maks. 10 dk sistemde kals�n.
    options.AccessDeniedPath = "/ErrorPage/Index/";
    options.LoginPath = "/Writer/Login/Index/"; //kullan�c� login degilse bu sayfaya gitsin.
});
var app = builder.Build();


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}

app.UseStatusCodePagesWithReExecute("/ErrorPage/Error404/"); //hata sayfas� d�zenlemesi
app.UseStaticFiles();
//authentication i�in gerekli konfig�rasyon.
app.UseAuthentication();
app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

//area i�in gerekli konfig�rasyon.
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
      name: "areas",
      pattern: "{area:exists}/{controller=Default}/{action=Index}/{id?}"
    );
});
app.Run();
