using Api.Middlewares;
using Application.Extensions;
using Application.Security;
using Infrastructure.Extensions;
using Infrastructure.Seeders;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddControllers();

builder.Services.AddInfrastructureLayer(builder.Configuration);
builder.Services.AddApplicationServices();

builder.Services.AddTransient<GlobalErrorHandlingMiddleware>();

builder.Services.AddAuth(builder.Configuration);
builder.Services.AddAuthorization();

var app = builder.Build();

await app.Services.SeedRolesAsync();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseMiddleware<GlobalErrorHandlingMiddleware>();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();