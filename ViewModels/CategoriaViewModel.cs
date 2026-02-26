using System.ComponentModel.DataAnnotations;
namespace Gestor_de_gastos.viewModel;

public class CategoriaViewModel
{
    public int Id { get; set; }

    [Required(ErrorMessage = "El nombre es obligatorio")]
    [StringLength(50, ErrorMessage = "El nombre no puede superar los 50 caracteres")]
    public string Nombre { get; set; } = string.Empty;

    [Required(ErrorMessage = "El color es obligatorio")]
    public string Color { get; set; } = "#000000";

}