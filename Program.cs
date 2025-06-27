using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Configure Kestrel to use both ports
builder.WebHost.UseUrls("http://0.0.0.0:50227", "http://0.0.0.0:59412");

// Allow CORS from any origin
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
    });
});

var app = builder.Build();

app.UseCors("AllowAll");

// Allow web pages to be embedded in iframes
app.Use(async (context, next) =>
{
    context.Response.Headers.Remove("X-Frame-Options");
    context.Response.Headers.Add("Content-Security-Policy", "frame-ancestors *");
    await next();
});

app.UseStaticFiles();

app.MapGet("/api/route", (double distance) =>
{
    // Dummy response; actual route generation logic can be implemented later.
    return Results.Json(new { success = true, message = $"Generating route for {distance} km cycle." });
});

app.Run();
