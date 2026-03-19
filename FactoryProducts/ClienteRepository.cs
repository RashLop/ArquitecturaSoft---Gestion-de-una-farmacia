using ProyectoArqSoft.Models;
using MySql.Data.MySqlClient;
using System.Data;

namespace ProyectoArqSoft.FactoryProducts
{
    public class ClienteRepository : IRepository<Cliente>
    {
        private readonly string _connectionString;
        private readonly ILogger<ClienteRepository> _logger;  // ? Declarar el campo

        public ClienteRepository(IConfiguration configuration, ILogger<ClienteRepository> logger)
        {
            _connectionString = configuration.GetConnectionString("MySqlConnection")
                ?? throw new InvalidOperationException("Connection string not found");
            _logger = logger;  // ? Asignar el logger
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
                _logger.LogInformation("Abriendo conexión a MySQL...");
                connection.Open();
                _logger.LogInformation("Conexión abierta exitosamente");

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

                _logger.LogDebug("Query: {Query}", query);

                using var command = new MySqlCommand(query, connection);

                if (!string.IsNullOrWhiteSpace(filtro))
                {
                    string parametro = $"%{filtro}%";
                    command.Parameters.AddWithValue("@filtro", parametro);
                    _logger.LogDebug("Parámetro @filtro: {Parametro}", parametro);
                }

                using var adapter = new MySqlDataAdapter(command);
                _logger.LogInformation("Ejecutando consulta...");
                adapter.Fill(dataTable);
                _logger.LogInformation("Filas obtenidas: {RowCount}", dataTable.Rows.Count);

                if (dataTable.Rows.Count > 0)
                {
                    _logger.LogInformation("Primera fila - ID: {Id}, Razón Social: {Razon}, NIT: {Nit}",
                        dataTable.Rows[0]["idCliente"],
                        dataTable.Rows[0]["razon_social"],
                        dataTable.Rows[0]["nit"]);
                }

                return dataTable;
            }
            catch (MySqlException ex)
            {
                _logger.LogError(ex, "Error MySQL en GetAll. Número: {Number}", ex.Number);
                throw new Exception($"Error de MySQL: {ex.Message} (Código: {ex.Number})", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error general en GetAll");
                throw new Exception($"Error general: {ex.Message}", ex);
            }
        }

        public Cliente? GetById(int id)
        {
            _logger.LogInformation("Buscando cliente por ID: {Id}", id);

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
                    _logger.LogInformation("Cliente encontrado: {RazonSocial}", reader["razon_social"]);

                    return new Cliente
                    {
                        IdCliente = reader.GetInt32("idCliente"),
                        Nit = reader.GetString("nit"),
                        RazonSocial = reader.GetString("razon_social"),
                        CorreoElectronico = reader.IsDBNull(reader.GetOrdinal("correo_electronico")) ? null : reader.GetString("correo_electronico"),
                        FechaRegistro = reader.GetDateTime("fecha_registro")
                    };
                }

                _logger.LogWarning("Cliente con ID {Id} no encontrado", id);
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
            _logger.LogInformation("Insertando cliente: {RazonSocial}", cliente.RazonSocial);

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

                int resultado = command.ExecuteNonQuery();
                _logger.LogInformation("Cliente insertado. Filas afectadas: {Filas}", resultado);

                return resultado;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al insertar cliente {RazonSocial}", cliente.RazonSocial);
                throw;
            }
        }

        public int Update(Cliente cliente)
        {
            _logger.LogInformation("Actualizando cliente ID: {Id}", cliente.IdCliente);

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

                int resultado = command.ExecuteNonQuery();
                _logger.LogInformation("Cliente actualizado. Filas afectadas: {Filas}", resultado);

                return resultado;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar cliente ID {Id}", cliente.IdCliente);
                throw;
            }
        }

        public int Delete(Cliente cliente)
        {
            _logger.LogInformation("Eliminando cliente ID: {Id}", cliente.IdCliente);

            try
            {
                using var connection = new MySqlConnection(_connectionString);
                connection.Open();

                string query = @"DELETE FROM cliente WHERE idCliente = @id";

                using var command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@id", cliente.IdCliente);

                int resultado = command.ExecuteNonQuery();
                _logger.LogInformation("Cliente eliminado. Filas afectadas: {Filas}", resultado);

                return resultado;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar cliente ID {Id}", cliente.IdCliente);
                throw;
            }
        }
    }
}