using Gestor_de_gastos.Interface;
using Gestor_de_gastos.Models;
using Microsoft.Data.Sqlite;


namespace Gestor_de_gastos.Repository;

public class CategoriaRepository : ICategoriaRepository
{
    private readonly string CadenaConexion;
    public CategoriaRepository(string ConnectionString)
    {
        CadenaConexion = ConnectionString;
    }

    public void Create(Categoria categoria)
    {
        using var con = new SqliteConnection(CadenaConexion);
        con.Open();
        string sql = "INSERT INTO Categoria(Nombre, Color) VALUES(@nombre, @color); SELECT last_insert_rowid();";
        using var cmd = new SqliteCommand(sql, con);
        cmd.Parameters.AddWithValue("@nombre", categoria.Nombre);
        cmd.Parameters.AddWithValue("@color", categoria.Color);
        categoria.Id = Convert.ToInt32(cmd.ExecuteScalar());
        con.Close();
    }

    public void Update(int id, Categoria categoria)
    {
        using var con = new SqliteConnection(CadenaConexion);
        con.Open();
        string sql = "UPDATE Categoria SET Nombre = @nombre, Color = @color WHERE Id = @id";
        using var cmd = new SqliteCommand(sql,con);
        cmd.Parameters.Add(new SqliteParameter("@nombre", categoria.Nombre));
        cmd.Parameters.Add(new SqliteParameter("@color", categoria.Color));
        cmd.Parameters.Add(new SqliteParameter("@id", id));
        int modificado = cmd.ExecuteNonQuery();
        if(modificado <= 0) throw new KeyNotFoundException($"La categoria {id} no existe");
        categoria.Id = id;
        con.Close();
        if(modificado == 0)
        {
            throw new Exception($"Error al actualizar: NJo se encontró ninguna categoría con el Id {id} en la base de datos");
        }
    }

    public List<Categoria> GetAll()
    {
        List<Categoria> categorias = [];
        using var con = new SqliteConnection(CadenaConexion);
        con.Open();
        string sql = "SELECT * FROM Categoria";
        using var cmd = new SqliteCommand(sql,con);
        using SqliteDataReader reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            var categoria = new Categoria
            {
                Id = Convert.ToInt32(reader["Id"]),
                Nombre = reader["Nombre"].ToString(),
                Color = reader["Color"].ToString()
            };
            categorias.Add(categoria);
        }
        con.Close();
        if(categorias.Count == 0)
        {
            throw new Exception("La consulta resultó vacía: No hay Categorias registradas en el sistema.");
        }
        return categorias;
    }

    public Categoria GetById(int id)
    {
        using var con = new SqliteConnection(CadenaConexion);
        con.Open();
        string sql = "SELECT * FROM Categoria WHERE Id = @id";
        using var cmd = new SqliteCommand(sql, con);
        cmd.Parameters.Add(new SqliteParameter("@id", id));
        using SqliteDataReader reader = cmd.ExecuteReader();
        if (!reader.Read()) throw new KeyNotFoundException($"El producto {id} no existe");
        var categoria = new Categoria
        {
            Id = Convert.ToInt32(reader["Id"]),
                Nombre = reader["Nombre"].ToString(),
                Color = reader["Color"].ToString()
        };
        con.Close();
        if (categoria == null)
        {
            throw new Exception($"Producto inexistente con el ID: {id}"); 
        }
        return categoria;
    }

    public void Delete(int id)
    {
        using var con = new SqliteConnection(CadenaConexion);
        con.Open();
        string sql = "DELETE FROM Categoria WHERE Id = @id";
        using var cmd = new SqliteCommand(sql,con);
        cmd.Parameters.Add(new SqliteParameter("@id", id));
        int modificado = cmd.ExecuteNonQuery();
        if (modificado <= 0) throw new KeyNotFoundException($"la categiria {id} no existe");
        if (modificado <= 0) 
            throw new Exception($"Error al eliminar: La categoria {id} no existe.");
        con.Close();
    }
}