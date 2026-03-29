using System.Data;
using MySql.Data.MySqlClient;
using ProyectoArqSoft.Helpers;
using ProyectoArqSoft.Models;
using ProyectoArqSoft.Services;

namespace ProyectoArqSoft.FactoryProducts
{
    public class UsuarioRepository : IRepository<Usuario>
    {
        private readonly string connectionString;

        public UsuarioRepository()
        {
            connectionString = ConexionStringSingleton.Instancia.CadenaConexion;
        }

        public int Insert(Usuario t)
        {
            string query = @"INSERT INTO usuario
                            (email, user_name, password_hash, role, must_change_password, is_active, bioquimico_id_bioquimico)
                            VALUES
                            (@email, @user_name, @password_hash, @role, @must_change_password, @is_active, @bioquimico_id_bioquimico)";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                MySqlCommand command = new MySqlCommand(query, connection);

                command.Parameters.AddWithValue("@email", t.Email);
                command.Parameters.AddWithValue("@user_name", t.UserName);
                command.Parameters.AddWithValue("@password_hash", t.PasswordHash);
                command.Parameters.AddWithValue("@role", t.Role);
                command.Parameters.AddWithValue("@must_change_password", t.MustChangePassword);
                command.Parameters.AddWithValue("@is_active", t.IsActive);
                command.Parameters.AddWithValue("@bioquimico_id_bioquimico", t.BioquimicoIdBioquimico);

                connection.Open();
                return command.ExecuteNonQuery();
            }
        }

        public int Update(Usuario t)
        {
                string query = @"UPDATE usuario
                     SET email = @email,
                         user_name = @user_name,
                         role = @role,
                         ultima_actualizacion = NOW()
                     WHERE id_usuario = @id_usuario";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                MySqlCommand command = new MySqlCommand(query, connection);

                command.Parameters.AddWithValue("@id_usuario", t.IdUsuario);
                command.Parameters.AddWithValue("@email", t.Email);
                command.Parameters.AddWithValue("@user_name", t.UserName);
                command.Parameters.AddWithValue("@role", t.Role);

                connection.Open();
                return command.ExecuteNonQuery();
            }
        }

        public int Delete(Usuario t)
        {
            string query = @"UPDATE usuario
                     SET is_active = 0,
                         ultima_actualizacion = NOW()
                     WHERE id_usuario = @id_usuario";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                MySqlCommand command = new MySqlCommand(query, connection);
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

        public Usuario? GetById(int id)
        {
            string query = @"SELECT id_usuario,
                                    email,
                                    user_name,
                                    password_hash,
                                    role,
                                    must_change_password,
                                    is_active,
                                    fecha_registro,
                                    ultima_actualizacion,
                                    bioquimico_id_bioquimico
                             FROM usuario
                             WHERE id_usuario = @id_usuario";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@id_usuario", id);

                connection.Open();

                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new Usuario
                        {
                            IdUsuario = reader.GetInt32("id_usuario"),
                            Email = reader.GetString("email"),
                            UserName = reader.GetString("user_name"),
                            PasswordHash = reader.GetString("password_hash"),
                            Role = reader.GetString("role"),
                            MustChangePassword = reader.GetSByte("must_change_password"),
                            IsActive = reader.GetSByte("is_active"),
                            FechaRegistro = reader.GetDateTime("fecha_registro"),
                            UltimaActualizacion = reader.IsDBNull(reader.GetOrdinal("ultima_actualizacion"))
                                ? (DateTime?)null
                                : reader.GetDateTime("ultima_actualizacion"),
                            BioquimicoIdBioquimico = reader.GetInt32("bioquimico_id_bioquimico")
                        };
                    }
                }
            }

            return null;
        }

        private string ConstruirQuery(string filtro)
        {
            string query = @"SELECT id_usuario,
                                    email,
                                    user_name,
                                    role,
                             FROM usuario
                             WHERE is_active = 1";

            query += FiltroSqlHelper.ConstruirCondicionLike(
                filtro,
                "email",
                "user_name",
                "role"
            );

            query += " ORDER BY user_name";

            return query;
        }
    }
}