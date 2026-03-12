using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using ProyectoArqSoft.Helpers;
using ProyectoArqSoft.Validaciones;
using ProyectoArqSoft.Pages.Base;
using System.Data;

namespace ProyectoArqSoft.Pages
{
    public class MedicamentoModel : BasePageModel
    {
        private readonly IConfiguration configuration;
        public DataTable MedicamentoDataTable { get; set; } = new DataTable();


        public MedicamentoModel(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public void OnGetListarMedicamentos(string? filtro, string? mensaje, string? error)
        {
            CargarParametros(filtro, mensaje, error);

            Validacion resultado = FiltroHelper.ValidarFiltro(Estado.FiltroActual);
            Estado.MensajeError = resultado.MensajeError;

            if (!resultado.EsValido)
                return;

            CargarMedicamentos(Estado.FiltroActual);
        }

        public IActionResult OnPostEliminarMedicamentoLogicamente(int id)
        {
            SoftDeleteMedicamento(id);
            return RedirectToPage();
        }

        private void CargarParametros(string? filtro, string? mensaje, string? error)
        {
            Estado.FiltroActual = FiltroHelper.LimpiarFiltro(filtro);
            Estado.Mensaje = mensaje ?? string.Empty;
            Estado.MensajeError = error ?? string.Empty;
        }

        private void CargarMedicamentos(string filtro)
        {
            string connectionString = configuration.GetConnectionString("MySqlConnection")!;

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                string query = ConstruirQuery(filtro);
                MySqlCommand command = new MySqlCommand(query, connection);

                FiltroSqlHelper.AgregarParametrosLike(command, filtro);

                MySqlDataAdapter adapter = new MySqlDataAdapter(command);
                adapter.Fill(MedicamentoDataTable);
            }
        }

        private string ConstruirQuery(string filtro)
        {
            string query = @"SELECT id_medicamento,
                                    nombre,
                                    presentacion,
                                    clasificacion,
                                    concentracion,
                                    precio
                             FROM medicamento
                             WHERE estado = 1";

            query += FiltroSqlHelper.ConstruirCondicionLike(
                filtro,
                "nombre",
                "presentacion",
                "clasificacion"
            );

            query += " ORDER BY nombre";

            return query;
        }

        private void SoftDeleteMedicamento(int id)
        {
            string connectionString = configuration.GetConnectionString("MySqlConnection")!;

            string query = @"UPDATE medicamento
                             SET estado = 0,
                                 ultima_actualizacion = NOW()
                             WHERE id_medicamento = @id";
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