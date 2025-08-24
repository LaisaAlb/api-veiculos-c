using veiculosApi.Dominio.Entidades;
using veiculosApi.DTOs;

namespace veiculosApi.Dominio.Interfaces.IAdminService;

public interface IAdminService
{
    Admin? Login(LoginDTO loginDTO);
}
