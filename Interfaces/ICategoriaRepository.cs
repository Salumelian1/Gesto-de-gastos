using Gestor_de_gastos.Models;

namespace Gestor_de_gastos.Interface;

public interface ICategoriaRepository
{
    public void Create(Categoria categoria);
    public void Update(int id, Categoria categoria);
    public List<Categoria> GetAll();
    public Categoria GetById(int id);
    public void Delete(int id);
}