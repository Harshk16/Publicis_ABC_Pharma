using Abc_Pharmacy.Application;
using Abc_Pharmacy.Web.Api.Middleware;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplicationService();

// Add controlelr
builder.Services.AddControllers();

builder.Services.AddControllers().AddJsonOptions(opts =>
    opts.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase);

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular",
        policy => policy.WithOrigins("http://localhost:4200")
                        .AllowAnyHeader()
                        .AllowAnyMethod());
});

var app = builder.Build();

app.UseMiddleware<ExceptionMiddleware>();

app.MapControllers();
app.UseCors("AllowAngular");
app.Run();


