using Brickwell.Data;
using Brickwell.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Configure Google SSO.
builder.Services.AddAuthentication().AddGoogle(googleOptions =>
{
    googleOptions.ClientId = builder.Configuration.GetSection("Authentication:Google:ClientId").Value;
    googleOptions.ClientSecret = builder.Configuration.GetSection("Authentication:Google:ClientSecret").Value;
});

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("AzureDBConnection") ?? throw new InvalidOperationException("Connection string 'AzureDBConnection' not found.");

// Add the secrets.json file.
builder.Configuration.AddJsonFile("secrets.json",
        optional: false,
        reloadOnChange: true);

// Deconstruct the connectionString so we can add properties.
var cb = new SqlConnectionStringBuilder(connectionString);
cb.UserID = builder.Configuration.GetSection("sql:username").Value;
cb.Password = builder.Configuration.GetSection("sql:password").Value;
connectionString = cb.ConnectionString;

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddScoped<IBrickwellRepository, EFBrickwellRepository>();
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<ApplicationRole>() // Add this
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddControllersWithViews();

builder.Services.Configure<IdentityOptions>(options =>
{
    // Password settings.
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequireUppercase = true;
    options.Password.RequiredLength = 12;
    options.Password.RequiredUniqueChars = 6; 

    // Lockout settings.
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.AllowedForNewUsers = true;

    // User settings.
    options.User.AllowedUserNameCharacters =
    "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
    options.User.RequireUniqueEmail = true;
});
    

// COOKIES!
builder.Services.Configure<CookiePolicyOptions>(options =>
{
    // This lambda determines whether user consent for non-essential 
    // cookies is needed for a given request.
    options.CheckConsentNeeded = context => true;

    options.MinimumSameSitePolicy = SameSiteMode.None;
    options.ConsentCookieValue = "true";

});

builder.Services.AddHsts(options => 
{
    options.Preload = true;
    options.IncludeSubDomains = true;
    options.MaxAge = TimeSpan.FromDays(60);
    // options.ExcludedHosts.Add("example.com");
    // options.ExcludedHosts.Add("www.example.com");
});

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession();

builder.Services.AddScoped<Cart>(sp => SessionCart.GetCart(sp));
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseSession();

app.Use(async (ctx, next) =>
{
    ctx.Response.Headers.Append("Content-Security-Policy", // TODO: Keep this updated!
                                "default-src 'self';" +
                                "connect-src 'self';" +
                                "script-src 'self'  'sha256-8KtGU/KMML7mQlFuTbhHjDCqVMP+dn8p4zGpTyJ0Tg8=' 'sha256-OvXc6U5FzIOWZLro8Icg57UWp+MXNVapQfho0/AQyCo=' 'sha256-7fWbrmA+Qx3CiwTN+bIxqIBcIUx4vVvg0pNvy+DykaY=' 'sha256-m1igTNlg9PL5o60ru2HIIK6OPQet2z9UgiEAhCyg/RU=' 'sha256-fIkC4b4crvv6N/++kl5qCd/nhTe0/JC7KANDL3YeODw=' 'sha256-w77ktvJB3FybpHrTd4ooqbairmYaAaYtYtuhBeiorvc=' 'sha256-9GlY3b6hPAM+8e87VxZOLd7mBlAQNTn31WsGviLLncc=' 'sha256-JE2J1ZcmHHspDIcaYOlWNACJ2bhnE6shsG8EeIBNOCw=' 'sha256-m1igTNlg9PL5o60ru2HIIK6OPQet2z9UgiEAhCyg/RU=' https://cdnjs.cloudflare.com https://code.jquery.com;" +
                                "img-src 'self' http://www.w3.org http://www.w3.org/2000/svg https://live.staticflickr.com/65535/;" +
                                "style-src 'self' 'unsafe-inline';") ;
    await next();
});


// Do the needful.
app.UseCookiePolicy();

app.UseRouting();

app.UseAuthorization();
// app.MapControllerRoute("pagenumandtype", "{projectType}/{pageNum}", new { Controller = "Home", action = "Products" });
// app.MapControllerRoute("projectType", "{productType}", new { Controller = "Home", action = "Products", pageNum = 1 });
// app.MapControllerRoute("pagination", "{pageNum}", new {Controller = "Home", action = "Products", pageNum = 1});
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();





app.Run();
