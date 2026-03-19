using ProyectoArqSoft.Models;
using MySql.Data.MySqlClient;
using System.Data;

namespace ProyectoArqSoft.FactoryProducts
{
    public class ClienteRepository : IRepository<Cliente>
    {
        private readonly string _connectionString;
        private readonly ILogger<ClienteRepository> _logger;

        public ClienteRepository(IConfiguration configuration, ILogger<ClienteRepository> logger)
        {
            _connectionString = configuration.GetConnectionString("MySqlConnection")
                ?? throw new InvalidOperationException("Connection string not found");
            _logger = logger;
        }

        public DataTable GetAll()
        {
            return GetAll(string.Empty);
        }

        public DataTable GetAll(string filtro)
        {
            _logger.LogInformation("=== ClienteRepository.GetAll ===");
            _logger.LogInformation("Filtro recibido: '{Filtro}'", filtro);

            var dataTable = new DataTable();

            try
            {
                using var connection = new MySqlConnection(_connectionString);
                connection.Open();

                string query = @"SELECT idCliente,
                                        nit,
                                        razon_social,
                                        correo_electronico,
                                        fecha_registro
                                 FROM cliente";

                if (!string.IsNullOrWhiteSpace(filtro))
                {
                    query += @" WHERE (nit LIKE @filtro 
                                OR razon_social LIKE @filtro 
                                OR correo_electronico LIKE @filtro)";
                }

                query += " ORDER BY razon_social";

                using var command = new MySqlCommand(query, connection);

                if (!string.IsNullOrWhiteSpace(filtro))
                {
                    string parametro = $"%{filtro}%";
                    command.Parameters.AddWithValue("@filtro", parametro);
                }

                using var adapter = new MySqlDataAdapter(command);
                adapter.Fill(dataTable);
                _logger.LogInformation("Filas obtenidas: {RowCount}", dataTable.Rows.Count);

                return dataTable;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en GetAll");
                throw;
            }
        }

        public Cliente? GetById(int id)
        {
            try
            {
                using var connection = new MySqlConnection(_connectionString);
                connection.Open();

                string query = @"SELECT idCliente,
                                        nit,
                                        razon_social,
                                        correo_electronico,
                                        fecha_registro
                                 FROM cliente
                                 WHERE idCliente = @id";

                using var command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@id", id);

                using var reader = command.ExecuteReader();

                if (reader.Read())
                {
                    return new Cliente
                    {
                        IdCliente = reader.GetInt32("idCliente"),
                        Nit = reader.GetString("nit"),
                        RazonSocial = reader.GetString("razon_social"),
                        CorreoElectronico = reader.IsDBNull(reader.GetOrdinal("correo_electronico")) ? null : reader.GetString("correo_electronico"),
                        FechaRegistro = reader.GetDateTime("fecha_registro")
                    };
                }

                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener cliente por ID {Id}", id);
                throw;
            }
        }

        public int Insert(Cliente cliente)
        {
            try
            {
                using var connection = new MySqlConnection(_connectionString);
                connection.Open();

                string query = @"INSERT INTO cliente 
                                (nit, razon_social, correo_electronico, fecha_registro) 
                                VALUES 
                                (@nit, @razon_social, @correo_electronico, @fecha_registro)";

                using var command = new MySqlCommand(query, connection);

                command.Parameters.AddWithValue("@nit", cliente.Nit);
                command.Parameters.AddWithValue("@razon_social", cliente.RazonSocial);
                command.Parameters.AddWithValue("@correo_electronico", string.IsNullOrWhiteSpace(cliente.CorreoElectronico) ? DBNull.Value : cliente.CorreoElectronico);
                command.Parameters.AddWithValue("@fecha_registro", cliente.FechaRegistro);

                return command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al insertar cliente");
                throw;
            }
        }

        public int Update(Cliente cliente)
        {
            try
            {
                using var connection = new MySqlConnection(_connectionString);
                connection.Open();

                string query = @"UPDATE cliente 
                                SET nit = @nit,
                                    razon_social = @razon_social,
                                    correo_electronico = @correo_electronico,
                                    fecha_registro = @fecha_registro
                                WHERE idCliente = @id";

                using var command = new MySqlCommand(query, connection);

                command.Parameters.AddWithValue("@id", cliente.IdCliente);
                command.Parameters.AddWithValue("@nit", cliente.Nit);
                command.Parameters.AddWithValue("@razon_social", cliente.RazonSocial);
                command.Parameters.AddWithValue("@correo_electronico", string.IsNullOrWhiteSpace(cliente.CorreoElectronico) ? DBNull.Value : cliente.CorreoElectronico);
                command.Parameters.AddWithValue("@fecha_registro", cliente.FechaRegistro);

                return command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar cliente ID {Id}", cliente.IdCliente);
                throw;
            }
        }

        public int Delete(Cliente cliente)
        {
            try
            {
                using var connection = new MySqlConnection(_connectionString);
                connection.Open();

                string query = @"DELETE FROM cliente WHERE idCliente = @id";

                using var command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@id", cliente.IdCliente);

                return command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar cliente ID {Id}", cliente.IdCliente);
                throw;
            }
        }
    }
}