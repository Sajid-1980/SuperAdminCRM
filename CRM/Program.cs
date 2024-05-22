using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;

using SuperAdmin.Areas.Identity.Data;
using SuperAdmin.IdentityDataSeeder;
using OfficeOpenXml;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("SuperAdminContextConnection") ?? throw new InvalidOperationException("Connection string 'SuperAdminContextConnection' not found.");

builder.Services.AddDbContext<SuperAdminContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddIdentity<SuperAdminUser, IdentityRole>()
    .AddDefaultTokenProviders()
    .AddEntityFrameworkStores<SuperAdminContext>();

// Set the EPPlus license context
ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
// or .Commercial

// Register ZekliApiService
builder.Services.AddHttpClient();

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages().AddRazorRuntimeCompilation();
builder.Services.AddControllersWithViews().AddRazorPagesOptions(options => {
    options.Conventions.AddAreaPageRoute("Identity", "/Account/Login", "");
});
// Add routing
var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var serviceProvider = scope.ServiceProvider;
    await IdentityDataSeeders.SeedAdminUserAndRole(serviceProvider);
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    // Map route for areas
    endpoints.MapControllerRoute(
        name: "MyArea",
        pattern: "{area:exists}/{controller=Dashboard}/{action=Index}/{id?}");

    endpoints.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}");
});

app.MapRazorPages();
app.Run();
