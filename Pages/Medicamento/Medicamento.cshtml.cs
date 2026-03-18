using Microsoft.AspNetCore.Mvc;
using ProyectoArqSoft.FactoryCreators;
using ProyectoArqSoft.FactoryProducts;
using ProyectoArqSoft.Helpers;
using ProyectoArqSoft.Pages.Base;
using ProyectoArqSoft.Validaciones;
using MedicamentoEntidad = ProyectoArqSoft.Models.Medicamento;
using System.Data;

namespace ProyectoArqSoft.Pages
{
    public class MedicamentoModel : BasePageModel
    {
        private readonly IRepository<MedicamentoEntidad> repository;

        public DataTable MedicamentoDataTable { get; set; } = new DataTable();

        public MedicamentoModel(IConfiguration configuration)
        {
            RepositoryCreator<MedicamentoEntidad> creator = new MedicamentoRepositoryCreator(configuration);
            repository = creator.CreateRepo();
        }

        public void OnGet(string? filtro, string? mensaje, string? error)
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
            MedicamentoEntidad medicamento = new MedicamentoEntidad { Id = id };
            repository.Delete(medicamento);
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
            MedicamentoDataTable = repository.GetAll(filtro);
        }
    }
}