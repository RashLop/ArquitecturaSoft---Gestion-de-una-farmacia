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

        public MedicamentoCreateModel(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            
            string connectionString = configuration.GetConnectionString("MySqlConnection")!;
            string query = @"INSERT INTO medicamento (nombre, presentacion, clasificacion, concentracion, precio, stock)
                             VALUES(@nombre, @presentacion, @clasificacion, @concentracion, @precio, @stock)";
            //Problemas
            //1. Complejidad en la Codificación -> Soporte
            //2. Manejo de Caracteres Especiales
            //3. Inyección SQL
            //Uso de Parámetros

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
