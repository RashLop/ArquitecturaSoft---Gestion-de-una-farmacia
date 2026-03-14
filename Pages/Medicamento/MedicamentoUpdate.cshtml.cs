using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ProyectoArqSoft.Models;
using ProyectoArqSoft.Validaciones;
using MySql.Data.MySqlClient;

namespace ProyectoArqSoft.Pages
{
    public class MedicamentoUpdateModel : PageModel
    {
        [BindProperty]
        public int IdMedicamento { get; set; }  // Cambiado de Id a IdMedicamento

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
        public int Stock { get; set; }

        // Propiedades para los combos
        public List<string> Presentaciones { get; set; } = new List<string>
        {
            "Tabletas", "Cápsulas", "Jarabe", "Inyectable",
            "Crema", "Gotas", "Suspensión", "Polvo",
            "Ampollas", "Supositorios", "Parche", "Inhalador",
            "Solución", "Emulsión", "Gel", "Ungüento"
        };

        public List<string> Clasificaciones { get; set; } = new List<string>
        {
            "Analgésico", "Antiinflamatorio", "Antibiótico",
            "Antihistamínico", "Antidepresivo", "Antihipertensivo",
            "Antidiabético", "Antifúngico", "Antiviral",
            "Vacuna", "Suplemento", "Ansiolítico",
            "Anticonvulsivo", "Antipsicótico", "Relajante Muscular",
            "Antiácido", "Laxante", "Antidiarreico", "Otros"
        };

        private readonly IConfiguration configuration;
        private readonly MedicamentoValidacion _medicamentoValidacion;

        public MedicamentoUpdateModel(IConfiguration configuration)
        {
            this.configuration = configuration;
            _medicamentoValidacion = new MedicamentoValidacion();
        }

        public IActionResult OnGet(int id)
        {
            string connectionString = configuration.GetConnectionString("MySqlConnection")!;

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    string query = @"SELECT idMedicamento, nombre, presentacion, clasificacion, concentracion, precio, stock 
                                    FROM medicamento 
                                    WHERE idMedicamento = @id AND estado = 1";

                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@id", id);

                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                IdMedicamento = reader.GetInt32("idMedicamento");
                                Nombre = reader.GetString("nombre");
                                Presentacion = reader.GetString("presentacion");
                                Clasificacion = reader.GetString("clasificacion");
                                Concentracion = reader.GetString("concentracion");
                                Precio = reader.GetDecimal("precio");
                                Stock = reader.GetInt32("stock");
                            }
                            else
                            {
                                TempData["ErrorMessage"] = "Medicamento no encontrado";
                                return RedirectToPage("Medicamento");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error al cargar medicamento: {ex.Message}";
                return RedirectToPage("Medicamento");
            }

            return Page();
        }

        public IActionResult OnPost()
        {
            var medicamento = new ProyectoArqSoft.Models.Medicamento
            {
                Nombre = Nombre,
                Presentacion = Presentacion,
                Clasificacion = Clasificacion,
                Concentracion = Concentracion,
                Precio = Precio,
                Stock = Stock
            };

            if (!_medicamentoValidacion.EsValido(medicamento))
            {
                TempData["ErrorMessage"] = _medicamentoValidacion.ObtenerMensajesError();
                return Page();
            }

            string connectionString = configuration.GetConnectionString("MySqlConnection")!;

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    string updateQuery = @"UPDATE medicamento 
                                        SET nombre = @nombre,
                                            presentacion = @presentacion,
                                            clasificacion = @clasificacion,
                                            concentracion = @concentracion,
                                            precio = @precio,
                                            stock = @stock,
                                            ultima_actualizacion = NOW()
                                        WHERE idMedicamento = @idMedicamento";

                    using (MySqlCommand command = new MySqlCommand(updateQuery, connection))
                    {
                        command.Parameters.AddWithValue("@idMedicamento", IdMedicamento);
                        command.Parameters.AddWithValue("@nombre", Nombre?.Trim());
                        command.Parameters.AddWithValue("@presentacion", Presentacion);
                        command.Parameters.AddWithValue("@clasificacion", Clasificacion);
                        command.Parameters.AddWithValue("@concentracion", Concentracion?.Trim());
                        command.Parameters.AddWithValue("@precio", Precio);
                        command.Parameters.AddWithValue("@stock", Stock);

                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            TempData["SuccessMessage"] = "Medicamento actualizado exitosamente";
                            return RedirectToPage("Medicamento");
                        }
                        else
                        {
                            TempData["ErrorMessage"] = "No se pudo actualizar el medicamento";
                            return Page();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error al actualizar: {ex.Message}";
                return Page();
            }
        }
    }
}