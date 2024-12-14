using Microsoft.AspNetCore.Http.Features;
using Microsoft.EntityFrameworkCore;
using Tian_fullstack.Data;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
// Conection to the DB that stores all the info besides accounts
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("AppConnection")));
// Conntection to the DB that sotores all the info related to the accounts system
builder.Services.AddDbContext<UserDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("UserConnection")));
builder.Services.Configure<FormOptions>(options =>
{
    options.MultipartBodyLengthLimit = 52428800; // 50 MB
});

builder.Services.AddIdentity<Tian_fullstack.Areas.Account.Models.User, IdentityRole>().AddEntityFrameworkStores<UserDbContext>();
// Changes the requirements of the apssword all over the app
builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = true;
    options.Password.RequiredLength = 8;
    options.Password.RequiredUniqueChars = 1;
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

// Seeding roles and assiging admin role to me
// My account:
// Password: 19671611INga_
// Username: admin
using (var scope = app.Services.CreateScope())
{
    // Getting managers
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<Tian_fullstack.Areas.Account.Models.User>>();
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    // Creating roles
    var roles = new[] { "Admin", "User" };

    // Saving roles
    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role))
        {
            await roleManager.CreateAsync(new IdentityRole(role));
        }
    }

    var me = await userManager.FindByNameAsync("admin");
    if (!await userManager.IsInRoleAsync(me, "Admin"))
    {
        await userManager.AddToRoleAsync(me, "Admin");
    };
}

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
);

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Unregistered}/{action=Index}/{id?}");

app.Run();