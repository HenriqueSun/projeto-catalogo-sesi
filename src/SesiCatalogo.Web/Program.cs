using System.Threading.RateLimiting;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using SesiCatalogo.Web.Catalog;
using SesiCatalogo.Web.Data;

var builder = WebApplication.CreateBuilder(args);

var databasePath = DatabaseInitializer.ResolvePath(builder.Configuration, builder.Environment);

builder.Services.AddDbContext<CatalogDbContext>(options =>
    options.UseSqlite(DatabaseInitializer.ConnectionString(databasePath, SqliteOpenMode.ReadOnly)));

builder.Services.AddProblemDetails();
builder.Services.AddHealthChecks();

var permitsPerMinute = builder.Configuration.GetValue("RateLimiting:PermitsPerMinute", 120);
builder.Services.AddRateLimiter(options =>
{
    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
    options.AddPolicy(CatalogEndpoints.RateLimitPolicy, context =>
        RateLimitPartition.GetFixedWindowLimiter(
            context.Connection.RemoteIpAddress?.ToString() ?? "unknown",
            _ => new FixedWindowRateLimiterOptions
            {
                PermitLimit = permitsPerMinute,
                Window = TimeSpan.FromMinutes(1),
                QueueLimit = 0,
            }));
});

var app = builder.Build();

DatabaseInitializer.Migrate(databasePath);

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler();
    app.UseHsts();
}

app.UseStatusCodePages();

app.Use(async (context, next) =>
{
    var headers = context.Response.Headers;
    headers.XContentTypeOptions = "nosniff";
    headers.XFrameOptions = "DENY";
    headers["Referrer-Policy"] = "strict-origin-when-cross-origin";
    headers["Permissions-Policy"] = "camera=(), microphone=(), geolocation=()";
    headers.ContentSecurityPolicy =
        "default-src 'self'; img-src 'self' data:; style-src 'self'; script-src 'self'; font-src 'self'; " +
        "connect-src 'self'; object-src 'none'; base-uri 'self'; form-action 'self'; frame-ancestors 'none'";
    await next();
});

app.UseDefaultFiles();
app.UseStaticFiles();
app.UseRateLimiter();

app.MapCatalogApi();
app.MapHealthChecks("/health");

app.Run();

public partial class Program;
