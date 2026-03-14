using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using ProyectoArqSoft.Helpers;
using ProyectoArqSoft.Pages.Base;
using ProyectoArqSoft.Validaciones;
using System.Data;

namespace ProyectoArqSoft.Pages
{
    public class BioquimicoModel : BasePageModel
    {
        private readonly string _connectionString;
        public DataTable BioquimicoDataTable { get; set; } = new DataTable();

        public BioquimicoModel(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("MySqlConnection")!;
        }

        public void OnGet(string? filtro, string? mensaje, string? error)
        {
            CargarParametros(filtro, mensaje, error);

            Validacion v = BioquimicoValidacion.ValidarCriterioBusqueda(Estado.FiltroActual);
            if (!v.EsValido)
            {
                Estado.MensajeError = v.MensajeError;
                return;
            }

            CargarDatos(Estado.FiltroActual);

            if (BioquimicoDataTable.Rows.Count == 0 && !string.IsNullOrEmpty(Estado.FiltroActual))
            {
                Estado.Mensaje = "No se encontraron bioquímicos";
            }
        }

        public IActionResult OnPostEliminar(int id)
        {
            if (EjecutarBajaLogica(id) == 0)
                return RedirectToPage(new { error = "Bioquímico no encontrado" });

            return RedirectToPage(new { mensaje = "Bioquímico eliminado correctamente" });
        }

        private void CargarParametros(string? filtro, string? mensaje, string? error)
        {
            Estado.FiltroActual = FiltroHelper.LimpiarFiltro(filtro);
            Estado.Mensaje = mensaje ?? string.Empty;
            Estado.MensajeError = error ?? string.Empty;
        }

        private void CargarDatos(string filtro)
        {
            using var connection = new MySqlConnection(_connectionString);
            
            string query = $@"SELECT idBioquimico, nombres, apellido_paterno, apellido_materno, 
                                     ci, ci_extencion, telefono 
                              FROM bioquimico 
                              WHERE activo = 1 
                              {FiltroSqlHelper.ConstruirCondicionLike(filtro, "nombres", "apellido_paterno", "apellido_materno", "ci", "telefono")}
                              ORDER BY apellido_paterno, nombres";

            using var command = new MySqlCommand(query, connection);
            FiltroSqlHelper.AgregarParametrosLike(command, filtro);
            new MySqlDataAdapter(command).Fill(BioquimicoDataTable);
        }

        private int EjecutarBajaLogica(int id)
        {
            using var connection = new MySqlConnection(_connectionString);
            string query = "UPDATE bioquimico SET activo = 0 WHERE idBioquimico = @id AND activo = 1";
            using var command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@id", id);
            connection.Open();
            return command.ExecuteNonQuery();
        }
    }
}