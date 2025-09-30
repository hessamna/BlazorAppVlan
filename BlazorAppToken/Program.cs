using BalzorAppVlan.Components;
using BalzorAppVlan.Datas;
using BalzorAppVlan.Repository.BaseRepository;
using BalzorAppVlan.Services;
using BalzorAppVlan.Services.API.Controllers;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// 🔹 Configure SQL Server DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// 🔹 Configure Identity (includes default cookie schemes)
builder.Services.AddIdentity<ApplicationUser, ApplicationRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

// 🔹 Configure Identity Cookie
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/login";
    options.LogoutPath = "/logout";
    options.AccessDeniedPath = "/accessdenied";
    options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
    options.SlidingExpiration = true;
});

// 🔹 Authorization and AuthenticationStateProvider
builder.Services.AddAuthorization();
builder.Services.AddHttpContextAccessor();
builder.Services.AddCascadingAuthenticationState();
builder.Services.AddScoped<AuthenticationStateProvider, IdentityRevalidatingAuthenticationStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider, ServerAuthenticationStateProvider>();

// 🔹 Custom Accessor & Redirect Helper
builder.Services.AddScoped<IdentityUserAccessor>();
builder.Services.AddScoped<IdentityRedirectManager>();

// 🔹 Repositories
builder.Services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));

// Repositories

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ICompanyRepository, CompanyRepository>();
builder.Services.AddScoped<ISwitchRepository, SwitchRepository>();
builder.Services.AddScoped<IVlanRepository, VlanRepository>();
builder.Services.AddScoped<IDeviceInterfaceRepository, DeviceInterfaceRepository>();
builder.Services.AddScoped<INeighborRepository, NeighborRepository>();
builder.Services.AddScoped<IAuditLogRepository, AuditLogRepository>();
builder.Services.AddScoped<ISystemSettingRepository, SystemSettingRepository>();


// Services


builder.Services.AddScoped<ICompanyService, CompanyService>();
builder.Services.AddScoped<ISwitchService, SwitchService>();
builder.Services.AddScoped<IVlanService, VlanService>();
builder.Services.AddScoped<IDeviceInterfaceService, DeviceInterfaceService>();
builder.Services.AddScoped<INeighborService, NeighborService>();


;

// 🔹 Add MVC Controllers
builder.Services.AddControllersWithViews();

// 🔹 Blazor Server
builder.Services.AddRazorComponents().AddInteractiveServerComponents();

// 🔹 Scoped HTTP client for Blazor
builder.Services.AddScoped(sp =>
{
    var nav = sp.GetRequiredService<NavigationManager>();
    return new HttpClient { BaseAddress = new Uri(nav.BaseUri) };
});

var app = builder.Build();

// 🔹 Seed admin user and roles
using (var scope = app.Services.CreateScope())
{
    var userMgr = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
    var roleMgr = scope.ServiceProvider.GetRequiredService<RoleManager<ApplicationRole>>();
    await SeedData.InitializeAsync(userMgr, roleMgr);
}

// 🔹 Middleware pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseCookiePolicy();
app.UseAuthentication(); // Must come before Authorization
app.UseAuthorization();
app.UseAntiforgery();

// 🔹 Map routes
app.MapControllers();
app.MapRazorComponents<App>().AddInteractiveServerRenderMode();

app.Run();
