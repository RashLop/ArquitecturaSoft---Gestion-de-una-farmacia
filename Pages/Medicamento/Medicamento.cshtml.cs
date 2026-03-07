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
                            FROM medicamento";

            using (MySqlConnection connection = new MySqlConnection(connectionString)) 
            {
                connection.Open();
                MySqlCommand command = new MySqlCommand(query, connection);
                //Se ejecuta la consulta SQL del comando y los datos se transfieren
                //al objeto adapter
                MySqlDataAdapter adapter = new MySqlDataAdapter(command);
                adapter.Fill(MedicamentoDataTable);

            }

          
               
        }


    }
}
