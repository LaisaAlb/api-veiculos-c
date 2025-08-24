using System.Net.NetworkInformation;
using Microsoft.EntityFrameworkCore;
using veiculosApi.Dominio.Entidades;

namespace veiculosApiApi.Infraestrutura.DB;

public class DbContexto : DbContext
{
    private readonly IConfiguration _configuracaoAppSettings;
    public DbContexto(IConfiguration configuracaoAppSettings)
    {
        _configuracaoAppSettings = configuracaoAppSettings;
    }

    public DbSet<Admin> Admin { get; set; } = default!;

    public DbSet<Veiculo> Veiculos { get; set; } = default!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Admin>().HasData(
            new Admin
            {
                Id = 1,
                Email = "administrador@teste.com",
                Senha = "123456",
                Perfil = "Adm"
            }
        );
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            var stringConecao = _configuracaoAppSettings.GetConnectionString("mysql");
            if (!string.IsNullOrEmpty(stringConecao))
            {
                optionsBuilder.UseMySql(stringConecao,
                ServerVersion.AutoDetect(stringConecao));
            }
        }
    }
}
