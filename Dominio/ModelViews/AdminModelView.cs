using veiculos.Dominio.Enuns;

namespace veiculos;

public record AdminModelView
{
    public int Id { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string Perfil { get; set; } = default!;
}
