using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MySql.Data;
using MySql.Data.MySqlClient;
using System.Data;//ADO .NET

namespace ProyectoArqSoft.Pages
{
    public class MedicamentoModel : PageModel
    {
        public DataTable MedicamentoDataTable { get; set; } = new DataTable();
        private readonly IConfiguration configuration;

        public MedicamentoModel(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
        public void OnGet()
        {
            Select();

        }

        void Select()
        {
            string connectionString = configuration.GetConnectionString("MySqlConnection")!;
            string query = @"SELECT  id_medicamento, nombre, presentacion, clasificacion, concentracion, precio
                            FROM medicamento
                            WHERE estado = 1
                            ORDER BY 2";

            using (MySqlConnection connection = new MySqlConnection(connectionString)) 
            {
                connection.Open();
                MySqlCommand command = new MySqlCommand(query, connection);
                MySqlDataAdapter adapter = new MySqlDataAdapter(command);
                adapter.Fill(MedicamentoDataTable);

            }
        }

        public IActionResult OnPostSoftDelete(int id)
        {
            string connectionString = configuration.GetConnectionString("MySqlConnection")!;

            string query = @"UPDATE medicamento 
                        SET estado = 0,
                        ultima_actualizacion = NOW()
                        WHERE id_medicamento = @id";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@id", id);

                connection.Open();
                command.ExecuteNonQuery();
            }

            return RedirectToPage();    
        }


    }
}
