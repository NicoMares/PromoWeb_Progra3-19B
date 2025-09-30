using Microsoft.EntityFrameworkCore;
using Promos.Web.Data;
using Promos.Web.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<PromosDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("PromosDb")));
builder.Services.AddScoped<IPromoService, PromoService>();

var app = builder.Build();
if (!app.Environment.IsDevelopment()) app.UseExceptionHandler("/Home/Error");
app.UseStaticFiles();
app.UseRouting();
app.MapControllerRoute(name: "default", pattern: "{controller=Promo}/{action=Index}/{id?}");
app.Run();
