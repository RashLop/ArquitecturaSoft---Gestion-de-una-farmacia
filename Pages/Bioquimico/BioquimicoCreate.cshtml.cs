using Microsoft.AspNetCore.Mvc;
using ProyectoArqSoft.Application.Interfaces;
using ProyectoArqSoft.Domain.DTOs;
using ProyectoArqSoft.Infrastructure.Helpers;
using ProyectoArqSoft.Domain.Validators;
using ProyectoArqSoft.Pages.Base;

namespace ProyectoArqSoft.Pages.Bioquimico
{
    public class BioquimicoCreateModel : BasePageModel
    {
        private readonly IUsuarioService _usuarioService;

        [BindProperty]
        public UsuarioRegistroDto Registro { get; set; } = new();

        [BindProperty]
        public string CiComplemento { get; set; } = string.Empty;

        public string MensajeError { get; set; } = string.Empty;
        public string MensajeOk { get; set; } = string.Empty;

        public BioquimicoCreateModel(IUsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        public IActionResult OnGet()
        {
            IActionResult? acceso = ValidarAccesoAdmin();
            if (acceso != null)
                return acceso;

            return Page();
        }

        public IActionResult OnPost()
        {
            IActionResult? acceso = ValidarAccesoAdmin();
            if (acceso != null)
                return acceso;

            string role = "Bioquimico";

            Registro.Ci = (Registro.Ci ?? string.Empty).Trim();
            CiComplemento = (CiComplemento ?? string.Empty).Trim().ToUpper();

            if (!string.IsNullOrWhiteSpace(CiComplemento))
            {
                Registro.Ci = $"{Registro.Ci}-{CiComplemento}";
            }

            Registro.UserName = CredencialesHelper.GenerarUserName(
                Registro.Nombres,
                Registro.ApellidoPaterno,
                Registro.Ci
            );

            Registro.Password = CredencialesHelper.GenerarPasswordTemporal();

            ModelState.Remove("Registro.UserName");
            ModelState.Remove("Registro.Password");

            Result resultado = _usuarioService.CrearUsuario(Registro, role);

            if (!resultado.IsSuccess)
            {
                MensajeError = resultado.Error;
                return Page();
            }

            MensajeOk = "Usuario registrado correctamente. Revisa las credenciales generadas y tu correo electrónico.";
            ModelState.Clear();
            Registro = new UsuarioRegistroDto();
            CiComplemento = string.Empty;

            return Page();
        }
    }
}