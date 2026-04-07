using System;
using Microsoft.AspNetCore.Mvc;
using ProyectoArqSoft.DTO;
using ProyectoArqSoft.Pages.Base;
using ProyectoArqSoft.Services;

namespace ProyectoArqSoft.Pages.Bioquimico
{
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
            IActionResult? acceso = ValidarAccesoAdmin();
            if (acceso != null)
                return acceso;

            UsuarioDto? usuario = usuarioService.ObtenerUsuarioPorId(id);

            if (usuario == null || !string.Equals(usuario.Role, "Bioquimico", StringComparison.OrdinalIgnoreCase))
                return RedirectToPage("Bioquimico", new { error = "Bioquímico no encontrado" });

            Input.IdUsuario = usuario.IdUsuario;
            Input.Nombres = usuario.Nombres;
            Input.ApellidoPaterno = usuario.ApellidoPaterno;
            Input.ApellidoMaterno = usuario.ApellidoMaterno;
            Input.Ci = usuario.Ci;
            Input.CiExtencion = usuario.CiExtencion;
            Input.Telefono = usuario.Telefono;
            Input.Email = usuario.Email;
            Input.UserName = usuario.UserName;
            Input.Role = "Bioquimico";
            Input.Activo = usuario.Activo;
            Input.MustChangePassword = usuario.MustChangePassword;

            return Page();
        }

        public IActionResult OnPostActualizarBioquimico()
        {
            int? idSession = HttpContext.Session.GetInt32("IdUsuario");
            IActionResult? acceso = ValidarAccesoAdmin();
            if (acceso != null)
                return acceso;

            Input.Role = "Bioquimico";

            var resultado = usuarioService.ActualizarUsuario(Input, idSession);

            if (resultado.IsFailure)
            {
                Estado.MensajeError = resultado.Error;
                return Page();
            }

            return RedirectToPage("Bioquimico", new { mensaje = "Bioquímico actualizado correctamente" });
        }
    }
}
