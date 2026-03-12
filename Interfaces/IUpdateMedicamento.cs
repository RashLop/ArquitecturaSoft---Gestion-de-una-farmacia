using Microsoft.AspNetCore.Mvc;

namespace ProyectoArqSoft.Interfaces
{
    public interface IUpdateMedicamento
    {
        IActionResult ActualizarMedicamento();
        IActionResult CargarMedicamentoParaEdicion(int id);
    }
}