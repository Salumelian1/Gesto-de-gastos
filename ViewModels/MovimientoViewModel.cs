using System.ComponentModel.DataAnnotations;
using Gestor_de_gastos.Models;
namespace Gestor_de_gastos.viewModel;

public class MovimientoViewModel
{
    public int Id { get; set; }

    [Required(ErrorMessage = "La descripción es obligatoria")]
    [StringLength(100, ErrorMessage = "La descripción no puede superar los 100 caracteres")]
    public string Descripcion { get; set; } = string.Empty;

    [Required(ErrorMessage = "El monto es obligatorio")]
    [Range(0.01, double.MaxValue, ErrorMessage = "El monto debe ser mayor a 0")]
    public decimal Monto { get; set; }

    [Required(ErrorMessage = "La fecha es obligatoria")]
    public DateOnly Fecha { get; set; } = DateOnly.FromDateTime(DateTime.Today);

    [Required(ErrorMessage = "El tipo es obligatorio")]
    public TipoMovimiento Tipo { get; set; }

    public string? Comentario { get; set; }

    [Required(ErrorMessage = "La categoría es obligatoria")]
    public int CategoriaId { get; set; }

    // Para poblar el dropdown de categorias en el formulario
    public List<Categoria> Categorias { get; set; } = new List<Categoria>();
}