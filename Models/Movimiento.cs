namespace Gestor_de_gastos.Models;

public enum TipoMovimiento
{
    Ingreso,
    Gasto
}

public class Movimiento
{
    public int Id {get;set;}
    public string Descripcion {get;set;}
    public decimal Monto{get;set;}
    public DateOnly Fecha {get;set;}
    public TipoMovimiento Tipo {get;set;}
    public int CategoriaId {get;set;}
    public string Comentario {get;set;}

    public Categoria? Categoria {get;set;}

    public Movimiento(){}
}