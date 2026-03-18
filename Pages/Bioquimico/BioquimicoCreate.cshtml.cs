using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using ProyectoArqSoft.Pages.Base;
using ProyectoArqSoft.Validaciones;
using BioquimicoEntidad = ProyectoArqSoft.Models.Bioquimico;

namespace ProyectoArqSoft.Pages
{
    public class BioquimicoCreateModel : BasePageModel
    {
        private readonly IConfiguration configuration;
        private readonly IValidacion<BioquimicoEntidad> validador;

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

        public BioquimicoCreateModel(IConfiguration configuration)
        {
            this.configuration = configuration;
            validador = new BioquimicoFormularioValidacion();
        }

        public void OnGet()
        {
        }

        public IActionResult OnPostCrearBioquimico()
        {
            BioquimicoEntidad bioquimico = ConstruirBioquimico();
            Validacion resultado = ValidarBioquimico(bioquimico);

            if (!resultado.EsValido)
            {
                Estado.MensajeError = resultado.MensajeError;
                return Page();
            }

            resultado = ValidarDuplicado(bioquimico);

            if (!resultado.EsValido)
            {
                Estado.MensajeError = resultado.MensajeError;
                return Page();
            }

            GuardarBioquimico(bioquimico);

            return RedirectToPage("/Bioquimico/Bioquimico", new { mensaje = "Bioquímico registrado correctamente" });
        }

        private BioquimicoEntidad ConstruirBioquimico()
        {
            return new BioquimicoEntidad
            {
                Nombres = LimpiarEspacios(Nombres),
                ApellidoMaterno = LimpiarEspacios(ApellidoMaterno),
                ApellidoPaterno = LimpiarEspacios(ApellidoPaterno),
                Ci = LimpiarEspacios(Ci),
                CiExtencion = LimpiarEspacios(CiExtencion).ToUpperInvariant(),
                Telefono = LimpiarEspacios(Telefono),
                Activo = 1
            };
        }

        private Validacion ValidarBioquimico(BioquimicoEntidad bioquimico)
        {
            return validador.Validar(bioquimico);
        }

        private Validacion ValidarDuplicado(BioquimicoEntidad bioquimico)
        {
            if (ExisteBioquimicoConDocumento(bioquimico.Ci, bioquimico.CiExtencion))
                return new Validacion(false, "Ya existe un bioquímico con ese CI y extensión");

            return new Validacion(true);
        }

        private bool ExisteBioquimicoConDocumento(string ci, string ciExtencion)
        {
            string connectionString = configuration.GetConnectionString("MySqlConnection")!;
            string query = @"SELECT COUNT(*)
                             FROM bioquimico
                             WHERE ci = @ci
                               AND ci_extencion = @ci_extencion
                               AND activo = 1";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@ci", ci);
                command.Parameters.AddWithValue("@ci_extencion", ciExtencion);

                connection.Open();
                return Convert.ToInt32(command.ExecuteScalar()) > 0;
            }
        }

        private void GuardarBioquimico(BioquimicoEntidad bioquimico)
        {
            string connectionString = configuration.GetConnectionString("MySqlConnection")!;
            string query = @"INSERT INTO bioquimico
                             (nombres, apellido_materno, apellido_paterno, ci, ci_extencion, telefono, activo)
                             VALUES
                             (@nombres, @apellido_materno, @apellido_paterno, @ci, @ci_extencion, @telefono, @activo)";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@nombres", bioquimico.Nombres);
                command.Parameters.AddWithValue("@apellido_materno", bioquimico.ApellidoMaterno);
                command.Parameters.AddWithValue("@apellido_paterno", bioquimico.ApellidoPaterno);
                command.Parameters.AddWithValue("@ci", bioquimico.Ci);
                command.Parameters.AddWithValue("@ci_extencion", bioquimico.CiExtencion);
                command.Parameters.AddWithValue("@telefono", bioquimico.Telefono);
                command.Parameters.AddWithValue("@activo", bioquimico.Activo);

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