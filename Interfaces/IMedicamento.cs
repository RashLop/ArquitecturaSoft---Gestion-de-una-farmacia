using Microsoft.AspNetCore.Mvc;

namespace ProyectoArqSoft.Interfaces
{
    public interface IMedicamento
    {
        void ListarMedicamentos(string? filtro, string? mensaje, string? error);
        IActionResult EliminarMedicamentoLogicamente(int id);
    }
}