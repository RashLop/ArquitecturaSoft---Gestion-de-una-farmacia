using MySql.Data.MySqlClient;
//using ProyectoArqSoft.Application.Ports.Output;
using ProyectoArqSoft.Services;
//using ProyectoArqSoft.Domain.Model.Clasificacion;
using ProyectoArqSoft.Models;
using ProyectoArqSoft.Helpers;
using System.Data;

//namespace ProyectoArqSoft.Infrastructure.Persistence.Repositories
namespace ProyectoArqSoft.FactoryProducts
{
    public class ClasificacionRepository : IClasificacionRepository
    {
        private readonly string connectionString;

        public ClasificacionRepository()
        {
            connectionString = ConexionStringSingleton.Instancia.CadenaConexion;
        }

        public int Insert(Clasificacion t)
        {
            string query = @"INSERT INTO clasificacion
                             (nombre, origen)
                             VALUES
                             (@nombre, @origen)";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@nombre", t.Nombre);
                command.Parameters.AddWithValue("@origen", t.Origen);

                connection.Open();
                return command.ExecuteNonQuery();
            }
        }

        public int Update(Clasificacion t)
        {
            string query = @"UPDATE clasificacion
                             SET nombre = @nombre,
                                 origen = @origen,
                                 ultima_actualizacion = NOW()
                             WHERE id_clasificacion = @id_clasificacion";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@id_clasificacion", t.Id);
                command.Parameters.AddWithValue("@nombre", t.Nombre);
                command.Parameters.AddWithValue("@origen", t.Origen);

                connection.Open();
                return command.ExecuteNonQuery();
            }
        }

        public int Delete(Clasificacion t)
        {
            string query = @"UPDATE clasificacion
                             SET estado = 0,
                                 ultima_actualizacion = NOW()
                             WHERE id_clasificacion = @id";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@id", t.Id);

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

        public Clasificacion? GetById(int id)
        {
            string query = @"SELECT id_clasificacion,
                                    nombre,
                                    origen,
                                    estado,
                                    fecha_registro,
                                    ultima_actualizacion
                             FROM clasificacion
                             WHERE id_clasificacion = @id_clasificacion";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@id_clasificacion", id);

                connection.Open();

                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new Clasificacion
                        {
                            Id = Convert.ToInt32(reader["id_clasificacion"]),
                            Nombre = StringHelper.LimpiarEspacios(reader["nombre"].ToString()),
                            Origen = StringHelper.LimpiarEspacios(reader["origen"].ToString()),
                            Estado = Convert.ToInt16(reader["estado"]),
                            FechaRegistro = Convert.ToDateTime(reader["fecha_registro"]),
                            UltimaActualizacion = reader["ultima_actualizacion"] == DBNull.Value
                                ? null
                                : Convert.ToDateTime(reader["ultima_actualizacion"])
                        };
                    }
                }
            }

            return null;
        }

        public bool TieneMedicamentosActivosAsociados(int idClasificacion)
        {
            string query = @"SELECT COUNT(*)
                             FROM medicamento
                             WHERE id_clasificacion = @id_clasificacion
                               AND estado = 1";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@id_clasificacion", idClasificacion);

                connection.Open();
                int cantidad = Convert.ToInt32(command.ExecuteScalar());

                return cantidad > 0;
            }
        }

        private string ConstruirQuery(string filtro)
        {
            string query = @"SELECT id_clasificacion,
                                    nombre,
                                    origen
                             FROM clasificacion
                             WHERE estado = 1";

            query += FiltroSqlHelper.ConstruirCondicionLike(
                filtro,
                "nombre",
                "origen"
            );

            query += " ORDER BY nombre";

            return query;
        }

        public int Count()
        {
            string query = "SELECT COUNT(*) FROM clasificacion WHERE estado = 1";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                MySqlCommand command = new MySqlCommand(query, connection);
                connection.Open();

                return Convert.ToInt32(command.ExecuteScalar());
            }
        }

        public bool ExisteNombreActivo(string nombre)
        {
            string query = @"SELECT COUNT(*)
                            FROM clasificacion
                            WHERE nombre = @nombre
                            AND estado = 1";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@nombre", nombre);

                connection.Open();
                int cantidad = Convert.ToInt32(command.ExecuteScalar());

                return cantidad > 0;
            }
        }

        public bool ExisteNombreActivoExcluyendoId(int idClasificacion, string nombre)
        {
            string query = @"SELECT COUNT(*)
                            FROM clasificacion
                            WHERE nombre = @nombre
                            AND estado = 1
                            AND id_clasificacion <> @id_clasificacion";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@nombre", nombre);
                command.Parameters.AddWithValue("@id_clasificacion", idClasificacion);

                connection.Open();
                int cantidad = Convert.ToInt32(command.ExecuteScalar());

                return cantidad > 0;
            }
        }
    }
}