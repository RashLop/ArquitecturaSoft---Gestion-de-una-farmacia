using MySql.Data.MySqlClient;
using ProyectoArqSoft.Infrastructure.Helpers;
using ProyectoArqSoft.Domain.Models;
using System.Data;
using ProyectoArqSoft.Application.Ports.Output;
using ProyectoArqSoft.Infrastructure.Persistence.Connection;

namespace ProyectoArqSoft.Infrastructure.Persistence.Repositories
{
    public class ClienteRepository : IRepository<Cliente>
    {
        private readonly string connectionString;

        public ClienteRepository()
        {
            connectionString = ConexionStringSingleton.Instancia.CadenaConexion;
        }

        public int Insert(Cliente t)
        {
            string query = @"INSERT INTO cliente
                            (nit, razon_social, correo_electronico)
                            VALUES
                            (@nit, @razon_social, @correo_electronico)";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                MySqlCommand command = new MySqlCommand(query, connection);

                command.Parameters.AddWithValue("@nit", t.Nit);
                command.Parameters.AddWithValue("@razon_social", t.RazonSocial);
                command.Parameters.AddWithValue(
                    "@correo_electronico",
                    string.IsNullOrWhiteSpace(t.CorreoElectronico) ? DBNull.Value : t.CorreoElectronico);

                connection.Open();
                return command.ExecuteNonQuery();
            }
        }

        public int Update(Cliente t)
        {
            string query = @"UPDATE cliente
                             SET nit = @nit,
                                 razon_social = @razon_social,
                                 correo_electronico = @correo_electronico,
                                 id_usuario = @id_usuario,
                                 ultima_actualizacion = NOW()
                             WHERE idCliente = @idCliente";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                MySqlCommand command = new MySqlCommand(query, connection);

                command.Parameters.AddWithValue("@idCliente", t.IdCliente);
                command.Parameters.AddWithValue("@nit", t.Nit);
                command.Parameters.AddWithValue("@razon_social", t.RazonSocial);
                command.Parameters.AddWithValue("@id_usuario", t.IdUsuario);
                command.Parameters.AddWithValue(
                    "@correo_electronico",
                    string.IsNullOrWhiteSpace(t.CorreoElectronico) ? DBNull.Value : t.CorreoElectronico);

                connection.Open();
                return command.ExecuteNonQuery();
            }
        }

        public int Delete(Cliente t)
        {
            string query = @"DELETE FROM cliente
                             WHERE idCliente = @idCliente";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@idCliente", t.IdCliente);

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

        public Cliente? GetById(int id)
        {
            string query = @"SELECT idCliente, fecha_registro, ultima_actualizacion, nit, razon_social, correo_electronico, id_usuario
                             FROM cliente
                             WHERE idCliente = @idCliente";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@idCliente", id);

                connection.Open();

                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new Cliente
                        {
                            IdCliente = Convert.ToInt32(reader["idCliente"]),
                            Nit = StringHelper.LimpiarEspacios(reader["nit"].ToString()),
                            RazonSocial = StringHelper.LimpiarEspacios(reader["razon_social"].ToString()),
                            CorreoElectronico = StringHelper.LimpiarEspacios(reader["correo_electronico"].ToString()),
                            FechaRegistro = Convert.ToDateTime(reader["fecha_registro"]),
                            IdUsuario = reader["id_usuario"] == DBNull.Value
                                ? null
                                : Convert.ToInt32(reader["id_usuario"]),
                            UltimaActualizacion = reader["ultima_actualizacion"] == DBNull.Value
                                ? null
                                : Convert.ToDateTime(reader["ultima_actualizacion"])
                        };
                    }
                }
            }

            return null;
        }

        private static string ConstruirQuery(string filtro)
        {
            string query = @"SELECT idCliente,
                                    nit,
                                    razon_social,
                                    correo_electronico,
                                    fecha_registro,
                                    ultima_actualizacion
                             FROM cliente
                             WHERE 1 = 1";

            query += FiltroSqlHelper.ConstruirCondicionLike(
                filtro,
                "nit",
                "razon_social",
                "correo_electronico"
            );

            query += " ORDER BY razon_social";

            return query;
        }

        public int Count()
        {
            string query = "SELECT COUNT(*) FROM cliente";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                MySqlCommand command = new MySqlCommand(query, connection);
                connection.Open();

                return Convert.ToInt32(command.ExecuteScalar());
            }
        }
    }
}
