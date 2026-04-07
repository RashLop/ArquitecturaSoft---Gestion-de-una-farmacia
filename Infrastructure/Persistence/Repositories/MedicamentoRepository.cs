using MySql.Data.MySqlClient;
using ProyectoArqSoft.Infrastructure.Helpers;
using ProyectoArqSoft.Domain.Models;
using System.Data;
using ProyectoArqSoft.Application.Ports.Output;
using ProyectoArqSoft.Infrastructure.Persistence.Connection;

namespace ProyectoArqSoft.Infrastructure.Persistence.Repositories
{
    public class MedicamentoRepository : IRepository<Medicamento>
    {
        private readonly string connectionString;

        public MedicamentoRepository()
        {
            connectionString = ConexionStringSingleton.Instancia.CadenaConexion;
        }

        public int Insert(Medicamento t)
        {
            string query = @"INSERT INTO medicamento
                            (nombre, presentacion, id_clasificacion, concentracion, precio, stock)
                            VALUES
                            (@nombre, @presentacion, @id_clasificacion, @concentracion, @precio, @stock)";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                MySqlCommand command = new MySqlCommand(query, connection);

                command.Parameters.AddWithValue("@nombre", t.Nombre);
                command.Parameters.AddWithValue("@presentacion", t.Presentacion);
                command.Parameters.AddWithValue("@id_clasificacion", t.IdClasificacion);
                command.Parameters.AddWithValue("@concentracion", t.Concentracion);
                command.Parameters.AddWithValue("@precio", t.Precio);
                command.Parameters.AddWithValue("@stock", t.Stock);

                connection.Open();
                return command.ExecuteNonQuery();
            }
        }

        public int Update(Medicamento t)
        {
            string query = @"UPDATE medicamento
                             SET nombre=@nombre,
                                 presentacion=@presentacion,
                                 id_clasificacion=@id_clasificacion,
                                 concentracion=@concentracion,
                                 precio=@precio,
                                 stock=@stock,
                                 id_usuario=@id_usuario,
                                 ultima_actualizacion = NOW()
                             WHERE id_medicamento=@id_medicamento";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                MySqlCommand command = new MySqlCommand(query, connection);

                command.Parameters.AddWithValue("@id_medicamento", t.Id);
                command.Parameters.AddWithValue("@nombre", t.Nombre);
                command.Parameters.AddWithValue("@presentacion", t.Presentacion);
                command.Parameters.AddWithValue("@id_clasificacion", t.IdClasificacion);
                command.Parameters.AddWithValue("@concentracion", t.Concentracion);
                command.Parameters.AddWithValue("@precio", t.Precio);
                command.Parameters.AddWithValue("@stock", t.Stock);
                command.Parameters.AddWithValue("@id_usuario", t.IdUsuario);

                connection.Open();
                return command.ExecuteNonQuery();
            }
        }

        public int Delete(Medicamento t)
        {
            string query = @"UPDATE medicamento
                             SET estado = 0,
                                 id_usuario = @id_usuario,
                                 ultima_actualizacion = NOW()
                             WHERE id_medicamento = @id";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@id", t.Id);
                command.Parameters.AddWithValue("@id_usuario", t.IdUsuario);

                connection.Open();
                return command.ExecuteNonQuery();
            }
        }

        public DataTable GetAll()
        {
            return GetAll(string.Empty);
        }

        public DataTable GetAll(string filtro)
        {
            DataTable tabla = new DataTable();

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                string query = ConstruirQuery(filtro);
                MySqlCommand command = new MySqlCommand(query, connection);

                FiltroSqlHelper.AgregarParametrosLike(command, filtro);

                MySqlDataAdapter adapter = new MySqlDataAdapter(command);
                adapter.Fill(tabla);
            }

            return tabla;
        }

        public Medicamento? GetById(int id)
        {
            string query = @"SELECT id_medicamento, nombre, presentacion, id_clasificacion, concentracion, precio, stock
                             FROM medicamento
                             WHERE id_medicamento = @id_medicamento";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@id_medicamento", id);

                connection.Open();

                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new Medicamento
                        {
                            Id = Convert.ToInt32(reader["id_medicamento"]),
                            Nombre = StringHelper.LimpiarEspacios(reader["nombre"].ToString()),
                            Presentacion = StringHelper.LimpiarEspacios(reader["presentacion"].ToString()),
                            IdClasificacion = Convert.ToInt32(reader["id_clasificacion"]),
                            Concentracion = StringHelper.LimpiarEspacios(reader["concentracion"].ToString()),
                            Precio = Convert.ToDecimal(reader["precio"]),
                            Stock = Convert.ToInt32(reader["stock"])
                        };
                    }
                }
            }

            return null;
        }

        private string ConstruirQuery(string filtro)
        {
            string query = @"SELECT m.id_medicamento,
                                    m.nombre,
                                    m.presentacion,
                                    c.nombre AS clasificacion,
                                    m.concentracion,
                                    m.precio
                            FROM medicamento m
                            INNER JOIN clasificacion c 
                                ON m.id_clasificacion = c.id_clasificacion
                            WHERE m.estado = 1";

            query += FiltroSqlHelper.ConstruirCondicionLike(
                filtro,
                "m.nombre",
                "m.presentacion",
                "c.nombre"
            );

            query += " ORDER BY m.nombre";

            return query;
        }

        public int Count()
        {
            string query = "SELECT COUNT(*) FROM medicamento WHERE estado = 1";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                MySqlCommand command = new MySqlCommand(query, connection);
                connection.Open();

                return Convert.ToInt32(command.ExecuteScalar());
            }
        }

    }
}