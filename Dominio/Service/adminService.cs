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

    public Admin? BuscarPorId(int id)
    {
        return _contexto.Admin.Where(v => v.Id == id).FirstOrDefault();
    }

    public Admin Incluir(Admin admin)
    {
        _contexto.Admin.Add(admin);
        _contexto.SaveChanges();

        return admin;
    }

    public Admin? Login(LoginDTO loginDTO)
    {
        var adm= _contexto.Admin.Where(a => a.Email == loginDTO.Email && a.Senha == loginDTO.Senha).FirstOrDefault();
        return adm;
    }

    public List<Admin> Todos(int? pagina)
    {
        var query = _contexto.Admin.AsQueryable();

        int itensPorPagina = 10;

        if (pagina != null)
            query = query.Skip(((int)pagina - 1) * itensPorPagina).Take(itensPorPagina);

        return [.. query];
    }
}
