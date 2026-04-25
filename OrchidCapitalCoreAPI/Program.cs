using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.OpenApi.Models;
using Orchid.DataAccess;
using Orchid.DataModels;
using Orchid.EmailService.Interface;
using Orchid.EmailService.Repository;
using Orchid.TokenValidator;
using OrchidCapitalCoreAPI.Interface;
using OrchidCapitalCoreAPI.Repository;
using System.IO.Compression;
using System.Security.Authentication;

var builder = WebApplication.CreateBuilder(args);

#region Enable TLS
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

// Add services to the container.
builder.Services.AddControllers();

// Register scoped services
builder.Services.AddScoped<IDapperRepository, DapperRepository>();
// Login
builder.Services.AddScoped<ILoginRepository, LoginRepository>();
builder.Services.AddScoped<IAdminRepository, AdminRepository>();
#region configure email keys
builder.Services.Configure<EmailConfiguration>(builder.Configuration.GetSection("EMAIL-CONFIG"));

builder.Services.AddTransient<IEmailService, EmailService>();
#endregion

// Add API versioning and versioned API explorer
builder.Services.AddApiVersioning(config =>
{
    config.AssumeDefaultVersionWhenUnspecified = true;
    config.DefaultApiVersion = new ApiVersion(1, 0);
    config.ReportApiVersions = true;
});
builder.Services.AddVersionedApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VVV";
    options.SubstituteApiVersionInUrl = true;
});

// Add JWT authentication (custom extension method)
builder.Services.AddJwtAuthentication(builder.Environment, builder.Configuration);

// Configure Response Compression
builder.Services.AddResponseCompression(options =>
{
    options.EnableForHttps = true;
    options.Providers.Add<GzipCompressionProvider>();
    options.Providers.Add<BrotliCompressionProvider>();

});
builder.Services.Configure<GzipCompressionProviderOptions>(options =>
{
    options.Level = CompressionLevel.Fastest;
});

// Register Swagger services
builder.Services.AddSwaggerGen(options =>
{
    // Add security definitions for JWT and ApiKey
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] { }
        }
    });


});

// Add authorization services
builder.Services.AddAuthorization();

var app = builder.Build();

app.UseDeveloperExceptionPage();

app.UseSwagger();

app.UseSwaggerUI(options =>
{
    string swaggerJsonBasePath = string.IsNullOrWhiteSpace(options.RoutePrefix) ? "." : "..";
    var apiVersionProvider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();
    foreach (var description in apiVersionProvider.ApiVersionDescriptions)
    {
        string swaggerEndpoint = $"{swaggerJsonBasePath}/swagger/{description.GroupName}/swagger.json";
        options.SwaggerEndpoint(swaggerEndpoint, description.GroupName.ToUpperInvariant());
    }
});

app.UseHttpsRedirection();

// Use authentication and authorization middleware
app.UseAuthentication();
app.UseAuthorization();

// Enable response compression
app.UseResponseCompression();

// Map controllers to the endpoints
app.MapControllers();

app.Run();
