using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using time_off_management_app.Authentication;
using time_off_management_app.Components;
using time_off_management_app.Components.Account;
using time_off_management_app.Data;
using time_off_management_app.Services;
using time_off_management_app.Shared.Constants;
using time_off_management_app.TicketStoring;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents()
    .AddAuthenticationStateSerialization();

builder.Services.AddCascadingAuthenticationState();
builder.Services.AddScoped<IdentityRedirectManager>();
builder.Services.AddScoped<AuthenticationStateProvider, IdentityRevalidatingAuthenticationStateProvider>();

builder.Services.AddAuthentication(options =>
    {
        options.DefaultScheme = IdentityConstants.ApplicationScheme;
        options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
    })
    .AddScheme<AuthenticationSchemeOptions, ApiKeyAuthenticationHandler>(
    "ApiKey",
    options => { })
    .AddIdentityCookies();


builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("CanReview", policy =>
    {
        policy.RequireClaim(Permissions.Type, Permissions.FormsReview, Permissions.FormsReviewAll);
    });
});


var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddIdentityCore<ApplicationUser>(options =>
    {
        options.SignIn.RequireConfirmedAccount = false;
        options.Stores.SchemaVersion = IdentitySchemaVersions.Version3;
    })
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddSignInManager()
    .AddDefaultTokenProviders();

builder.Services.AddSingleton<IEmailSender<ApplicationUser>, IdentityNoOpEmailSender>();

builder.Services.AddHttpClient("Api", client =>
{
    client.BaseAddress = new Uri(builder.Configuration["Api:BaseUrl"]);
});

builder.Services.AddScoped<TimeOffService>();
builder.Services.AddScoped<ReasonsService>();
builder.Services.AddScoped<ApiKeyService>();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.HttpOnly = true;
    options.Cookie.SameSite = SameSiteMode.Lax; //Change to Strict
    options.ExpireTimeSpan = TimeSpan.FromMinutes(60); //Change to 30
    options.SlidingExpiration = true;
});

//Keyed stores
// builder.Services.AddKeyedSingleton<ITicketStore, (implementation class)>("implementation name");


builder.Services.AddSingleton<ITicketStoreResolver, TicketStoreResolver>();

var ticketStore = builder.Configuration["Authentication:TicketStore"];
if (!string.IsNullOrEmpty(ticketStore) && !ticketStore.Equals("Default", StringComparison.OrdinalIgnoreCase))
{
    builder.Services
        .AddOptions<CookieAuthenticationOptions>(IdentityConstants.ApplicationScheme)
        .Configure<ITicketStoreResolver>(
        (options, resolver) =>
        {
            options.SessionStore = resolver.Resolve(ticketStore);
        });
}


builder.Services.AddControllers();

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(time_off_management_app.Client._Imports).Assembly);

// Add additional endpoints required by the Identity /Account Razor components.
app.MapAdditionalIdentityEndpoints();

app.Run();
