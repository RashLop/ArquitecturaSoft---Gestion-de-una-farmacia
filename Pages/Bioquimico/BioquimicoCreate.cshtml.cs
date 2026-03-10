using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MySql.Data.MySqlClient;
using System.Linq;

namespace ProyectoArqSoft.Pages
{
    public class BioquimicoCreateModel : PageModel
    {
        private readonly IConfiguration configuration;

        [BindProperty]
        public string Nombres { get; set; } = string.Empty;

        [BindProperty]
        public string Apellidos { get; set; } = string.Empty;

        [BindProperty]
        public string Ci { get; set; } = string.Empty;

        [BindProperty]
        public string Telefono { get; set; } = string.Empty;

        [BindProperty]
        public bool Activo { get; set; }

        public string MensajeError { get; set; } = string.Empty;

        public BioquimicoCreateModel(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            if (string.IsNullOrWhiteSpace(Nombres) ||
                string.IsNullOrWhiteSpace(Apellidos) ||
                string.IsNullOrWhiteSpace(Ci) ||
                string.IsNullOrWhiteSpace(Telefono))
            {
                MensajeError = "Complete los campos obligatorios";
                return Page();
            }

            if (!EsTelefonoValido(Telefono))
            {
                MensajeError = "Teléfono inválido";
                return Page();
            }

            Activo = true;

            string connectionString = configuration.GetConnectionString("MySqlConnection")!;
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                string queryExiste = @"SELECT COUNT(*) 
                                       FROM bioquimico
                                       WHERE ci = @ci";

                using (MySqlCommand cmdExiste = new MySqlCommand(queryExiste, connection))
                {
                    cmdExiste.Parameters.AddWithValue("@ci", Ci.Trim());
                    int existe = Convert.ToInt32(cmdExiste.ExecuteScalar());

                    if (existe > 0)
                    {
                        MensajeError = "Bioquímico ya registrado";
                        return Page();
                    }
                }

                string queryInsert = @"INSERT INTO bioquimico
                                       (nombres, apellidos, ci, telefono, activo)
                                       VALUES
                                       (@nombres, @apellidos, @ci, @telefono, @activo)";

                using (MySqlCommand cmdInsert = new MySqlCommand(queryInsert, connection))
                {
                    cmdInsert.Parameters.AddWithValue("@nombres", Nombres.Trim());
                    cmdInsert.Parameters.AddWithValue("@apellidos", Apellidos.Trim());
                    cmdInsert.Parameters.AddWithValue("@ci", Ci.Trim());
                    cmdInsert.Parameters.AddWithValue("@telefono", Telefono.Trim());
                    cmdInsert.Parameters.AddWithValue("@activo", Activo);

                    cmdInsert.ExecuteNonQuery();
                }
            }

            return RedirectToPage("/Bioquimico/Bioquimico", new { mensaje = "Bioquímico registrado correctamente" });
        }

        private bool EsTelefonoValido(string telefono)
        {
            telefono = telefono.Trim();
            return telefono.All(char.IsDigit) && telefono.Length == 8;
        }
    }
}