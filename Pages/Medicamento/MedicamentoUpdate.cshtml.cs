using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MySql.Data.MySqlClient;

namespace ProyectoArqSoft.Pages
{
    public class MedicamentoUpdateModel : PageModel
    {
        private readonly IConfiguration configuration;

        [BindProperty]
        public int IdMedicamento { get; set; }

        [BindProperty]
        public string Nombre { get; set; } = string.Empty;

        [BindProperty]
        public string Presentacion { get; set; } = string.Empty;

        [BindProperty]
        public string Clasificacion { get; set; } = string.Empty;

        [BindProperty]
        public string Concentracion { get; set; } = string.Empty;

        [BindProperty]
        public decimal Precio { get; set; }

        [BindProperty]
        public short Stock { get; set; }

        public string MensajeError { get; set; } = string.Empty;


        public MedicamentoUpdateModel(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public IActionResult OnGet(int id)
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
                        IdMedicamento = Convert.ToInt32(reader["id_medicamento"]);
                        Nombre = reader["nombre"].ToString()!;
                        Presentacion = reader["presentacion"].ToString()!;
                        Clasificacion = reader["clasificacion"].ToString()!;
                        Concentracion = reader["concentracion"].ToString()!;
                        Precio = Convert.ToDecimal(reader["precio"]);
                        Stock = Convert.ToInt16(reader["stock"]);
                    }
                    else
                    {
                        return RedirectToPage("Medicamento");
                    }
                }
            }

            return Page();
        }

        public IActionResult OnPost()
        {
            if (Precio <= 0)
            {
                MensajeError = "El precio no puede ser menor o igual a 0";
                return Page();
            }
            if (Stock < 0)
            {
                MensajeError = "El stock no puede ser negativo";
                return Page();
            }   
            if (!ModelState.IsValid)
            {
                return Page();
            }
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

                command.Parameters.AddWithValue("@id_medicamento", IdMedicamento);
                command.Parameters.AddWithValue("@nombre", Nombre);
                command.Parameters.AddWithValue("@presentacion", Presentacion);
                command.Parameters.AddWithValue("@clasificacion", Clasificacion);
                command.Parameters.AddWithValue("@concentracion", Concentracion);
                command.Parameters.AddWithValue("@precio", Precio);
                command.Parameters.AddWithValue("@stock", Stock);

                connection.Open();
                command.ExecuteNonQuery();
            }

            return RedirectToPage("Medicamento");
        }
    }
}
