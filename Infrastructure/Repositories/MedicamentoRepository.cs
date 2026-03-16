using MySql.Data.MySqlClient;
using ProyectoArqSoft.Domain.Entities.Medicamento;
using ProyectoArqSoft.Domain.Interfaces;

namespace ProyectoArqSoft.Infrastructure.Repositories
{
    public class MedicamentoRepository : IMedicamentoRepository
    {
        private readonly IConfiguration configuration;

        public MedicamentoRepository(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public void Crear(Medicamento medicamento)
        {
            Console.WriteLine("Repository: Creando medicamento...");
            string connectionString = configuration.GetConnectionString("MySqlConnection")!;

            string query = @"INSERT INTO medicamento 
                (nombre, presentacion, clasificacion, concentracion, precio, stock)
                VALUES
                (@nombre,@presentacion,@clasificacion,@concentracion,@precio,@stock)";

            using MySqlConnection connection = new MySqlConnection(connectionString);

            MySqlCommand command = new MySqlCommand(query, connection);

            command.Parameters.AddWithValue("@nombre", medicamento.Nombre);
            command.Parameters.AddWithValue("@presentacion", medicamento.Presentacion);
            command.Parameters.AddWithValue("@clasificacion", medicamento.Clasificacion);
            command.Parameters.AddWithValue("@concentracion", medicamento.Concentracion);
            command.Parameters.AddWithValue("@precio", medicamento.Precio);
            command.Parameters.AddWithValue("@stock", medicamento.Stock);

            connection.Open();
            command.ExecuteNonQuery();
        }

        public void Actualizar(Medicamento medicamento)
        {
            string connectionString = configuration.GetConnectionString("MySqlConnection")!;

            string query = @"UPDATE medicamento
                             SET nombre=@nombre,
                                 presentacion=@presentacion,
                                 clasificacion=@clasificacion,
                                 concentracion=@concentracion,
                                 precio=@precio,
                                 stock=@stock
                             WHERE id_medicamento=@id";

            using MySqlConnection connection = new MySqlConnection(connectionString);

            MySqlCommand command = new MySqlCommand(query, connection);

            command.Parameters.AddWithValue("@id", medicamento.Id);
            command.Parameters.AddWithValue("@nombre", medicamento.Nombre);
            command.Parameters.AddWithValue("@presentacion", medicamento.Presentacion);
            command.Parameters.AddWithValue("@clasificacion", medicamento.Clasificacion);
            command.Parameters.AddWithValue("@concentracion", medicamento.Concentracion);
            command.Parameters.AddWithValue("@precio", medicamento.Precio);
            command.Parameters.AddWithValue("@stock", medicamento.Stock);

            connection.Open();
            command.ExecuteNonQuery();
        }

        public void EliminarLogico(int id)
        {
            string connectionString = configuration.GetConnectionString("MySqlConnection")!;

            string query = @"UPDATE medicamento
                             SET estado = 0
                             WHERE id_medicamento=@id";

            using MySqlConnection connection = new MySqlConnection(connectionString);

            MySqlCommand command = new MySqlCommand(query, connection);

            command.Parameters.AddWithValue("@id", id);

            connection.Open();
            command.ExecuteNonQuery();
        }

        public Medicamento ObtenerPorId(int id)
        {
            throw new NotImplementedException();
        }
    }
}
