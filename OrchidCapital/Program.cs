using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.DependencyInjection;
using OrchidCapital.Helper;
using System.Security.Authentication;
using System.Text.Json;
using System.Text.Json.Serialization;

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
builder.Services.AddControllers()
    .AddJsonOptions(options =>
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
app.UseAuthorization();
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

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
