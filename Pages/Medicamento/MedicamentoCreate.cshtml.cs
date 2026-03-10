using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MySql.Data.MySqlClient;

namespace ProyectoArqSoft.Pages
{
    public class MedicamentoCreateModel : PageModel
    {
        private readonly IConfiguration configuration;

        [BindProperty]
        public string Nombre { get; set; }

        [BindProperty]
        public string Presentacion { get; set; }

        [BindProperty]
        public string Clasificacion { get; set; }

        [BindProperty]
        public string Concentracion { get; set; }

        [BindProperty]
        public decimal Precio { get; set; }

        [BindProperty]
        public short Stock { get; set; }

         public string MensajeError { get; set; } = string.Empty;

        public MedicamentoCreateModel(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            if (Precio <= 0)
            {
                MensajeError = "El precio debe ser mayor a 0";
                return Page();
            }

            if (Stock < 0)
            {
                MensajeError = "El stock no puede ser negativo";
                return Page();
            }

            string connectionString = configuration.GetConnectionString("MySqlConnection")!;
            string query = @"INSERT INTO medicamento (nombre, presentacion, clasificacion, concentracion, precio, stock)
                             VALUES(@nombre, @presentacion, @clasificacion, @concentracion, @precio, @stock)";

            using (MySqlConnection connection= new MySqlConnection(connectionString))
            { 
               MySqlCommand command= new MySqlCommand(query, connection);
               command.Parameters.AddWithValue("@nombre",Nombre);
               command.Parameters.AddWithValue("@presentacion", Presentacion);
               command.Parameters.AddWithValue("@clasificacion",Clasificacion);
               command.Parameters.AddWithValue("@concentracion", Concentracion);
               command.Parameters.AddWithValue("@precio",Precio);
               command.Parameters.AddWithValue("@stock", Stock);

                connection.Open();
               command.ExecuteNonQuery();
            
            }

            return RedirectToPage("Medicamento");
        }

    }
}
