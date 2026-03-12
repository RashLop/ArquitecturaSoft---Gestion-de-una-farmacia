using Microsoft.AspNetCore.Mvc;

namespace ProyectoArqSoft.Interfaces
{
    public interface IMedicamento
    {
        void OnGet(string? filtro, string? mensaje, string? error);
        IActionResult OnPostEliminarMedicamentoLogicamente(int id);
    }
}