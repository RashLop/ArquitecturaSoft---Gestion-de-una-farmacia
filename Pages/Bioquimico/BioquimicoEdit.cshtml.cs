using System;
using Microsoft.AspNetCore.Mvc;
using ProyectoArqSoft.Domain.DTOs;
using ProyectoArqSoft.Pages.Base;
using ProyectoArqSoft.Application.Interfaces;
using Result = ProyectoArqSoft.Domain.Validators.Result;
using Microsoft.AspNetCore.Authorization;

namespace ProyectoArqSoft.Pages.Bioquimico
{
    [Authorize(Roles = "Admin")]
    public class BioquimicoEditModel : BasePageModel
    {
        private readonly IUsuarioService usuarioService;

        [BindProperty]
        public UsuarioActualizacionDto Input { get; set; } = new UsuarioActualizacionDto
        {
            Role = "Bioquimico",
            Activo = 1,
            MustChangePassword = 1
        };

        [BindProperty]
        public string CiBase { get; set; } = string.Empty;

        [BindProperty]
        public string CiComplemento { get; set; } = string.Empty;

        public BioquimicoEditModel(IUsuarioService usuarioService)
        {
            this.usuarioService = usuarioService;
        }

        public IActionResult OnGet()
        {
            IActionResult? acceso = ValidarAccesoAdmin();
            if (acceso != null)
                return acceso;

            return Page();
        }

        public IActionResult OnPostCargarBioquimicoParaEdicion(int id)
        {
            UsuarioDto? usuario = usuarioService.ObtenerUsuarioPorId(id);

            if (usuario == null)
                return RedirectToPage("Bioquimico", new { error = "Bioquímico no encontrado" });

            UsuarioDto usuarioNoNull = usuario;
            if (!EsBioquimico(usuarioNoNull.Role))
                return RedirectToPage("Bioquimico", new { error = "Bioquímico no encontrado" });

            string ciCompleto = usuarioNoNull.Ci?.Trim() ?? string.Empty;
            int separador = ciCompleto.IndexOf('-');

            CiBase = separador >= 0 ? ciCompleto[..separador].Trim() : ciCompleto;
            CiComplemento = separador >= 0 ? ciCompleto[(separador + 1)..].Trim() : string.Empty;

            Input.IdUsuario = usuarioNoNull.IdUsuario;
            Input.Nombres = usuarioNoNull.Nombres;
            Input.ApellidoPaterno = usuarioNoNull.ApellidoPaterno;
            Input.ApellidoMaterno = usuarioNoNull.ApellidoMaterno;
            Input.Ci = usuarioNoNull.Ci?.Trim() ?? string.Empty;
            Input.CiExtencion = usuarioNoNull.CiExtencion?.Trim() ?? string.Empty;
            Input.Telefono = usuarioNoNull.Telefono;
            Input.Email = usuarioNoNull.Email;
            Input.UserName = usuarioNoNull.UserName;
            Input.Role = "Bioquimico";
            Input.Activo = usuarioNoNull.Activo;
            Input.MustChangePassword = usuarioNoNull.MustChangePassword;

            return Page();
        }

        public IActionResult OnPostActualizarBioquimico()
        {
            int? idSession = HttpContext.Session.GetInt32("IdUsuario");

            UsuarioDto? usuarioActual = usuarioService.ObtenerUsuarioPorId(Input.IdUsuario);

            if (usuarioActual == null || !EsBioquimico(usuarioActual.Role))
                return RedirectToPage("Bioquimico", new { error = "Bioquímico no encontrado o rol inválido" });

            Input.Role = "Bioquimico";

            Result resultado = usuarioService.ActualizarUsuario(Input, idSession);

            if (resultado.IsSuccess == false)
            {
                Estado.MensajeError = resultado.Error;
                return Page();
            }

            return RedirectToPage("Bioquimico", new { mensaje = "Bioquímico actualizado correctamente" });
        }

        private static bool EsBioquimico(string? role)
        {
            return string.Equals(role?.Trim(), "Bioquimico", StringComparison.OrdinalIgnoreCase);
        }
    }
}


