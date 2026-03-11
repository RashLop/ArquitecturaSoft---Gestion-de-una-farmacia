using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using ProyectoArqSoft.Helpers;
using ProyectoArqSoft.Validaciones;
using ProyectoArqSoft.Pages.Base;
using MedicamentoEntidad = ProyectoArqSoft.Models.Medicamento;


namespace ProyectoArqSoft.Pages
{
    public class MedicamentoUpdateModel : BasePageModel
    {
        private readonly IConfiguration configuration;
        private readonly IValidacion<MedicamentoEntidad> validador;

        [BindProperty]
        public int IdMedicamento { get; set; }

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

        public MedicamentoUpdateModel(IConfiguration configuration)
        {
            this.configuration = configuration;
            validador = new MedicamentoValidacion();
        }

        public IActionResult OnGet(int id)
        {
            string connectionString = configuration.GetConnectionString("MySqlConnection")!;
            string query = @"SELECT id_medicamento, nombre, presentacion, clasificacion, concentracion, precio, stock
                             FROM medicamento
                             WHERE id_medicamento = @id_medicamento";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@id_medicamento", id);

                connection.Open();

                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        IdMedicamento = Convert.ToInt32(reader["id_medicamento"]);
                        Nombre = StringHelper.LimpiarEspacios(reader["nombre"].ToString());
                        Presentacion = StringHelper.LimpiarEspacios(reader["presentacion"].ToString());
                        Clasificacion = StringHelper.LimpiarEspacios(reader["clasificacion"].ToString());
                        Concentracion = StringHelper.LimpiarEspacios(reader["concentracion"].ToString());
                        Precio = Convert.ToDecimal(reader["precio"]);
                        Stock = Convert.ToInt32(reader["stock"]);
                    }
                    else
                    {
                        return RedirectToPage("Medicamento");
                    }
                }
            }

            return Page();
        }

        public IActionResult OnPost()
        {
            MedicamentoEntidad medicamento = ConstruirMedicamento();
            Validacion resultado = ValidarMedicamento(medicamento);

            if (!resultado.EsValido)
            {
                Estado.MensajeError = resultado.MensajeError;
                return Page();
            }

            if (!ModelState.IsValid)
            {
                return Page();
            }

            ActualizarMedicamento(medicamento);

            return RedirectToPage("Medicamento");
        }

        private MedicamentoEntidad ConstruirMedicamento()
        {
            return new MedicamentoEntidad
            {
                Id = IdMedicamento,
                Nombre = StringHelper.QuitarEspacios(Nombre),
                Presentacion = StringHelper.LimpiarEspacios(Presentacion),
                Clasificacion = StringHelper.LimpiarEspacios(Clasificacion),
                Concentracion = StringHelper.LimpiarEspacios(Concentracion),
                Precio = Precio,
                Stock = Stock
            };
        }

        private Validacion ValidarMedicamento(MedicamentoEntidad medicamento)
        {
            return validador.Validar(medicamento);
        }

        private void ActualizarMedicamento(MedicamentoEntidad medicamento)
        {
            string connectionString = configuration.GetConnectionString("MySqlConnection")!;
            string query = @"UPDATE medicamento
                             SET nombre=@nombre,
                                 presentacion=@presentacion,
                                 clasificacion=@clasificacion,
                                 concentracion=@concentracion,
                                 precio=@precio,
                                 stock=@stock,
                                 ultima_actualizacion = NOW()
                             WHERE id_medicamento=@id_medicamento";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                MySqlCommand command = new MySqlCommand(query, connection);

                command.Parameters.AddWithValue("@id_medicamento", IdMedicamento);
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
    }
}