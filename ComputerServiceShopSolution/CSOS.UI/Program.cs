using ComputerServiceOnlineShop.Entities.Models.IdentityEntities;
using ComputerServiceOnlineShop.Services;
using CSOS.Core.Domain.InfrastructureServiceContracts;
using CSOS.Core.Domain.RepositoryContracts;
using CSOS.Core.ServiceContracts;
using CSOS.Core.Services;
using CSOS.Infrastructure.DbContext;
using CSOS.Infrastructure.Repositories;
using CSOS.UI.Helpers;
using CSOS.UI.Middleware;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

//Add Serilog
builder.Host.UseSerilog((context, loggerConfig) =>
    loggerConfig.ReadFrom.Configuration(context.Configuration)
);

builder.Services.AddHttpLogging(options =>
{
    options.LoggingFields = HttpLoggingFields.RequestProperties
                            | HttpLoggingFields.ResponsePropertiesAndHeaders;
});

// Add DbContext
builder.Services.AddDbContext<DatabaseContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ComputerServiceOnlineShop"),
        migrations => migrations.MigrationsAssembly("CSOS.Infrastructure")));

// Add Business-Logic Services to the container.
builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<IOfferService, OfferService>();
builder.Services.AddScoped<IPictureHandlerService, PictureHandlerService>();
builder.Services.AddScoped<ICountriesGetterService, CountryGetterService>();
builder.Services.AddScoped<ICategoryGetterService, CategoryGetterService>();
builder.Services.AddScoped<IDeliveryTypeGetterService, DeliveryTypeGetterService>();
builder.Services.AddScoped<IConditionGetterService, ConditionGetterService>();
builder.Services.AddScoped<ICartService, CartService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IAddressService, AddressService>();
builder.Services.AddScoped<IProductImageService, ProductImageService>();
builder.Services.AddScoped<ISortingOptionService, SortingOptionsService>();

//Add Helper Classes
builder.Services.AddScoped<OfferViewModelInitializer>();
builder.Services.AddScoped<PicturesValidatorHelper>();
builder.Services.AddScoped<IConfigurationReader, ConfigurationReader>();

//Add Unit of Work
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

//Add repositories
builder.Services.AddScoped<IOfferRepository, OfferRepository>();
builder.Services.AddScoped<IOfferDeliveryTypeRepository, OfferDeliveryTypeRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IProductImageRepository, ProductImageRepository>();
builder.Services.AddScoped<IAccountRepository, AccountRepository>();
builder.Services.AddScoped<IProductCategoryRepository, ProductCategoryRepository>();
builder.Services.AddScoped<IConditionRepository, ConditionRepository>();
builder.Services.AddScoped<IAddressRepository, AddressRepository>();
builder.Services.AddScoped<IDeliveryTypeRepository, DeliveryTypeRepository>();
builder.Services.AddScoped<ICountryRepository, CountryRepository>();
builder.Services.AddScoped<ICartRepository, CartRepository>();

//Enabling identity
builder.Services.AddIdentity<ApplicationUser, ApplicationRole>()
    .AddEntityFrameworkStores<DatabaseContext>()
    .AddDefaultTokenProviders();

builder.Services.AddAuthentication("Cookies")
    .AddCookie("Cookies", options => { options.LoginPath = "/Account/Login"; });
builder.Services.AddAuthorization(options =>
{
    //enforces authorization policy
    options.FallbackPolicy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser().Build();
});
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login";
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
    options.Cookie.SameSite = SameSiteMode.Lax;
    options.Cookie.HttpOnly = true;
});

builder.Services.AddSession(options =>
{
    options.Cookie.HttpOnly = true;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
    options.Cookie.SameSite = SameSiteMode.Lax;
});

builder.Services.AddAntiforgery(options =>
{
    options.Cookie.HttpOnly = true;
    options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
    options.Cookie.SameSite = SameSiteMode.Lax;
});
builder.Services.AddHttpContextAccessor();
builder.Services.AddControllersWithViews(options =>
{
    //for every form that uses POST,DELETE,PUT generate csrf token
    //this auto-adds [ValidateAntiForgeryToken] in certain controller actions
    options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
});

var app = builder.Build();
// Configure the HTTP request pipeline.

app.UseSerilogRequestLogging();
app.UseHttpLogging();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseHsts();
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseExceptionHandlingMiddleware();
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication(); //reads auth cookie and can extract data from it
app.UseAuthorization(); //validates access permissions of the user
app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();