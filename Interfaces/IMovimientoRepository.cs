using Gestor_de_gastos.Models;
using Gestor_de_gastos.DTOs;

namespace Gestor_de_gastos.Interface;

public interface IMovimientoRepository
{
    public void Create(Movimiento movimiento);
    public List<Movimiento> GetAll(DateOnly desde, DateOnly hasta);
    public Movimiento GetById(int id);
    public void Update(int id, Movimiento movimiento);
    public void Delete(int id);
    public List<GastosPorCategoriaDTO> GetGastosPorCategoria(DateOnly desde, DateOnly hasta);
    public ResumenDTO GetResumen(DateOnly desde, DateOnly hasta);
    public List<GastosPorCategoriaDTO> GetTopCategorias(DateOnly desde, DateOnly hasta);
}