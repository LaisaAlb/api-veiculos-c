using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using veiculos.Dominio.ModelViews;
using veiculosApi;
using veiculosApi.Dominio.Interfaces.IAdminService;
using veiculosApi.Dominio.Servicos;
using veiculosApi.DTOs;
using veiculosApiApi.Infraestrutura.DB;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddScoped<IAdminService, AdminService>();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<DbContexto>(options =>
{
    options.UseMySql(
        builder.Configuration.GetConnectionString("mysql"),
        ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("mysql"))
        );
});

var app = builder.Build();

app.UseSwagger();

app.UseSwaggerUI();

app.MapControllers();

app.MapGet("/", () => Results.Json(new Home()));

app.MapPost("/login", ([FromBody]LoginDTO loginDTO, IAdminService adminService) =>
{
    if (adminService.Login(loginDTO) != null)
        return Results.Ok("Login com sucesso");
    else
        return Results.Unauthorized();
});

app.Run();

