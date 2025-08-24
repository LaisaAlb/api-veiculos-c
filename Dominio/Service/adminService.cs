using Microsoft.EntityFrameworkCore;
using veiculosApi.Dominio.Entidades;
using veiculosApi.Dominio.Interfaces.IAdminService;
using veiculosApi.DTOs;
using veiculosApiApi.Infraestrutura.DB;

namespace veiculosApi.Dominio.Servicos;

public class AdminService : IAdminService
{
    private readonly DbContexto _contexto; 
    public AdminService(DbContexto contexto)
    {
        _contexto = contexto;
    }
    public Admin? Login(LoginDTO loginDTO)
    {
        var adm= _contexto.Admin.Where(a => a.Email == loginDTO.Email && a.Senha == loginDTO.Senha).FirstOrDefault();
        return adm;
    }
}
