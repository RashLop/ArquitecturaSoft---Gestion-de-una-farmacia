using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using ProyectoArqSoft.Pages.Base;
using ProyectoArqSoft.Validaciones;
using BioquimicoEntidad = ProyectoArqSoft.Models.Bioquimico;

namespace ProyectoArqSoft.Pages
{
    public class BioquimicoEditModel : BasePageModel
    {
        private readonly IConfiguration configuration;
        private readonly IValidacion<BioquimicoEntidad> validador;

        [BindProperty]
        public int IdBioquimico { get; set; }

        [BindProperty]
        public string Nombres { get; set; } = string.Empty;

        [BindProperty]
        [Display(Name = "Apellido Materno")]
        public string ApellidoMaterno { get; set; } = string.Empty;

        [BindProperty]
        [Display(Name = "Apellido Paterno")]
        public string ApellidoPaterno { get; set; } = string.Empty;

        [BindProperty]
        [Display(Name = "CI Extensión")]
        public string CiExtencion { get; set; } = string.Empty;

        [BindProperty]
        [Display(Name = "CI")]

        public string Ci { get; set; } = string.Empty;

        [BindProperty]
        [Display(Name = "Teléfono")]
        public string Telefono { get; set; } = string.Empty;

        public BioquimicoEditModel(IConfiguration configuration)
        {
            this.configuration = configuration;
            validador = new BioquimicoFormularioValidacion();
        }

        public IActionResult OnGet(int id)
        {
            string connectionString = configuration.GetConnectionString("MySqlConnection")!;
            string query = @"SELECT idBioquimico,
                                    nombres,
                                    apellido_materno,
                                    apellido_paterno,
                                    ci,
                                    ci_extencion,
                                    telefono
                             FROM bioquimico
                             WHERE idBioquimico = @id
                               AND activo = 1";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@id", id);

                connection.Open();

                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    if (!reader.Read())
                        return RedirectToPage("/Bioquimico/Bioquimico", new { error = "Bioquímico no encontrado" });

                    IdBioquimico = Convert.ToInt32(reader["idBioquimico"]);
                    Nombres = LimpiarEspacios(reader["nombres"].ToString());
                    ApellidoMaterno = LimpiarEspacios(reader["apellido_materno"].ToString());
                    ApellidoPaterno = LimpiarEspacios(reader["apellido_paterno"].ToString());
                    Ci = LimpiarEspacios(reader["ci"].ToString());
                    CiExtencion = LimpiarEspacios(reader["ci_extencion"].ToString());
                    Telefono = LimpiarEspacios(reader["telefono"].ToString());
                }
            }

            return Page();
        }

        public IActionResult OnPost()
        {
            BioquimicoEntidad bioquimico = ConstruirBioquimico();
            Validacion resultado = ValidarBioquimico(bioquimico);

            if (!resultado.EsValido)
            {
                Estado.MensajeError = resultado.MensajeError;
                return Page();
            }

            resultado = ValidarExistencia(bioquimico.IdBioquimico);

            if (!resultado.EsValido)
                return RedirectToPage("/Bioquimico/Bioquimico", new { error = resultado.MensajeError });

            resultado = ValidarDuplicado(bioquimico);

            if (!resultado.EsValido)
            {
                Estado.MensajeError = resultado.MensajeError;
                return Page();
            }

            ActualizarBioquimico(bioquimico);

            return RedirectToPage("/Bioquimico/Bioquimico", new { mensaje = "Bioquímico actualizado correctamente" });
        }

        private BioquimicoEntidad ConstruirBioquimico()
        {
            return new BioquimicoEntidad
            {
                IdBioquimico = IdBioquimico,
                Nombres = LimpiarEspacios(Nombres),
                ApellidoMaterno = LimpiarEspacios(ApellidoMaterno),
                ApellidoPaterno = LimpiarEspacios(ApellidoPaterno),
                Ci = LimpiarEspacios(Ci),
                CiExtencion = LimpiarEspacios(CiExtencion).ToUpperInvariant(),
                Telefono = LimpiarEspacios(Telefono)
            };
        }

        private Validacion ValidarBioquimico(BioquimicoEntidad bioquimico)
        {
            return validador.Validar(bioquimico);
        }

        private Validacion ValidarExistencia(int idBioquimico)
        {
            if (!ExisteBioquimicoPorId(idBioquimico))
                return new Validacion(false, "Bioquímico no encontrado");

            return new Validacion(true);
        }

        private Validacion ValidarDuplicado(BioquimicoEntidad bioquimico)
        {
            if (ExisteOtroBioquimicoConDocumento(bioquimico.Ci, bioquimico.CiExtencion, bioquimico.IdBioquimico))
                return new Validacion(false, "Ya existe otro bioquímico con ese CI y extensión");

            return new Validacion(true);
        }

        private bool ExisteBioquimicoPorId(int idBioquimico)
        {
            string connectionString = configuration.GetConnectionString("MySqlConnection")!;
            string query = @"SELECT COUNT(*)
                             FROM bioquimico
                             WHERE idBioquimico = @id
                               AND activo = 1";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@id", idBioquimico);

                connection.Open();
                return Convert.ToInt32(command.ExecuteScalar()) > 0;
            }
        }

        private bool ExisteOtroBioquimicoConDocumento(string ci, string ciExtencion, int idBioquimico)
        {
            string connectionString = configuration.GetConnectionString("MySqlConnection")!;
            string query = @"SELECT COUNT(*)
                             FROM bioquimico
                             WHERE ci = @ci
                               AND ci_extencion = @ci_extencion
                               AND idBioquimico <> @id
                               AND activo = 1";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@ci", ci);
                command.Parameters.AddWithValue("@ci_extencion", ciExtencion);
                command.Parameters.AddWithValue("@id", idBioquimico);

                connection.Open();
                return Convert.ToInt32(command.ExecuteScalar()) > 0;
            }
        }

        private void ActualizarBioquimico(BioquimicoEntidad bioquimico)
        {
            string connectionString = configuration.GetConnectionString("MySqlConnection")!;
            string query = @"UPDATE bioquimico
                             SET nombres = @nombres,
                                 apellido_materno = @apellido_materno,
                                 apellido_paterno = @apellido_paterno,
                                 ci = @ci,
                                 ci_extencion = @ci_extencion,
                                 telefono = @telefono,
                                 ultima_actualizacion = NOW()
                             WHERE idBioquimico = @id";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@id", bioquimico.IdBioquimico);
                command.Parameters.AddWithValue("@nombres", bioquimico.Nombres);
                command.Parameters.AddWithValue("@apellido_materno", bioquimico.ApellidoMaterno);
                command.Parameters.AddWithValue("@apellido_paterno", bioquimico.ApellidoPaterno);
                command.Parameters.AddWithValue("@ci", bioquimico.Ci);
                command.Parameters.AddWithValue("@ci_extencion", bioquimico.CiExtencion);
                command.Parameters.AddWithValue("@telefono", bioquimico.Telefono);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        private static string LimpiarEspacios(string valor)
        {
            return string.IsNullOrWhiteSpace(valor) ? string.Empty : valor.Trim();
        }
    }
}