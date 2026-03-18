using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using ProyectoArqSoft.Helpers;
using ProyectoArqSoft.Pages.Base;
using ProyectoArqSoft.Validaciones;
using ClienteEntidad = ProyectoArqSoft.Models.Cliente;

namespace ProyectoArqSoft.Pages
{
    public class ClienteCreateModel : BasePageModel
    {
        private readonly IConfiguration configuration;
        private readonly IValidacion<ClienteEntidad> validador;

        [BindProperty]
        [Display(Name = "Tipo de Cliente")]
        public string TipoCliente { get; set; } = string.Empty;

        [BindProperty]
        [Display(Name = "Nombre")]
        public string Nombre { get; set; } = string.Empty;

        [BindProperty]
        [Display(Name = "Apellido Materno")]
        public string ApellidoMaterno { get; set; } = string.Empty;

        [BindProperty]
        [Display(Name = "Apellido Paterno")]
        public string ApellidoPaterno { get; set; } = string.Empty;

        [BindProperty]
        [Display(Name = "Extensión")]

        public string CiExtencion { get; set; } = string.Empty;

        [BindProperty]
        [Display(Name = "CI")]

        public string Ci { get; set; } = string.Empty;

        [BindProperty]
        [Display(Name = "Fecha de Nacimiento")]
        public DateTime FechaDeNacimiento { get; set; }

        [BindProperty]
        [Display(Name = "Teléfono")]
        public string Telefono { get; set; } = string.Empty;

        public ClienteCreateModel(IConfiguration configuration)
        {
            this.configuration = configuration;
            validador = new ClienteValidacion();
        }

        public void OnGet()
        {
        }

        public IActionResult OnPostCrearCliente()
        {
            ClienteEntidad cliente = ConstruirCliente();
            Validacion resultado = ValidarCliente(cliente);

            if (!resultado.EsValido)
            {
                Estado.MensajeError = resultado.MensajeError;
                return Page();
            }

            resultado = ValidarDuplicado(cliente);

            if (!resultado.EsValido)
            {
                Estado.MensajeError = resultado.MensajeError;
                return Page();
            }

            GuardarCliente(cliente);

            return RedirectToPage("Cliente", new { mensaje = "Cliente registrado correctamente" });
        }

        private ClienteEntidad ConstruirCliente()
        {
            return new ClienteEntidad
            {
                TipoCliente = StringHelper.LimpiarEspacios(TipoCliente),
                Nombre = StringHelper.LimpiarEspacios(Nombre),
                ApellidoMaterno = StringHelper.LimpiarEspacios(ApellidoMaterno),
                ApellidoPaterno = StringHelper.LimpiarEspacios(ApellidoPaterno),
                CiExtencion = StringHelper.LimpiarEspacios(CiExtencion).ToUpperInvariant(),
                Ci = NormalizarCi(Ci),
                FechaDeNacimiento = FechaDeNacimiento,
                Telefono = StringHelper.LimpiarEspacios(Telefono)
            };
        }

        private static string NormalizarCi(string ci)
        {
            string valor = StringHelper.Limpiar(ci);

            int indiceGuion = valor.IndexOf('-');
            if (indiceGuion < 0)
                return valor;

            string numero = valor[..indiceGuion];
            string complemento = valor[(indiceGuion + 1)..].ToUpperInvariant();
            return $"{numero}-{complemento}";
        }

        private Validacion ValidarCliente(ClienteEntidad cliente)
        {
            return validador.Validar(cliente);
        }

        private Validacion ValidarDuplicado(ClienteEntidad cliente)
        {
            if (ExisteClienteConDocumento(cliente.Ci, cliente.CiExtencion))
                return new Validacion(false, "Ya existe un cliente con ese CI y extension");

            return new Validacion(true);
        }

        private bool ExisteClienteConDocumento(string ci, string ciExtencion)
        {
            string connectionString = configuration.GetConnectionString("MySqlConnection")!;
            string query = @"SELECT COUNT(*)
                             FROM cliente
                             WHERE ci = @ci
                               AND ci_extencion = @ci_extencion
                               AND estado = 1";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@ci", ci);
                command.Parameters.AddWithValue("@ci_extencion", ciExtencion);

                connection.Open();
                return Convert.ToInt32(command.ExecuteScalar()) > 0;
            }
        }

        private void GuardarCliente(ClienteEntidad cliente)
        {
            string connectionString = configuration.GetConnectionString("MySqlConnection")!;
            string query = @"INSERT INTO cliente
                             (tipo_cliente, nombre, apellido_materno, apellido_paterno, ci_extencion, ci, fecha_de_nacimiento, telefono)
                             VALUES
                             (@tipo_cliente, @nombre, @apellido_materno, @apellido_paterno, @ci_extencion, @ci, @fecha_de_nacimiento, @telefono)";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@tipo_cliente", cliente.TipoCliente);
                command.Parameters.AddWithValue("@nombre", cliente.Nombre);
                command.Parameters.AddWithValue("@apellido_materno", cliente.ApellidoMaterno);
                command.Parameters.AddWithValue("@apellido_paterno", cliente.ApellidoPaterno);
                command.Parameters.AddWithValue("@ci_extencion", cliente.CiExtencion);
                command.Parameters.AddWithValue("@ci", cliente.Ci);
                command.Parameters.AddWithValue("@fecha_de_nacimiento", cliente.FechaDeNacimiento);
                command.Parameters.AddWithValue("@telefono", string.IsNullOrWhiteSpace(cliente.Telefono) ? DBNull.Value : cliente.Telefono);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }
    }
}
