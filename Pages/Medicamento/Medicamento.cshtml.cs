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

        public string FiltroActual {get; set;} = string.Empty; 
        public string Mensaje {get; set; } = string.Empty; 
        public string MensajeError{get;set;} = string.Empty; 
        private readonly IConfiguration configuration;

        public MedicamentoModel(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
        public void OnGet(string? filtro, string? mensaje, string? error)
        {
            FiltroActual = filtro ?? string.Empty;
            Mensaje = mensaje ?? string.Empty;
            MensajeError = error ?? string.Empty;

            if (!string.IsNullOrWhiteSpace(FiltroActual))
                {
        
                    if (FiltroActual.Length < 3 || FiltroActual.Any(c => !char.IsLetterOrDigit(c) && c != ' '))
                    {
                        MensajeError = "Criterio inválido";
                        return;
                    }
                }

            Select(FiltroActual);
        }

        void Select(string filtro)
        {

            string connectionString = configuration.GetConnectionString("MySqlConnection")!;
            bool hayFiltro = !string.IsNullOrWhiteSpace(filtro);

            string query = @"SELECT  id_medicamento, nombre, presentacion, clasificacion, concentracion, precio
                            FROM medicamento
                            WHERE estado = 1"; 
            
            if (hayFiltro)
            {
                    string[] partes = filtro.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                    for (int i = 0; i < partes.Length; i++)
                    {
                        query += $" AND (nombre LIKE @valor{i} OR presentacion LIKE @valor{i} OR clasificacion LIKE @valor{i})";
                    }
            }
            query += " ORDER BY 2";

            using (MySqlConnection connection = new MySqlConnection(connectionString)) 
            {
                connection.Open();
                MySqlCommand command = new MySqlCommand(query, connection);
                if (hayFiltro)
                {
                    string[] partes = filtro.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                    for (int i = 0; i < partes.Length; i++)
                    {
                        command.Parameters.AddWithValue($"@valor{i}", "%" + partes[i] + "%");
                    }
                }
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
