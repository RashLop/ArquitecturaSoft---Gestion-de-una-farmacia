using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using ProyectoArqSoft.Helpers;
using MedicamentoEntidad = ProyectoArqSoft.Domain.Entities.Medicamento.Medicamento;
using ProyectoArqSoft.Base;
using ProyectoArqSoft.Application.Validaciones;

namespace ProyectoArqSoft.Pages
{
    public class MedicamentoCreateModel : BasePageModel
    {
        private readonly IConfiguration configuration;
        private readonly IValidacion<MedicamentoEntidad> validador;

        [BindProperty]
        public string Nombre { get; set; } = string.Empty;

        [BindProperty]
        public string Presentacion { get; set; } = string.Empty;

        [BindProperty]
        public string Clasificacion { get; set; } = string.Empty;

        [BindProperty]
        public string Concentracion { get; set; } = string.Empty;

        [BindProperty]
        public decimal Precio { get; set; }

        [BindProperty]
        public int Stock { get; set; }

        public MedicamentoCreateModel(IConfiguration configuration)
        {
            this.configuration = configuration;
            validador = new MedicamentoValidacion();
        }

        public void OnGet()
        {
        }

        public IActionResult OnPostCrearMedicamento()
        {
            MedicamentoEntidad medicamento = ConstruirMedicamento();
            Validacion resultado = ValidarMedicamento(medicamento);

            if (!resultado.EsValido)
            {
                Estado.MensajeError = resultado.MensajeError;
                return Page();
            }

            GuardarMedicamento(medicamento);

            return RedirectToPage("Medicamento");
        }
        private Validacion ValidarMedicamento(MedicamentoEntidad medicamento)
        {
            return validador.Validar(medicamento);
        }

        private void GuardarMedicamento(MedicamentoEntidad medicamento)
        {
            string connectionString = configuration.GetConnectionString("MySqlConnection")!;
            string query = @"INSERT INTO medicamento 
                            (nombre, presentacion, clasificacion, concentracion, precio, stock)
                            VALUES
                            (@nombre, @presentacion, @clasificacion, @concentracion, @precio, @stock)";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                MySqlCommand command = new MySqlCommand(query, connection);

                command.Parameters.AddWithValue("@nombre", medicamento.Nombre);
                command.Parameters.AddWithValue("@presentacion", medicamento.Presentacion);
                command.Parameters.AddWithValue("@clasificacion", medicamento.Clasificacion);
                command.Parameters.AddWithValue("@concentracion", medicamento.Concentracion);
                command.Parameters.AddWithValue("@precio", medicamento.Precio);
                command.Parameters.AddWithValue("@stock", medicamento.Stock);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        private MedicamentoEntidad ConstruirMedicamento()
        {
            return new MedicamentoEntidad
            {
                Nombre = StringHelper.QuitarEspacios(Nombre),
                Presentacion = StringHelper.Limpiar(Presentacion),
                Clasificacion = StringHelper.Limpiar(Clasificacion),
                Concentracion = StringHelper.Limpiar(Concentracion),
                Precio = Precio,
                Stock = Stock
            };
        }
    }
}