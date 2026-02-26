using Gestor_de_gastos.Models;
using Gestor_de_gastos.DTOs;

namespace Gestor_de_gastos.Interface;

public interface IMovimientoRepository
{
    public void Create(Movimiento movimiento);
    public List<Movimiento> GetAll();
    public Movimiento GetById(int id);
    public void Update(int id, Movimiento movimiento);
    public void Delete(int id);
    public List<GastosPorCategoriaDTO> GetGastosPorCategoria();
    public ResumenDTO GetResumen();
    public List<GastosPorCategoriaDTO> GetTopCategorias();
}