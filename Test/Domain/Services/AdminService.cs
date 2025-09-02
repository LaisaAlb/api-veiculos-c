using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using veiculosApi.Dominio.Entidades;
using veiculosApi.Dominio.Servicos;
using veiculosApiApi.Infraestrutura.DB;

namespace Test.Domain.Entidades;

[TestClass]
public class AdministradorServicoTest
{
    private DbContexto CriarContextoDeTeste()
    {
        var assemblyPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        var path = Path.GetFullPath(Path.Combine(assemblyPath ?? "", "..", "..", ".."));

        var builder = new ConfigurationBuilder()
            .SetBasePath(path ?? Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddEnvironmentVariables();

        var configuration = builder.Build();

        return new DbContexto(configuration);
    }


    [TestMethod]
    public void TestandoSalvarAdmin()
    {
        var context = CriarContextoDeTeste();

        // Limpar a tabela e resetar o auto_increment
        context.Database.ExecuteSqlRaw("DELETE FROM Admin");
        context.Database.ExecuteSqlRaw("ALTER TABLE Admin AUTO_INCREMENT = 1");

        var adm = new Admin
        {
            Email = "teste@teste.com",
            Senha = "teste",
            Perfil = "Adm"
        };

        var adminService = new AdminService(context);

        adminService.Incluir(adm);

        var total = adminService.Todos(1).Count();
        Assert.AreEqual(1, total); // ou Assert.IsTrue(total >= 1)
    }


    [TestMethod]
    public void TestandoBuscaPorId()
    {
        // Arrange
        var context = CriarContextoDeTeste();
        context.Database.ExecuteSqlRaw("TRUNCATE TABLE Admin");

        var adm = new Admin();
        adm.Email = "teste@teste.com";
        adm.Senha = "teste";
        adm.Perfil = "Adm";

        var adminService = new AdminService(context);

        // Act
        adminService.Incluir(adm);
        var admDoBanco = adminService.BuscarPorId(adm.Id);

        // Assert
        Assert.AreEqual(2, admDoBanco?.Id);
    }
}