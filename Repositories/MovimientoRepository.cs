using Gestor_de_gastos.DTOs;
using Gestor_de_gastos.Interface;
using Gestor_de_gastos.Models;
using Microsoft.Data.Sqlite;

namespace Gestor_de_gastos.Repository;

public class MovimientoRepositorio : IMovimientoRepository
{
    private readonly string CadenaConexion;

    public MovimientoRepositorio(string ConnectionString)
    {
        CadenaConexion = ConnectionString;
    }
    public void Create(Movimiento movimiento)
    {
        using var con = new SqliteConnection(CadenaConexion);
        con.Open();

        string sql = @"INSERT INTO Movimiento (Descripcion, Monto, Fecha, Tipo, CategoriaId, Comentario) 
                    VALUES (@desc, @monto, @fecha, @tipo, @categoria, @comentario);
                    SELECT last_insert_rowid();";

        using var cmd = new SqliteCommand(sql, con);
        cmd.Parameters.Add(new SqliteParameter("@desc",       movimiento.Descripcion));
        cmd.Parameters.Add(new SqliteParameter("@monto",      movimiento.Monto));
        cmd.Parameters.Add(new SqliteParameter("@fecha",      movimiento.Fecha.ToString("yyyy-MM-dd"))); // ✅ Convertir a string
        cmd.Parameters.Add(new SqliteParameter("@tipo",       (int)movimiento.Tipo)); // ✅ Castear enum a int
        cmd.Parameters.Add(new SqliteParameter("@categoria",  movimiento.CategoriaId));
        cmd.Parameters.Add(new SqliteParameter("@comentario", (object?)movimiento.Comentario ?? DBNull.Value)); // ✅ Manejar null

        movimiento.Id = Convert.ToInt32(cmd.ExecuteScalar());
    }
    public List<Movimiento> GetAll()
    {
        List<Movimiento> movimientos = [];
        using var con = new SqliteConnection(CadenaConexion);
        con.Open();
        string sql = @"SELECT m.Id,
                m.Descripcion,
                m.Monto,
                m.Fecha,
                m.Tipo,
                m.Comentario,
                c.Id AS CategoriaId,
                c.Nombre AS CategoriaNombre,
                c.Color AS CategoriaColor
            FROM Movimiento m
            INNER JOIN Categoria c ON m.CategoriaId = c.Id
            ORDER BY m.Fecha DESC";
            using var cmd = new SqliteCommand(sql,con);
            using var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            int idMovimientoActual = Convert.ToInt32(reader["Id"]);
            Movimiento movimiento = null;
            foreach(var mov in movimientos)
            {
                if(mov.Id == idMovimientoActual)
                {
                    movimiento = mov;
                    break;
                }
            }
            if(movimiento == null)
            {
                movimiento = new Movimiento
                {
                    Id = idMovimientoActual,
                    Descripcion = reader["Descripcion"].ToString(),
                    Monto = Convert.ToDecimal(reader["Monto"]),
                    Fecha = DateOnly.FromDateTime(reader.GetDateTime(reader.GetOrdinal("Fecha"))),
                    Tipo = (TipoMovimiento)reader.GetInt32(reader.GetOrdinal("Tipo")),
                    CategoriaId = Convert.ToInt32(reader["CategoriaId"]),
                    Comentario  =reader.IsDBNull(reader.GetOrdinal("Comentario")) ? null : reader["Comentario"].ToString(),
                    Categoria = new Categoria
                    {
                        Id = Convert.ToInt32(reader["CategoriaId"]),
                        Nombre = reader["CategoriaNombre"].ToString(),
                        Color = reader["CategoriaColor"].ToString()
                    }
                };
                movimientos.Add(movimiento);
            }
        }
        con.Close();
        return movimientos;
    }

    public Movimiento GetById(int id)
    {
        Movimiento movimiento = null;
        using var con = new SqliteConnection(CadenaConexion);
        con.Open();
        string sql = @"SELECT m.Id,
                m.Descripcion,
                m.Monto,
                m.Fecha,
                m.Tipo,
                m.Comentario,
                c.Id AS CategoriaId,
                c.Nombre AS CategoriaNombre,
                c.Color AS CategoriaColor
            FROM Movimiento m
            INNER JOIN Categoria c ON m.CategoriaId = c.Id WHERE m.Id = @id
            ORDER BY m.Fecha DESC";
            using var cmd = new SqliteCommand(sql,con);
            cmd.Parameters.Add(new SqliteParameter("@id",id));
            using var reader = cmd.ExecuteReader();
            if (!reader.HasRows) throw new KeyNotFoundException($"El movimiento {id} no existe");
        while (reader.Read())
        {

            if(movimiento == null)
            {
                movimiento = new Movimiento
                {
                    Id = Convert.ToInt32(reader["Id"]),
                    Descripcion = reader["Descripcion"].ToString(),
                    Monto = Convert.ToDecimal(reader["Monto"]),
                    Fecha = DateOnly.FromDateTime(reader.GetDateTime(reader.GetOrdinal("Fecha"))),
                    Tipo = (TipoMovimiento)reader.GetInt32(reader.GetOrdinal("Tipo")),
                    CategoriaId = Convert.ToInt32(reader["CategoriaId"]),
                    Comentario  =reader.IsDBNull(reader.GetOrdinal("Comentario")) ? null : reader["Comentario"].ToString(),
                    Categoria = new Categoria
                    {
                        Id = Convert.ToInt32(reader["CategoriaId"]),
                        Nombre = reader["CategoriaNombre"].ToString(),
                        Color = reader["CategoriaColor"].ToString()
                    }
                };
            }
        }
        con.Close();
        return movimiento;
    }

    public void Update(int id,Movimiento movimiento)
    {
        const string sql = @"
            UPDATE Movimiento
            SET
                Descripcion = @Descripcion,
                Monto       = @Monto,
                Fecha       = @Fecha,
                Tipo        = @Tipo,
                Comentario  = @Comentario,
                CategoriaId = @CategoriaId
            WHERE Id = @Id";

        using var con = new SqliteConnection(CadenaConexion);
        con.Open();

        using var cmd = new SqliteCommand(sql, con);
        cmd.Parameters.Add(new SqliteParameter("@Id",id));
        cmd.Parameters.Add(new SqliteParameter("@Descripcion", movimiento.Descripcion));
        cmd.Parameters.Add(new SqliteParameter("@Monto",       movimiento.Monto));
        cmd.Parameters.Add(new SqliteParameter("@Fecha",       movimiento.Fecha.ToString("yyyy-MM-dd")));
        cmd.Parameters.Add(new SqliteParameter("@Tipo",        (int)movimiento.Tipo));
        cmd.Parameters.Add(new SqliteParameter("@Comentario",  (object?)movimiento.Comentario ?? DBNull.Value));
        cmd.Parameters.Add(new SqliteParameter("@CategoriaId", movimiento.CategoriaId));

        int rowsAffected = cmd.ExecuteNonQuery();

        if (rowsAffected == 0) throw new KeyNotFoundException($"El movimiento {movimiento.Id} no existe");
        con.Close();
    }

    public void Delete(int id)
    {
        using var con = new SqliteConnection(CadenaConexion);
        con.Open();
        string sql = "DELETE FROM Movimiento WHERE Id = @id";
        using var cmd = new SqliteCommand(sql,con);
        cmd.Parameters.Add(new SqliteParameter("@id",id));
        int modificado = cmd.ExecuteNonQuery();
        if(modificado == 0) throw new KeyNotFoundException($"El movimiento {id} no existe");
        con.Close();
    }

    public List<GastosPorCategoriaDTO> GetGastosPorCategoria()
    {
        var resultado = new List<GastosPorCategoriaDTO>();
        string sql = @"SELECT c.Nombre, c.Color, SUM(m.Monto) AS Total FROM Movimiento m INNER JOIN Categoria c ON m.CategoriaId = c.Id
        WHERE  m.Tipo = 1 GROUP BY c.Id,c.Nombre, c.Color ORDER BY Total DESC;";
        using var con = new SqliteConnection(CadenaConexion);
        con.Open();
        using var cmd = new SqliteCommand(sql,con);
        using var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            resultado.Add( new GastosPorCategoriaDTO
            {
                Nombre = reader["Nombre"].ToString(),
                Color = reader["Color"].ToString(),
                Total = Convert.ToDecimal(reader["Total"])   
            });
        }
        return resultado;
    }

    public ResumenDTO GetResumen()
    {
         const string sql = @"
        SELECT 
            SUM(CASE WHEN Tipo = 0 THEN Monto ELSE 0 END) AS TotalIngresos,
            SUM(CASE WHEN Tipo = 1 THEN Monto ELSE 0 END) AS TotalGastos
        FROM Movimiento";

        using var con = new SqliteConnection(CadenaConexion);
        con.Open();

        using var cmd = new SqliteCommand(sql, con);
        using var reader = cmd.ExecuteReader();

        if (reader.Read())
        {
            return new ResumenDTO
            {
                TotalIngresos = reader.IsDBNull(reader.GetOrdinal("TotalIngresos")) 
                                    ? 0 
                                    : Convert.ToDecimal(reader["TotalIngresos"]),
                TotalGastos   = reader.IsDBNull(reader.GetOrdinal("TotalGastos"))   
                                    ? 0 
                                    : Convert.ToDecimal(reader["TotalGastos"])
            };
        }
        return new ResumenDTO();
    }
    public List<GastosPorCategoriaDTO> GetTopCategorias()
    {
        var resultado = new List<GastosPorCategoriaDTO>();

        const string sql = @"
            SELECT c.Nombre, c.Color, SUM(m.Monto) AS Total
            FROM Movimiento m
            INNER JOIN Categoria c ON m.CategoriaId = c.Id
            WHERE m.Tipo = 1
            GROUP BY c.Id, c.Nombre, c.Color
            ORDER BY Total DESC
            LIMIT 5";

        using var con = new SqliteConnection(CadenaConexion);
        con.Open();

        using var cmd = new SqliteCommand(sql, con);
        using var reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            resultado.Add(new GastosPorCategoriaDTO
            {
                Nombre = reader["Nombre"].ToString(),
                Color  = reader["Color"].ToString(),
                Total  = Convert.ToDecimal(reader["Total"])
            });
        }

        return resultado;
    }
}