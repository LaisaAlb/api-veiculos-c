using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using veiculos;
using veiculos.Dominio.DTOs.VeiculoDTOs;
using veiculos.Dominio.Enuns;
using veiculos.Dominio.ModelViews;
using veiculosApi;
using veiculosApi.Dominio.Entidades;
using veiculosApi.Dominio.Interfaces.IAdminService;
using veiculosApi.Dominio.Servicos;
using veiculosApi.DTOs;
using veiculosApiApi.Infraestrutura.DB;

#region Builder
var builder = WebApplication.CreateBuilder(args);

var key = builder.Configuration["Jwt:Key"];
if (string.IsNullOrEmpty(key))
    throw new Exception("Chave JWT não definida em appsettings.json");


builder.Services.AddAuthentication(option =>
{
    option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(option =>
{
    option.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateLifetime = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key))
    };
});

builder.Services.AddAuthorization();


builder.Services.AddControllers();

builder.Services.AddScoped<IAdminService, AdminService>();
builder.Services.AddScoped<IVeiculoService, VeiculoService>();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<DbContexto>(options =>
{
    options.UseMySql(
        builder.Configuration.GetConnectionString("MySql"),
        ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("MySql"))
        );
});

var app = builder.Build();

app.MapControllers();
#endregion

#region Home
app.MapGet("/", () => Results.Json(new Home())).WithTags("Home");
#endregion

#region Admin
string GerarTokenJwt(Admin admin)
{
    if (string.IsNullOrEmpty(key)) return string.Empty;

    var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
    var crendentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

    var claims = new List<Claim>()
    {
        new Claim("Email", admin.Email),
        new Claim("Perfil", admin.Perfil),
    };

    var token = new JwtSecurityToken(
        claims: claims, 
        expires: DateTime.Now.AddDays(1),
        signingCredentials: crendentials
    );
    return new JwtSecurityTokenHandler().WriteToken(token);
}

app.MapPost("/admin/login", ([FromBody] LoginDTO loginDTO, IAdminService adminService) =>
{
    var admin = adminService.Login(loginDTO);
    if (admin != null)
    {
        string token = GerarTokenJwt(admin);
        return Results.Ok(new AdmLogado
        {
            Email = admin.Email,
            Perfil = admin.Perfil,
            Token = token
        });
    }
    else
        return Results.Unauthorized();
}).WithTags("Administradores");

app.MapGet("/admin", ([FromQuery] int? pagina, IAdminService adminService) =>
{
    var adms = new List<AdminModelView>();
    var admin = adminService.Todos(pagina);
    foreach (var adm in admin)
    {
        adms.Add(new AdminModelView
        {
            Id = adm.Id,
            Email = adm.Email,
            Perfil = adm.Perfil
        });
    }
    return Results.Ok(adms);
}).RequireAuthorization().WithTags("Administradores");

app.MapGet("/admin/{id}", ([FromRoute] int id, IAdminService adminService) =>
{
    var admin = adminService.BuscarPorId(id);

    if (admin == null) return Results.NotFound();

    return Results.Ok(new AdminModelView
    {
        Id = admin.Id,
        Email = admin.Email,
        Perfil = admin.Perfil
    });
}).RequireAuthorization().WithTags("Administradores");

app.MapPost("/admin", ([FromBody] AdminDTO adminDTO, IAdminService adminService) =>
{
    var validacao = new ErrosDeValidacao
    {
        Mensagens = new List<string>()
    };

    if (string.IsNullOrEmpty(adminDTO.Email))
        validacao.Mensagens.Add("Email não pode ser vazio");

    if (string.IsNullOrEmpty(adminDTO.Senha))
        validacao.Mensagens.Add("Senha não pode ser vazia");

    if (adminDTO.Perfil == null)
        validacao.Mensagens.Add("Perfil não pode ser vazio");

    if (validacao.Mensagens.Count > 0)
        return Results.BadRequest(validacao);

    var admin = new Admin
    {
        Email = adminDTO.Email,
        Senha = adminDTO.Senha,
        Perfil = adminDTO.Perfil.ToString() ?? Perfil.Editor.ToString()
    };
    adminService.Incluir(admin);

    return Results.Created($"/admin/{admin.Id}", new AdminModelView
    {
        Id = admin.Id,
        Email = admin.Email,
        Perfil = admin.Perfil
    });

}).RequireAuthorization().WithTags("Administradores");
#endregion

#region Veiculos

ErrosDeValidacao validaDTO(VeiculoDTO veiculoDTO)
{
    var validacao = new ErrosDeValidacao
    {
        Mensagens = new List<string>()
    };

    if (string.IsNullOrEmpty(veiculoDTO.Nome))
        validacao.Mensagens.Add("O nome não pode ser vazio");

    if (string.IsNullOrEmpty(veiculoDTO.Marca))
        validacao.Mensagens.Add("A marca não pode ficar em branco");

    if (veiculoDTO.Ano < 1900)
        validacao.Mensagens.Add("Veículo muito antigo. Aceito somene anos superiores");

    return validacao;
}

app.MapPost("/veiculos", ([FromBody] VeiculoDTO veiculoDTO, IVeiculoService veiculoService) =>
{
    var validacao = validaDTO(veiculoDTO);
    if (validacao.Mensagens.Count > 0)
        return Results.BadRequest(validacao);

    var veiculo = new Veiculo
    {
        Nome = veiculoDTO.Nome,
        Marca = veiculoDTO.Marca,
        Ano = veiculoDTO.Ano,
    };

    veiculoService.Incluir(veiculo);

    return Results.Created($"/veiculo/{veiculo.Id}", veiculo);
}).RequireAuthorization().WithTags("Veículos");

app.MapGet("/veiculos", ([FromQuery] int? pagina, IVeiculoService veiculoService) =>
{
    var veiculos = veiculoService.Todos(pagina);

    return Results.Ok(veiculos);
}).RequireAuthorization().WithTags("Veículos");

app.MapGet("/veiculos/{id}", ([FromRoute] int id, IVeiculoService veiculoService) =>
{
    var veiculo = veiculoService.BuscarPorId(id);

    if (veiculo == null) return Results.NotFound();

    return Results.Ok(veiculo);
}).RequireAuthorization().WithTags("Veículos");

app.MapPut("/veiculos/{id}", ([FromRoute] int id, VeiculoDTO veiculoDTO, IVeiculoService veiculoService) =>
{
    var validacao = validaDTO(veiculoDTO);
    if (validacao.Mensagens.Count > 0)
        return Results.BadRequest(validacao);

    var veiculo = veiculoService.BuscarPorId(id);
    if (veiculo == null) return Results.NotFound();

    veiculo.Nome = veiculoDTO.Nome;
    veiculo.Marca = veiculoDTO.Marca;
    veiculo.Ano = veiculoDTO.Ano;

    veiculoService.Atualizar(veiculo);

    return Results.Ok(veiculo);
}).RequireAuthorization().WithTags("Veículos");

app.MapDelete("/veiculos/{id}", ([FromRoute] int id, IVeiculoService veiculoService) =>
{
    var veiculo = veiculoService.BuscarPorId(id);
    if (veiculo == null) return Results.NotFound();

    veiculoService.Apagar(veiculo);

    return Results.NoContent();
}).RequireAuthorization().WithTags("Veículos");
#endregion

#region App()
app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthentication();
app.UseAuthorization();

app.Run();
#endregion

