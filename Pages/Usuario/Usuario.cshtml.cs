using Microsoft.AspNetCore.Mvc;
using ProyectoArqSoft.Services;
using ProyectoArqSoft.Validaciones;
using ProyectoArqSoft.Helpers;
using ProyectoArqSoft.Pages.Base;
using System.Data;
using Microsoft.AspNetCore.Authorization;

namespace ProyectoArqSoft.Pages.Usuarios
{
    [Authorize(Roles = "Admin")]
    public class UsuarioModel : BasePageModel
    {
        private readonly IUsuarioService usuarioService;
        public DataTable UsuarioDataTable { get; set; } = new DataTable();

        public UsuarioModel(IUsuarioService usuarioService)
        {
            this.usuarioService = usuarioService;
        }

        public void OnGet(string? filtro, string? mensaje, string? error)
        {
            Estado.FiltroActual = FiltroHelper.LimpiarFiltro(filtro);
            Estado.Mensaje = mensaje ?? string.Empty;
            Estado.MensajeError = error ?? string.Empty;

            // Select con refresco (filtro)
            UsuarioDataTable = usuarioService.ObtenerTodos(Estado.FiltroActual);
        }

        public IActionResult OnPostEliminarUsuario(int id)
        {
            // Cumple con la rúbrica: "Dar de baja" (Eliminación lógica en el Service)
            Validacion resultado = usuarioService.EliminarUsuario(id);

            if (resultado.IsFailure)
            {
                Estado.MensajeError = resultado.Error;
                UsuarioDataTable = usuarioService.ObtenerTodos(Estado.FiltroActual);
                return Page();
            }

            return RedirectToPage("Usuario", new { mensaje = "El usuario ha sido dado de baja (Desactivado)" });
        }
    }
}