using MySql.Data.MySqlClient;
using ProyectoArqSoft.Helpers;
using ProyectoArqSoft.Models;
using System.Data;

namespace ProyectoArqSoft.FactoryProducts
{
    public class MedicamentoRepository : IRepository<Medicamento>
    {
        private readonly IConfiguration configuration;

        public MedicamentoRepository(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public int Insert(Medicamento t)
        {
            string connectionString = configuration.GetConnectionString("MySqlConnection")!;
            string query = @"INSERT INTO medicamento
                            (nombre, presentacion, clasificacion, concentracion, precio, stock)
                            VALUES
                            (@nombre, @presentacion, @clasificacion, @concentracion, @precio, @stock)";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                MySqlCommand command = new MySqlCommand(query, connection);

                command.Parameters.AddWithValue("@nombre", t.Nombre);
                command.Parameters.AddWithValue("@presentacion", t.Presentacion);
                command.Parameters.AddWithValue("@clasificacion", t.Clasificacion);
                command.Parameters.AddWithValue("@concentracion", t.Concentracion);
                command.Parameters.AddWithValue("@precio", t.Precio);
                command.Parameters.AddWithValue("@stock", t.Stock);

                connection.Open();
                return command.ExecuteNonQuery();
            }
        }

        public int Update(Medicamento t)
        {
            string connectionString = configuration.GetConnectionString("MySqlConnection")!;
            string query = @"UPDATE medicamento
                             SET nombre=@nombre,
                                 presentacion=@presentacion,
                                 clasificacion=@clasificacion,
                                 concentracion=@concentracion,
                                 precio=@precio,
                                 stock=@stock,
                                 ultima_actualizacion = NOW()
                             WHERE id_medicamento=@id_medicamento";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                MySqlCommand command = new MySqlCommand(query, connection);

                command.Parameters.AddWithValue("@id_medicamento", t.Id);
                command.Parameters.AddWithValue("@nombre", t.Nombre);
                command.Parameters.AddWithValue("@presentacion", t.Presentacion);
                command.Parameters.AddWithValue("@clasificacion", t.Clasificacion);
                command.Parameters.AddWithValue("@concentracion", t.Concentracion);
                command.Parameters.AddWithValue("@precio", t.Precio);
                command.Parameters.AddWithValue("@stock", t.Stock);

                connection.Open();
                return command.ExecuteNonQuery();
            }
        }

        public int Delete(Medicamento t)
        {
            string connectionString = configuration.GetConnectionString("MySqlConnection")!;
            string query = @"UPDATE medicamento
                             SET estado = 0,
                                 ultima_actualizacion = NOW()
                             WHERE id_medicamento = @id";

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

        public Medicamento? GetById(int id)
        {
            string connectionString = configuration.GetConnectionString("MySqlConnection")!;
            string query = @"SELECT id_medicamento, nombre, presentacion, clasificacion, concentracion, precio, stock
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
                            Clasificacion = StringHelper.LimpiarEspacios(reader["clasificacion"].ToString()),
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
            string query = @"SELECT id_medicamento,
                                    nombre,
                                    presentacion,
                                    clasificacion,
                                    concentracion,
                                    precio
                             FROM medicamento
                             WHERE estado = 1";

            query += FiltroSqlHelper.ConstruirCondicionLike(
                filtro,
                "nombre",
                "presentacion",
                "clasificacion"
            );

            query += " ORDER BY nombre";

            return query;
        }

        public int Count()
        {
            string connectionString = configuration.GetConnectionString("MySqlConnection")!;
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