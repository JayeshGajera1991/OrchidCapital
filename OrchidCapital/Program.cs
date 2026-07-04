using OrchidCapital.Helper;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Security.Authentication;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using System.Globalization;
using Microsoft.AspNetCore.Localization;

var builder = WebApplication.CreateBuilder(args);
Utility.Initialize(builder.Configuration);

#region "Enable TLS"
builder.WebHost.ConfigureKestrel(options =>
{
    options.AddServerHeader = false; // Disable Server header
    // Configure HTTPS defaults
    options.ConfigureHttpsDefaults(httpsOptions =>
    {
        httpsOptions.SslProtocols = SslProtocols.Tls13 | SslProtocols.Tls12 | SslProtocols.Tls11 | SslProtocols.Tls;
    });
});
#endregion

#region Json Response

builder.Services.AddControllers(options =>
{
    options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true;
}).AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.IncludeFields = true;
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
    });
#endregion



// Add services to the container.
var mvcBuilder = builder.Services.AddControllersWithViews();

#if DEBUG
mvcBuilder.AddRazorRuntimeCompilation();
#endif

#region Set Session time out
builder.Services.AddAuthentication("Cookies")
    .AddCookie("Cookies", options =>
    {
        options.LoginPath = "/Login/Login";
        options.AccessDeniedPath = "/Login/Logout";
    });
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(60); // Set the session timeout duration
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
#endregion

builder.Services.Configure<CookiePolicyOptions>(options =>
{
    options.Secure = CookieSecurePolicy.Always;
});

builder.Services.Configure<ForwardedHeadersOptions>(opts =>
{
    opts.ForwardedHeaders = ForwardedHeaders.XForwardedFor
                         | ForwardedHeaders.XForwardedProto
                         | ForwardedHeaders.XForwardedHost;
    opts.KnownNetworks.Clear();
    opts.KnownProxies.Clear();
});
builder.Services.AddHttpsRedirection(opts => { opts.HttpsPort = 443; });
builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 6;
    options.Password.RequireUppercase = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireNonAlphanumeric = false;
});
builder.Services.AddScoped<RsaService>();
#region Cors Domain Policy
var AllowBasePath = builder.Configuration["AllowBasePath"]!;
if (AllowBasePath.ToLower() == "true")
{
    var PBasePath = builder.Configuration["BasePath"]!;
    builder.Services.AddCors(options =>
    {
        options.AddPolicy("CorsPolicy",
            builder => builder.WithOrigins(PBasePath)
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials());
    });
}
#endregion

builder.Services.AddHttpContextAccessor();

var app = builder.Build();

app.UseForwardedHeaders();
app.UseSession();
app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseRouting();
if (AllowBasePath.ToLower() == "true")
{
    app.UseCors("CorsPolicy");
}
app.UseAuthentication();
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
var culture = new CultureInfo("en-IN");

var localizationOptions = new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture(culture),
    SupportedCultures = new[] { culture },
    SupportedUICultures = new[] { culture }
};

app.UseRequestLocalization(localizationOptions);
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Login}/{action=Login}/{id?}");

app.Run();
