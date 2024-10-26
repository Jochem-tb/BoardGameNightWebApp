using BGN.Domain.Repositories;
using BGN.Infrastructure;
using BGN.Infrastructure.Repositories;
using BGN.Services.Abstractions;
using BGN.Services;
using BGN.Domain;
using Microsoft.EntityFrameworkCore;
using BGN.Services.Mapping;
using Microsoft.AspNetCore.Identity;
using BGN.UI.Areas.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;


var builder = WebApplication.CreateBuilder(args);


builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin() 
               .AllowAnyHeader()
               .AllowAnyMethod();
    });

});

// Add services to the container.
//Automate the mapping of DTOs to Entities and reverse
builder.Services.AddAutoMapper(typeof(MappingProfile));

//Dependency Injection
builder.Services.AddScoped<IServiceManager, ServiceManager>();
builder.Services.AddScoped<IRepositoryManager, RepositoryManager>();
builder.Services.AddScoped<IUserService, UserService>();

//Add Email Sender to container
builder.Services.AddTransient<IEmailSender, EmailSenderConsole>();

builder.Services.AddHttpContextAccessor();

// Add session services
builder.Services.AddDistributedMemoryCache(); // Adds a default in-memory implementation of IDistributedCache
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Set session timeout
    options.Cookie.HttpOnly = true;                // Set the cookie to be HTTP only
    options.Cookie.IsEssential = true;              // Ensure the session cookie is sent even when the user hasn't consented to non-essential cookies
});

// Add services to the container.
builder.Services.AddControllersWithViews();

// Fetch the connection strings from environment variables if available
var azureEntitySqlConnectionString = Environment.GetEnvironmentVariable("AZURE_ENTITY_SQL_CONNECTION_STRING");
var azureIdentitySqlConnectionString = Environment.GetEnvironmentVariable("AZURE_IDENTITY_SQL_CONNECTION_STRING");

// Use the GitHub variable if available; otherwise, fallback to appsettings.json
string bgnDatabaseConnection = azureEntitySqlConnectionString ?? builder.Configuration.GetConnectionString("BGN_Database")!;
string bgnAccountsConnection = azureIdentitySqlConnectionString ?? builder.Configuration.GetConnectionString("BGN_Accounts")!;

//Database for Entities
builder.Services.AddDbContext<RepositoryDbContext>(options =>
{
    options.UseSqlServer(bgnDatabaseConnection);

});


//Database for Login.
builder.Services.AddDbContext<AuthDbContext>(options =>
    options.UseSqlServer(bgnAccountsConnection));


//Add Identity Services
builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
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

builder.Services.AddAuthorization(polictyBuilder =>
{
    //Policies / Claims Config
});

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Identity/Account/Login"; // Correct the login path
    options.AccessDeniedPath = "/Identity/Account/AccessDenied";
});

builder.Services.AddAuthorization();

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

// Enable CORS with the specified policy
app.UseCors("AllowAll");

app.UseAuthentication();
app.UseAuthorization();

// Add session middleware here
app.UseSession(); // Make sure to add this before UseEndpoints

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();

app.Run();
