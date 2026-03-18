using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using ProyectoArqSoft.Helpers;
using ProyectoArqSoft.Pages.Base;
using ProyectoArqSoft.Validaciones;
using ClienteEntidad = ProyectoArqSoft.Models.Cliente;

namespace ProyectoArqSoft.Pages
{
    public class ClienteEditModel : BasePageModel
    {
        private readonly IConfiguration configuration;
        private readonly IValidacion<ClienteEntidad> validador;

        [BindProperty]
        public int IdCliente { get; set; }

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

        public ClienteEditModel(IConfiguration configuration)
        {
            this.configuration = configuration;
            validador = new ClienteValidacion();
        }

        public void OnGet()
        {
        }

        public IActionResult OnPostCargarClienteParaEdicion(int id)
        {
            string connectionString = configuration.GetConnectionString("MySqlConnection")!;
            string query = @"SELECT idCliente,
                                    tipo_cliente,
                                    nombre,
                                    apellido_materno,
                                    apellido_paterno,
                                    ci_extencion,
                                    ci,
                                    fecha_de_nacimiento,
                                    telefono
                             FROM cliente
                             WHERE idCliente = @id AND estado = 1";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@id", id);

                connection.Open();

                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    if (!reader.Read())
                        return RedirectToPage("Cliente", new { error = "Cliente no encontrado" });

                    IdCliente = Convert.ToInt32(reader["idCliente"]);
                    TipoCliente = StringHelper.LimpiarEspacios(reader["tipo_cliente"].ToString());
                    Nombre = StringHelper.LimpiarEspacios(reader["nombre"].ToString());
                    ApellidoMaterno = StringHelper.LimpiarEspacios(reader["apellido_materno"].ToString());
                    ApellidoPaterno = StringHelper.LimpiarEspacios(reader["apellido_paterno"].ToString());
                    CiExtencion = StringHelper.LimpiarEspacios(reader["ci_extencion"].ToString());
                    Ci = StringHelper.Limpiar(reader["ci"].ToString());
                    FechaDeNacimiento = Convert.ToDateTime(reader["fecha_de_nacimiento"]);
                    Telefono = StringHelper.LimpiarEspacios(reader["telefono"].ToString());
                }
            }

            return Page();
        }

        public IActionResult OnPostActualizarCliente()
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

            ActualizarCliente(cliente);

            return RedirectToPage("Cliente", new { mensaje = "Cliente actualizado correctamente" });
        }

        private ClienteEntidad ConstruirCliente()
        {
            return new ClienteEntidad
            {
                IdCliente = IdCliente,
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
            if (ExisteClienteConDocumento(cliente.Ci, cliente.CiExtencion, IdCliente))
                return new Validacion(false, "Ya existe otro cliente con ese CI y extension");

            return new Validacion(true);
        }

        private bool ExisteClienteConDocumento(string ci, string ciExtencion, int idCliente)
        {
            string connectionString = configuration.GetConnectionString("MySqlConnection")!;
            string query = @"SELECT COUNT(*)
                             FROM cliente
                             WHERE ci = @ci
                               AND ci_extencion = @ci_extencion
                               AND idCliente <> @id
                               AND estado = 1";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@ci", ci);
                command.Parameters.AddWithValue("@ci_extencion", ciExtencion);
                command.Parameters.AddWithValue("@id", idCliente);

                connection.Open();
                return Convert.ToInt32(command.ExecuteScalar()) > 0;
            }
        }

        private void ActualizarCliente(ClienteEntidad cliente)
        {
            string connectionString = configuration.GetConnectionString("MySqlConnection")!;
            string query = @"UPDATE cliente
                             SET tipo_cliente = @tipo_cliente,
                                 nombre = @nombre,
                                 apellido_materno = @apellido_materno,
                                 apellido_paterno = @apellido_paterno,
                                 ci_extencion = @ci_extencion,
                                 ci = @ci,
                                 fecha_de_nacimiento = @fecha_de_nacimiento,
                                 telefono = @telefono,
                                 ultima_actualizacion = NOW()
                             WHERE idCliente = @id";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@id", cliente.IdCliente);
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
