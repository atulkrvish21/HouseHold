using HHSurvey.Data;
using HHSurvey.Service;
using HHSurvey.Service.Implementation;
using HHSurvey.Service.Interface;
using log4net;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.OpenApi;
using System.Reflection;
using System.Text;
using Microsoft.AspNetCore.Builder;
using FluentValidation.AspNetCore;
using HHSurvey.Models;
using FluentValidation;
using Microsoft.Extensions.FileProviders;


var builder = WebApplication.CreateBuilder(args);

// -------------------- SERVICES --------------------
builder.Services.AddControllersWithViews();     // API + MVC + Angular

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IXmlValidationRuleService, XmlValidationRuleService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddFluentValidationClientsideAdapters();

// Register validators
builder.Services.AddValidatorsFromAssemblyContaining<HouseholdValidator>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular", policy =>
    {
        policy
            .AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod();
           // .AllowCredentials();
    });
});

builder.Logging.AddLog4Net("log4net.config");

builder.Services.Configure<FormOptions>(options =>
{
    options.MultipartBodyLengthLimit = 104_857_600; // 100 MB
});

// JWT Auth
var jwtSettings = builder.Configuration.GetSection("Jwt");
var secretKey = jwtSettings["SecretKey"];

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings["Issuer"],
            ValidAudience = jwtSettings["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
        };
    });

var app = builder.Build();
app.UseStaticFiles();
var uploadsPath = Path.Combine(builder.Environment.ContentRootPath, "Uploads");

// Check if directory exists to prevent crash
if (!Directory.Exists(uploadsPath))
{
    Directory.CreateDirectory(uploadsPath);
}

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(uploadsPath),
    RequestPath = "/Uploads"
});
// -------------------- MIDDLEWARE --------------------

app.UseMiddleware<ApiLoggingMiddleware>();

if (!app.Environment.IsDevelopment())
{
    app.UseHsts();
}



// app.UseHttpsRedirection();

app.UseRouting();
// CORS
app.UseCors("AllowAngular"); 
// Auth
app.UseAuthentication();
app.UseAuthorization();



// Controllers
app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");

// Angular SPA fallback (must be last)
app.MapFallbackToFile("index.html");

app.Run();
