using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MySql.Data.MySqlClient;
using System.Linq;

namespace ProyectoArqSoft.Pages
{
    public class BioquimicoEditModel : PageModel
    {
        private readonly IConfiguration configuration;

        [BindProperty]
        public int IdBioquimico { get; set; }

        [BindProperty]
        public string Nombres { get; set; } = string.Empty;

        [BindProperty]
        public string Apellidos { get; set; } = string.Empty;

        [BindProperty]
        public string Ci { get; set; } = string.Empty;

        [BindProperty]
        public string Telefono { get; set; } = string.Empty;

        public string MensajeError { get; set; } = string.Empty;

        public BioquimicoEditModel(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public IActionResult OnGet(int id)
        {
            string connectionString = configuration.GetConnectionString("MySqlConnection")!;

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                string query = @"SELECT idBioquimico, nombres, apellidos, ci, telefono
                                 FROM bioquimico
                                 WHERE idBioquimico = @id";

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id", id);

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        if (!reader.Read())
                        {
                            return RedirectToPage("/Bioquimico/Bioquimico", new { error = "Bioquímico no encontrado" });
                        }

                        IdBioquimico = Convert.ToInt32(reader["idBioquimico"]);
                        Nombres = reader["nombres"].ToString() ?? string.Empty;
                        Apellidos = reader["apellidos"].ToString() ?? string.Empty;
                        Ci = reader["ci"].ToString() ?? string.Empty;
                        Telefono = reader["telefono"].ToString() ?? string.Empty;
                    }
                }
            }

            return Page();
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

            string connectionString = configuration.GetConnectionString("MySqlConnection")!;

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                string queryExiste = @"SELECT COUNT(*) 
                                       FROM bioquimico
                                       WHERE idBioquimico = @id";

                using (MySqlCommand cmdExiste = new MySqlCommand(queryExiste, connection))
                {
                    cmdExiste.Parameters.AddWithValue("@id", IdBioquimico);
                    int existe = Convert.ToInt32(cmdExiste.ExecuteScalar());

                    if (existe == 0)
                    {
                        return RedirectToPage("/Bioquimico/Bioquimico", new { error = "Bioquímico no encontrado" });
                    }
                }

                string queryCiDuplicado = @"SELECT COUNT(*)
                                            FROM bioquimico
                                            WHERE ci = @ci AND idBioquimico <> @id";

                using (MySqlCommand cmdDuplicado = new MySqlCommand(queryCiDuplicado, connection))
                {
                    cmdDuplicado.Parameters.AddWithValue("@ci", Ci.Trim());
                    cmdDuplicado.Parameters.AddWithValue("@id", IdBioquimico);

                    int duplicado = Convert.ToInt32(cmdDuplicado.ExecuteScalar());

                    if (duplicado > 0)
                    {
                        MensajeError = "Ya existe un bioquímico con ese CI";
                        return Page();
                    }
                }

                string queryUpdate = @"UPDATE bioquimico
                                       SET nombres = @nombres,
                                           apellidos = @apellidos,
                                           ci = @ci,
                                           telefono = @telefono
                                       WHERE idBioquimico = @id";

                using (MySqlCommand cmdUpdate = new MySqlCommand(queryUpdate, connection))
                {
                    cmdUpdate.Parameters.AddWithValue("@nombres", Nombres.Trim());
                    cmdUpdate.Parameters.AddWithValue("@apellidos", Apellidos.Trim());
                    cmdUpdate.Parameters.AddWithValue("@ci", Ci.Trim());
                    cmdUpdate.Parameters.AddWithValue("@telefono", Telefono.Trim());
                    cmdUpdate.Parameters.AddWithValue("@id", IdBioquimico);

                    cmdUpdate.ExecuteNonQuery();
                }
            }

            return RedirectToPage("/Bioquimico/Bioquimico", new { mensaje = "Bioquímico actualizado correctamente" });
        }

        private bool EsTelefonoValido(string telefono)
        {
            telefono = telefono.Trim();
            return telefono.All(char.IsDigit) && telefono.Length == 8;
        }
    }
}