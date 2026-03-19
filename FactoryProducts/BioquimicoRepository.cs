using MySql.Data.MySqlClient;
using ProyectoArqSoft.Models;
using ProyectoArqSoft.FactoryProducts;
using ProyectoArqSoft.Helpers;
using System.Data;

namespace ProyectoArqSoft.Repositories
{
    public class BioquimicoRepository : IRepository<Bioquimico>
    {
        private readonly string _connectionString;

        public BioquimicoRepository(IConfiguration configuration)
        {
            // Obtenemos la cadena de conexión centralizada
            _connectionString = configuration.GetConnectionString("MySqlConnection")!;
        }

        // Obtener todos los bioquímicos activos (usado para el listado)
        public DataTable GetAll(string filtro)
        {
            DataTable dt = new DataTable();
            using var connection = new MySqlConnection(_connectionString);
            
            // Reutilizamos tu lógica de construcción de filtros SQL
            string query = $@"SELECT idBioquimico, nombres, apellido_paterno, apellido_materno, 
                                     ci, ci_extencion, telefono 
                              FROM bioquimico 
                              WHERE activo = 1 
                              {FiltroSqlHelper.ConstruirCondicionLike(filtro, "nombres", "apellido_paterno", "apellido_materno", "ci", "telefono")}
                              ORDER BY apellido_paterno, nombres";

            using var command = new MySqlCommand(query, connection);
            FiltroSqlHelper.AgregarParametrosLike(command, filtro);
            
            new MySqlDataAdapter(command).Fill(dt);
            return dt;
        }

        // Buscar un bioquímico específico por su ID
        public Bioquimico? GetById(int id)
        {
            using var connection = new MySqlConnection(_connectionString);
            string query = "SELECT * FROM bioquimico WHERE idBioquimico = @id AND activo = 1";
            
            using var command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@id", id);
            connection.Open();

            using var reader = command.ExecuteReader();
            if (reader.Read())
            {
                return new Bioquimico
                {
                    IdBioquimico = Convert.ToInt32(reader["idBioquimico"]),
                    Nombres = reader["nombres"].ToString()!,
                    ApellidoPaterno = reader["apellido_paterno"].ToString()!,
                    ApellidoMaterno = reader["apellido_materno"].ToString()!,
                    Ci = reader["ci"].ToString()!,
                    CiExtencion = reader["ci_extencion"].ToString()!,
                    Telefono = reader["telefono"].ToString()!
                };
            }
            return null;
        }

        // Insertar un nuevo registro
        public int Insert(Bioquimico entity)
        {
            using var connection = new MySqlConnection(_connectionString);
            string query = @"INSERT INTO bioquimico 
                             (nombres, apellido_paterno, apellido_materno, ci, ci_extencion, telefono, activo) 
                             VALUES (@nom, @apP, @apM, @ci, @ext, @tel, 1)";
            
            using var command = new MySqlCommand(query, connection);
            MapearParametros(command, entity);
            
            connection.Open();
            return command.ExecuteNonQuery();
        }

        // Actualizar datos existentes
        public int Update(Bioquimico entity)
        {
            using var connection = new MySqlConnection(_connectionString);
            string query = @"UPDATE bioquimico 
                             SET nombres=@nom, apellido_paterno=@apP, apellido_materno=@apM, 
                                 ci=@ci, ci_extencion=@ext, telefono=@tel, ultima_actualizacion=NOW() 
                             WHERE idBioquimico=@id AND activo=1";
            
            using var command = new MySqlCommand(query, connection);
            MapearParametros(command, entity);
            command.Parameters.AddWithValue("@id", entity.IdBioquimico);
            
            connection.Open();
            return command.ExecuteNonQuery();
        }

        // Baja lógica (Eliminar)
        public int Delete(Bioquimico entity)
        {
            using var connection = new MySqlConnection(_connectionString);
            string query = "UPDATE bioquimico SET activo = 0 WHERE idBioquimico = @id";
            
            using var command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@id", entity.IdBioquimico);
            
            connection.Open();
            return command.ExecuteNonQuery();
        }

        // Método privado para no repetir código de parámetros
        private void MapearParametros(MySqlCommand command, Bioquimico b)
        {
            command.Parameters.AddWithValue("@nom", b.Nombres);
            command.Parameters.AddWithValue("@apP", b.ApellidoPaterno);
            command.Parameters.AddWithValue("@apM", b.ApellidoMaterno);
            command.Parameters.AddWithValue("@ci", b.Ci);
            command.Parameters.AddWithValue("@ext", b.CiExtencion);
            command.Parameters.AddWithValue("@tel", b.Telefono);
        }

        // Necesitaremos este para las validaciones de duplicados más adelante
        public DataTable GetByDocumento(string ci, string extension)
        {
            DataTable dt = new DataTable();
            using var connection = new MySqlConnection(_connectionString);
            string query = "SELECT idBioquimico FROM bioquimico WHERE ci = @ci AND ci_extencion = @ext AND activo = 1";
            
            using var command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@ci", ci);
            command.Parameters.AddWithValue("@ext", extension);
            
            new MySqlDataAdapter(command).Fill(dt);
            return dt;
        }

        // Implementación requerida por IRepository si no pasas filtro
        public DataTable GetAll() => GetAll(string.Empty);
    }
}