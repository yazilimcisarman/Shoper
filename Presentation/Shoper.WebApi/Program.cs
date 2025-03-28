using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Shoper.Application.Interfaces;
using Shoper.Application.Interfaces.ICartItemsRepository;
using Shoper.Application.Interfaces.ICartsRepository;
using Shoper.Application.Interfaces.IFavoritesRepository;
using Shoper.Application.Interfaces.IOrderRepository;
using Shoper.Application.Interfaces.IProductsRepository;
using Shoper.Application.Usecasess.AccountServices;
using Shoper.Application.Usecasess.CartItemSevices;
using Shoper.Application.Usecasess.CartServices;
using Shoper.Application.Usecasess.CategoryServices;
using Shoper.Application.Usecasess.ContactServices;
using Shoper.Application.Usecasess.CustomerServices;
using Shoper.Application.Usecasess.EmailServices;
using Shoper.Application.Usecasess.FavoritesServices;
using Shoper.Application.Usecasess.HelpServices;
using Shoper.Application.Usecasess.OrderItemServices;
using Shoper.Application.Usecasess.OrderServices;
using Shoper.Application.Usecasess.ProductServices;
using Shoper.Application.Usecasess.SubscriberServices;
using Shoper.Persistence.Context;
using Shoper.Persistence.Context.Identity;
using Shoper.Persistence.Repositories;
using Shoper.Persistence.Repositories.CartItemsRepository;
using Shoper.Persistence.Repositories.CartsRepository;
using Shoper.Persistence.Repositories.FavoritesRepository;
using Shoper.Persistence.Repositories.OrdersRepository;
using Shoper.Persistence.Repositories.ProductsRepository;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>();
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<IUserIdentityRepository, UserIdentityRepository>();
builder.Services.AddScoped<IProductsRepository, ProductsRepository>();
builder.Services.AddScoped<IOrderRepository, OrdersRepository>();
builder.Services.AddScoped<ICartsRepository, CartsRepository>();
builder.Services.AddScoped<ICartItemsRepository, CartItemsRepository>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ICartService, CartService>();
builder.Services.AddScoped<ICartItemService, CartItemService>();
builder.Services.AddScoped<IOrderServices, OrderServices>();
builder.Services.AddScoped<IOrderItemService, OrderItemService>();
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<ISubscriberService, SubscriberService>();
builder.Services.AddScoped<IHelpService, HelpService>();
builder.Services.AddScoped<IContactService, ContactService>();
builder.Services.AddScoped<ICustomerServices, CustomerServices>();
builder.Services.AddScoped<IFavoritesService, FavoritesService>();
builder.Services.AddScoped<ICategoryServices, CategoryServices>();
builder.Services.AddScoped<IFavoritesRepository, FavoritesRepository>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddDbContext<AppIdentityDbContext>(options =>
{
    var configuration = builder.Configuration;
    var connectionString = configuration.GetConnectionString("DefaultConnection");
    options.UseSqlServer(connectionString);
});
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login"; // Giri� yapmadan eri�ilmeye �al���ld���nda y�nlendirilecek sayfa
    });
builder.Services.AddIdentity<AppIdentityUser, AppIdentityRole>()
                .AddEntityFrameworkStores<AppIdentityDbContext>()
                .AddDefaultTokenProviders();
builder.Services.Configure<IdentityOptions>(options =>
{

    //options.Password.RequireDigit = true; //�ifre Say�sal karakteri desteklesin mi?
    options.Password.RequiredLength = 6;  //�ifre minumum karakter say�s�
    options.Password.RequireLowercase = true; //�ifre k���k harf olabilir
    options.Password.RequireLowercase = true; //�ifre b�y�k harf olabilir
    options.Password.RequireNonAlphanumeric = false; //Sembol bulunabilir

    options.Lockout.MaxFailedAccessAttempts = 5; //Kullan�c� ka� ba�ar�s�z giri�ten sonra sisteme giri� yapamas�n
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5); //Ba�ar�s�z giri� i�lemlerinden sonra ne kadar s�re sonra sisteme giri� hakk� tan�ns�n
    options.Lockout.AllowedForNewUsers = true; //Yeni �yeler i�in kilit sistemi ge�erli olsun mu

    options.User.RequireUniqueEmail = true; //Kullan�c� benzersiz e-mail adresine sahip olsun

    options.SignIn.RequireConfirmedEmail = false; //Kay�t i�lemleri i�in email onaylamas� zorunlu olsun mu?
    options.SignIn.RequireConfirmedPhoneNumber = false; //Telefon onay� olsun mu?
});
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
