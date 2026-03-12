using Microsoft.AspNetCore.Mvc;

namespace ProyectoArqSoft.Interfaces
{
    public interface IUpdateMedicamento
    {
        IActionResult OnPostActualizarMedicamento();
        IActionResult OnPostCargarMedicamentoParaEdicion(int id);
    }
}