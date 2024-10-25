using BGN.Domain.Repositories;
using BGN.Infrastructure;
using BGN.Infrastructure.Repositories;
using BGN.Services.Mapping;
using BGN.WebService.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add CORS services
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowApi",
        builder => builder.WithOrigins("https://sswfr-jjl-webapp-cgcdfyctfgbmggep.northeurope-01.azurewebsites.net") // Replace with your Web App URL
                          .AllowAnyMethod()
                          .AllowAnyHeader());
});

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add services to the container.
//Automate the mapping of DTOs to Entities and reverse
builder.Services.AddAutoMapper(typeof(MappingProfile));

builder.Services.AddScoped<ApiKeyAuthFilter>();
builder.Services.AddTransient<IRepositoryManager, RepositoryManager>();

// Fetch the connection strings from environment variables if available
var azureEntitySqlConnectionString = Environment.GetEnvironmentVariable("AZURE_ENTITY_SQL_CONNECTION_STRING");
var azureIdentitySqlConnectionString = Environment.GetEnvironmentVariable("AZURE_IDENTITY_SQL_CONNECTION_STRING");

// Use the GitHub variable if available; otherwise, fallback to appsettings.json
string bgnDatabaseConnection = azureEntitySqlConnectionString ?? builder.Configuration.GetConnectionString("BGN_Database");
string bgnAccountsConnection = azureIdentitySqlConnectionString ?? builder.Configuration.GetConnectionString("BGN_Accounts");

//Database for Entities
builder.Services.AddDbContext<RepositoryDbContext>(options =>
{
    options.UseSqlServer(bgnDatabaseConnection);

});


//Database for Login.
builder.Services.AddDbContext<AuthDbContext>(options =>
    options.UseSqlServer(bgnAccountsConnection));

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
});

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

builder.Services.AddHttpContextAccessor();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Enable CORS with the specified policy
app.UseCors("AllowApi");

app.UseAuthorization();

app.MapControllers();

app.Run();
