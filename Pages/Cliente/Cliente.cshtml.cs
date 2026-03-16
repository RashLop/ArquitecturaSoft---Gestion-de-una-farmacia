using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using ProyectoArqSoft.Application.Validaciones;
using ProyectoArqSoft.Base;
using ProyectoArqSoft.Helpers;
using System.Data;

namespace ProyectoArqSoft.Pages
{
    public class ClienteModel : BasePageModel
    {
        private readonly IConfiguration configuration;

        public DataTable ClienteDataTable { get; set; } = new DataTable();

        public ClienteModel(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public void OnGet(string? filtro, string? mensaje, string? error)
        {
            CargarParametros(filtro, mensaje, error);

            Validacion resultado = FiltroHelper.ValidarFiltro(Estado.FiltroActual);
            Estado.MensajeError = resultado.MensajeError;

            if (!resultado.EsValido)
                return;

            CargarClientes(Estado.FiltroActual);
        }

        public IActionResult OnPostEliminarClienteLogicamente(int id)
        {
            EliminarClienteLogicamente(id);
            return RedirectToPage("Cliente", new { mensaje = "Cliente eliminado correctamente" });
        }

        private void CargarParametros(string? filtro, string? mensaje, string? error)
        {
            Estado.FiltroActual = FiltroHelper.LimpiarFiltro(filtro);
            Estado.Mensaje = mensaje ?? string.Empty;
            Estado.MensajeError = error ?? string.Empty;
        }

        private void CargarClientes(string filtro)
        {
            string connectionString = configuration.GetConnectionString("MySqlConnection")!;

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                string query = ConstruirQuery(filtro);
                MySqlCommand command = new MySqlCommand(query, connection);

                FiltroSqlHelper.AgregarParametrosLike(command, filtro);

                MySqlDataAdapter adapter = new MySqlDataAdapter(command);
                adapter.Fill(ClienteDataTable);
            }
        }

        private string ConstruirQuery(string filtro)
        {
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
                             WHERE estado = 1";

            query += FiltroSqlHelper.ConstruirCondicionLike(
                filtro,
                "nombre",
                "apellido_materno",
                "apellido_paterno",
                "tipo_cliente",
                "ci",
                "ci_extencion",
                "telefono"
            );

            query += " ORDER BY nombre, apellido_paterno, apellido_materno";

            return query;
        }

        private void EliminarClienteLogicamente(int id)
        {
            string connectionString = configuration.GetConnectionString("MySqlConnection")!;
            string query = @"UPDATE cliente
                             SET estado = 0,
                                 ultima_actualizacion = NOW()
                             WHERE idCliente = @id";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@id", id);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }
    }
}
