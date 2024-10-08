using BGN.Domain.Repositories;
using BGN.Infrastructure;
using BGN.Infrastructure.Repositories;
using BGN.Services.Abstractions;
using BGN.Services;
using BGN.Domain;
using Microsoft.EntityFrameworkCore;
using BGN.Services.Mapping;
using Microsoft.AspNetCore.Identity;
using BGN.UI.Areas.Identity.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
//Automate the mapping of DTOs to Entities and reverse
builder.Services.AddAutoMapper(typeof(MappingProfile));

//Dependency Injection
builder.Services.AddScoped<IServiceManager, ServiceManager>();
builder.Services.AddScoped<IRepositoryManager, RepositoryManager>();


// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<RepositoryDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("BGN_Database")));


//Database for Login.
builder.Services.AddDbContext<AuthDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("BGN_Accounts")));

//builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<AuthDbContext>();

//Add Identity Services
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    //Identity Password options --> The annoying onces
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequireUppercase = true;
    options.Password.RequiredLength = 6;
})
    .AddEntityFrameworkStores<AuthDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddRazorPages();

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

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();

app.Run();
