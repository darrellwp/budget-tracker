using BudgetTracker.Data;
using BudgetTracker.Data.Entities;
using BudgetTracker.Data.Repositories;
using BudgetTracker.Extensions;
using BudgetTracker.Middleware;
using BudgetTracker.Services;
using BudgetTracker.Settings;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("BudgetTrackerConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

// Configuration settings classes
builder.Services.Configure<ApplicationSettings>(builder.Configuration.GetSection(ApplicationSettings.SECTION_NAME));
builder.Services.Configure<SmtpSettings>(builder.Configuration.GetSection(SmtpSettings.SECTION_NAME));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

// Add the database context
builder.Services.AddDbContext<IAppDbContext, AppDbContext>(options =>
{
    options.UseSqlServer(connectionString);
});

// Add the UserIdentity default
builder.Services.AddDefaultIdentity<ApplicationUser>(options =>
{
    options.SignIn.RequireConfirmedAccount = true;
    options.Password.RequiredLength = 10;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.User.RequireUniqueEmail = true;
    options.Lockout.MaxFailedAccessAttempts = 3;
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
})
.AddEntityFrameworkStores<AppDbContext>();

// Add memory cache
builder.Services.AddMemoryCache(options =>
{
    options.SizeLimit = 1024;
});

// Standard services from .NET
builder.Services.AddControllers();
builder.Services.AddRazorPages();

// Middleware
builder.Services.AddScoped<HttpRequestLogMiddleware>();

// Repositories
builder.Services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<ITransactionRepository, TransactionRepository>();

// Custom services
builder.Services.AddScoped<ISmtpService, SmtpService>();
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IDashboardService, DashboardService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseHttpRequestLogging();
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthorization();

app.MapStaticAssets();
app.MapRazorPages()
   .WithStaticAssets();
app.MapControllers();

await app.RunAsync();
