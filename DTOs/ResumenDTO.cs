using Gestor_de_gastos.Models;

namespace Gestor_de_gastos.DTOs;

// DTOs/ResumenDTO.cs
public class ResumenDTO
{
    public decimal TotalIngresos { get; set; }
    public decimal TotalGastos   { get; set; }
    public decimal Saldo => TotalIngresos - TotalGastos; // Se calcula solo
    public List<Movimiento> Movimientos { get; set; } = new List<Movimiento>();
}