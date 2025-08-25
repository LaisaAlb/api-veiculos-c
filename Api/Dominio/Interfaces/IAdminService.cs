using veiculosApi.Dominio.Entidades;
using veiculosApi.DTOs;

namespace veiculosApi.Dominio.Interfaces.IAdminService;

public interface IAdminService
{
    Admin? Login(LoginDTO loginDTO);
    Admin Incluir(Admin admin);
    Admin? BuscarPorId(int id);
    List<Admin> Todos(int? pagina); 


}
