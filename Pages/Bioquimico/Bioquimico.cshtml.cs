using Microsoft.AspNetCore.Mvc.RazorPages;
using MySql.Data.MySqlClient;
using System.Data;

namespace ProyectoArqSoft.Pages
{
    public class BioquimicoModel : PageModel
    {
        public DataTable BioquimicoDataTable { get; set; } = new DataTable();
        public string Mensaje { get; set; } = string.Empty;
        public string MensajeError { get; set; } = string.Empty;

        private readonly IConfiguration configuration;

        public BioquimicoModel(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public void OnGet(string? mensaje, string? error)
        {
            Mensaje = mensaje ?? string.Empty;
            MensajeError = error ?? string.Empty;

            Select();
        }

        void Select()
        {
            string connectionString = configuration.GetConnectionString("MySqlConnection")!;
            string query = @"SELECT idBioquimico, nombres, apellidos, ci, telefono, activo
                             FROM bioquimico
                             ORDER BY idBioquimico ASC";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                MySqlCommand command = new MySqlCommand(query, connection);
                MySqlDataAdapter adapter = new MySqlDataAdapter(command);
                adapter.Fill(BioquimicoDataTable);
            }
        }
    }
}