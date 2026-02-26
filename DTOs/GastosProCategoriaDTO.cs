namespace Gestor_de_gastos.DTOs;

public class GastosPorCategoriaDTO
{
    public string Nombre { get; set; } = string.Empty;
    public string Color  { get; set; } = string.Empty;
    public decimal Total { get; set; }
}