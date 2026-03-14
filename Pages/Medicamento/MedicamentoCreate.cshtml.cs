using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ProyectoArqSoft.Models;
using ProyectoArqSoft.Validaciones;
using MySql.Data.MySqlClient;

namespace ProyectoArqSoft.Pages
{
    public class MedicamentoCreateModel : PageModel
    {
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

        private readonly IConfiguration configuration;
        private readonly MedicamentoValidacion _medicamentoValidacion;

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

        public MedicamentoCreateModel(IConfiguration configuration)
        {
            this.configuration = configuration;
            _medicamentoValidacion = new MedicamentoValidacion();
        }

        public void OnGet()
        {
            // Valores por defecto
            Precio = 0;
            Stock = 0;
        }

        public IActionResult OnPost()
        {
            // Crear objeto Medicamento para validar
            var medicamento = new ProyectoArqSoft.Models.Medicamento
            {
                Nombre = Nombre,
                Presentacion = Presentacion,
                Clasificacion = Clasificacion,
                Concentracion = Concentracion,
                Precio = Precio,
                Stock = Stock
            };

            // Usar EsValido() para validar
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

                    string insertQuery = @"INSERT INTO medicamento 
                        (nombre, presentacion, clasificacion, concentracion, precio, stock, estado, fecha_registro, ultima_actualizacion) 
                        VALUES 
                        (@nombre, @presentacion, @clasificacion, @concentracion, @precio, @stock, 1, NOW(), NOW())";

                    using (MySqlCommand command = new MySqlCommand(insertQuery, connection))
                    {
                        command.Parameters.AddWithValue("@nombre", Nombre?.Trim());
                        command.Parameters.AddWithValue("@presentacion", Presentacion);
                        command.Parameters.AddWithValue("@clasificacion", Clasificacion);
                        command.Parameters.AddWithValue("@concentracion", Concentracion?.Trim());
                        command.Parameters.AddWithValue("@precio", Precio);
                        command.Parameters.AddWithValue("@stock", Stock);

                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            TempData["SuccessMessage"] = "Medicamento registrado exitosamente";
                            return RedirectToPage("Medicamento");
                        }
                        else
                        {
                            TempData["ErrorMessage"] = "No se pudo registrar el medicamento";
                            return Page();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error al registrar: {ex.Message}";
                return Page();
            }
        }
    }
}