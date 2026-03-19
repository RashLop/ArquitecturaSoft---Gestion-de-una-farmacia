using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ProyectoArqSoft.FactoryProducts;
using MedicamentoEntidad = ProyectoArqSoft.Models.Medicamento; // Alias para la entidad
using System.Data;

namespace ProyectoArqSoft.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IRepository<MedicamentoEntidad> _medicamentoRepository; // Usando el alias

        public string? Usuario { get; set; }
        public DataTable MedicamentoDataTable { get; set; } = new DataTable();
        public int TotalMedicamentos { get; set; }

        // Constructor con la interfaz usando el alias
        public IndexModel(
            ILogger<IndexModel> logger,
            IRepository<MedicamentoEntidad> medicamentoRepository) // Usando el alias
        {
            _logger = logger;
            _medicamentoRepository = medicamentoRepository;
        }

        public void OnGet()
        {
            Usuario = HttpContext.Session.GetString("Usuario");

            try
            {
                // Usar el repositorio para obtener el total
                var todosMedicamentos = _medicamentoRepository.GetAll();
                TotalMedicamentos = todosMedicamentos?.Rows.Count ?? 0;

                CargarMedicamentosDestacados();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al cargar datos en Index");
            }
        }

        private void CargarMedicamentosDestacados()
        {
            try
            {
                // Obtener todos los medicamentos y filtrar los destacados
                var todos = _medicamentoRepository.GetAll();

                if (todos != null && todos.Rows.Count > 0)
                {
                    // Tomar los primeros 3 (o menos si hay menos)
                    var destacados = todos.AsEnumerable().Take(3);

                    if (destacados.Any())
                    {
                        MedicamentoDataTable = destacados.CopyToDataTable();
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al cargar medicamentos destacados");
            }
        }
    }
}