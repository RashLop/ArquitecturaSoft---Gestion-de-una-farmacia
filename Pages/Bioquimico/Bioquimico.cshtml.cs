using System;
using System.Data;
using Microsoft.AspNetCore.Mvc;
using ProyectoArqSoft.Infrastructure.Helpers;
using ProyectoArqSoft.Pages.Base;
using ProyectoArqSoft.Application.Interfaces;
using ProyectoArqSoft.Domain.Validators;

namespace ProyectoArqSoft.Pages.Bioquimico
{
    public class BioquimicoModel : BasePageModel
    {
        private const string RolBioquimico = "Bioquimico";
        private readonly IUsuarioService usuarioService;

        public DataTable BioquimicoDataTable { get; set; } = new DataTable();

        public BioquimicoModel(IUsuarioService usuarioService)
        {
            this.usuarioService = usuarioService;
        }

        public IActionResult OnGet(string? filtro, string? mensaje, string? error)
        {
            IActionResult? acceso = ValidarAccesoAdmin();
            if (acceso != null)
                return acceso;

            CargarParametros(filtro, mensaje, error);

            Result resultado = FiltroHelper.ValidarFiltro(Estado.FiltroActual);
            Estado.MensajeError = resultado.Error;

            if (resultado.IsFailure)
                return Page();

            CargarBioquimicos(Estado.FiltroActual);
            return Page();
        }

        public IActionResult OnPostEliminarBioquimicoLogicamente(int id)
        {
            int? idSesion = HttpContext.Session.GetInt32("IdUsuario");
            IActionResult? acceso = ValidarAccesoAdmin();
            if (acceso != null)
                return acceso;

            Result resultado = usuarioService.EliminarUsuario(id, idSesion);

            if (resultado.IsFailure)
            {
                Estado.MensajeError = resultado.Error;
                CargarBioquimicos(Estado.FiltroActual);
                return Page();
            }

            return RedirectToPage("Bioquimico", new { mensaje = "Bioquímico dado de baja correctamente" });
        }

        private void CargarParametros(string? filtro, string? mensaje, string? error)
        {
            Estado.FiltroActual = FiltroHelper.LimpiarFiltro(filtro);
            Estado.Mensaje = mensaje ?? string.Empty;
            Estado.MensajeError = error ?? string.Empty;
        }

        private void CargarBioquimicos(string filtro)
        {
            DataTable tablaUsuarios = usuarioService.ObtenerTodos(filtro);
            DataTable tablaFiltrada = tablaUsuarios.Clone();

            foreach (DataRow row in tablaUsuarios.Rows)
            {
                string role = row["role"]?.ToString() ?? string.Empty;
                if (string.Equals(role, RolBioquimico, StringComparison.OrdinalIgnoreCase))
                {
                    tablaFiltrada.ImportRow(row);
                }
            }

            BioquimicoDataTable = tablaFiltrada;
        }
    }
}
