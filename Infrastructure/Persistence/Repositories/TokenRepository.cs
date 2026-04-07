using MySql.Data.MySqlClient;
using ProyectoArqSoft.FactoryProducts;
using ProyectoArqSoft.Helpers;
using ProyectoArqSoft.Domain.Models;
using ProyectoArqSoft.Services;

namespace ProyectoArqSoft.Repositories
{
    public class UsuarioTokenRepository : IUsuarioTokenRepository
    {
        private readonly string connectionString;

        public UsuarioTokenRepository()
        {
            connectionString = ConexionStringSingleton.Instancia.CadenaConexion;
        }

        public int Insert(UsuarioToken token)
        {
            string query = @"INSERT INTO usuario_token
                            (
                                usuario_idUsuario,
                                token_hash,
                                tipo_token,
                                fecha_creacion,
                                fecha_expiracion,
                                revocado,
                                usado,
                                fecha_uso,
                                fecha_revocacion
                            )
                            VALUES
                            (
                                @usuario_idUsuario,
                                @token_hash,
                                @tipo_token,
                                @fecha_creacion,
                                @fecha_expiracion,
                                @revocado,
                                @usado,
                                @fecha_uso,
                                @fecha_revocacion
                            )";

            MySqlCommand command = new MySqlCommand(query);
            command.Parameters.AddWithValue("@usuario_idUsuario", token.UsuarioIdUsuario);
            command.Parameters.AddWithValue("@token_hash", token.TokenHash);
            command.Parameters.AddWithValue("@tipo_token", token.TipoToken);
            command.Parameters.AddWithValue("@fecha_creacion", token.FechaCreacion);
            command.Parameters.AddWithValue("@fecha_expiracion", token.FechaExpiracion);
            command.Parameters.AddWithValue("@revocado", token.Revocado);
            command.Parameters.AddWithValue("@usado", token.Usado);
            command.Parameters.AddWithValue("@fecha_uso", token.FechaUso.HasValue ? token.FechaUso.Value : DBNull.Value);
            command.Parameters.AddWithValue("@fecha_revocacion", token.FechaRevocacion.HasValue ? token.FechaRevocacion.Value : DBNull.Value);

            return RepositoryDbHelper.ExecuteNonQuery(connectionString, command);
        }

        public UsuarioToken? GetByTokenHash(string tokenHash)
        {
            string query = @"SELECT *
                             FROM usuario_token
                             WHERE token_hash = @token_hash
                             LIMIT 1";

            MySqlCommand command = new MySqlCommand(query);
            command.Parameters.AddWithValue("@token_hash", tokenHash);

            return RepositoryDbHelper.ExecuteReaderSingle(connectionString, command, MapearUsuarioToken);
        }

        public UsuarioToken? GetTokenActivo(string tokenHash, string tipoToken)
        {
            string query = @"SELECT *
                             FROM usuario_token
                             WHERE token_hash = @token_hash
                               AND tipo_token = @tipo_token
                               AND revocado = 0
                               AND usado = 0
                             LIMIT 1";

            MySqlCommand command = new MySqlCommand(query);
            command.Parameters.AddWithValue("@token_hash", tokenHash);
            command.Parameters.AddWithValue("@tipo_token", tipoToken);

            return RepositoryDbHelper.ExecuteReaderSingle(connectionString, command, MapearUsuarioToken);
        }

        public int MarcarComoUsado(int idUsuarioToken)
        {
            string query = @"UPDATE usuario_token
                             SET usado = 1,
                                 fecha_uso = NOW()
                             WHERE idusuario_token = @idusuario_token";

            MySqlCommand command = new MySqlCommand(query);
            command.Parameters.AddWithValue("@idusuario_token", idUsuarioToken);

            return RepositoryDbHelper.ExecuteNonQuery(connectionString, command);
        }

        public int RevocarTokensActivos(int idUsuario, string tipoToken)
        {
            string query = @"UPDATE usuario_token
                             SET revocado = 1,
                                 fecha_revocacion = NOW()
                             WHERE usuario_idUsuario = @usuario_idUsuario
                               AND tipo_token = @tipo_token
                               AND revocado = 0
                               AND usado = 0";

            MySqlCommand command = new MySqlCommand(query);
            command.Parameters.AddWithValue("@usuario_idUsuario", idUsuario);
            command.Parameters.AddWithValue("@tipo_token", tipoToken);

            return RepositoryDbHelper.ExecuteNonQuery(connectionString, command);
        }

        public int EliminarTokensObsoletos(int dias)
        {
            if (dias <= 0)
                return 0;

            string query = @"DELETE FROM usuario_token
                             WHERE fecha_expiracion < NOW() - INTERVAL @dias DAY
                                OR (usado = 1 AND fecha_uso IS NOT NULL AND fecha_uso < NOW() - INTERVAL @dias DAY)
                                OR (revocado = 1 AND fecha_revocacion IS NOT NULL AND fecha_revocacion < NOW() - INTERVAL @dias DAY)";

            MySqlCommand command = new MySqlCommand(query);
            command.Parameters.AddWithValue("@dias", dias);

            return RepositoryDbHelper.ExecuteNonQuery(connectionString, command);
        }

        private UsuarioToken MapearUsuarioToken(MySqlDataReader reader)
        {
            return new UsuarioToken
            {
                IdUsuarioToken = reader.GetInt32("idusuario_token"),
                UsuarioIdUsuario = reader.GetInt32("usuario_idUsuario"),
                TokenHash = reader.GetString("token_hash"),
                TipoToken = reader.GetString("tipo_token"),
                FechaCreacion = reader.GetDateTime("fecha_creacion"),
                FechaExpiracion = reader.GetDateTime("fecha_expiracion"),
                Revocado = reader.GetSByte("revocado"),
                Usado = reader.GetSByte("usado"),
                FechaUso = reader.IsDBNull(reader.GetOrdinal("fecha_uso"))
                    ? (DateTime?)null
                    : reader.GetDateTime("fecha_uso"),
                FechaRevocacion = reader.IsDBNull(reader.GetOrdinal("fecha_revocacion"))
                    ? (DateTime?)null
                    : reader.GetDateTime("fecha_revocacion")
            };
        }
    }
}