using MySql.Data.MySqlClient;
using ProyectoArqSoft.Helpers;
using ProyectoArqSoft.Models;
using System.Data;

namespace ProyectoArqSoft.FactoryProducts
{
    public class ClienteRepository : IRepository<Cliente>
    {
        private readonly IConfiguration configuration;

        public ClienteRepository(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public int Insert(Cliente t)
        {
            string connectionString = configuration.GetConnectionString("MySqlConnection")!;
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
            string connectionString = configuration.GetConnectionString("MySqlConnection")!;
            string query = @"UPDATE cliente
                             SET nit = @nit,
                                 razon_social = @razon_social,
                                 correo_electronico = @correo_electronico,
                                 ultima_actualizacion = NOW()
                             WHERE idCliente = @idCliente";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                MySqlCommand command = new MySqlCommand(query, connection);

                command.Parameters.AddWithValue("@idCliente", t.IdCliente);
                command.Parameters.AddWithValue("@nit", t.Nit);
                command.Parameters.AddWithValue("@razon_social", t.RazonSocial);
                command.Parameters.AddWithValue(
                    "@correo_electronico",
                    string.IsNullOrWhiteSpace(t.CorreoElectronico) ? DBNull.Value : t.CorreoElectronico);

                connection.Open();
                return command.ExecuteNonQuery();
            }
        }

        public int Delete(Cliente t)
        {
            string connectionString = configuration.GetConnectionString("MySqlConnection")!;
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
            string connectionString = configuration.GetConnectionString("MySqlConnection")!;

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
            string connectionString = configuration.GetConnectionString("MySqlConnection")!;
            string query = @"SELECT idCliente, fecha_registro, ultima_actualizacion, nit, razon_social, correo_electronico
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
    }
}
