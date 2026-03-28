using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using ProyectoArqSoft.Pages.Base;
using ProyectoArqSoft.Services;
using ProyectoArqSoft.Validaciones;

namespace ProyectoArqSoft.Pages
{
    public class ClienteCreateModel : BasePageModel
    {
        private readonly IClienteService clienteService;

        [BindProperty]
        public bool EsConsumidorFinal { get; set; }

        [BindProperty]
        [Display(Name = "NIT")]
        public string Nit { get; set; } = string.Empty;

        [BindProperty]
        [Display(Name = "Razón Social")]
        public string RazonSocial { get; set; } = string.Empty;

        [BindProperty]
        [Display(Name = "Correo Electrónico")]
        public string CorreoElectronico { get; set; } = string.Empty;

        public ClienteCreateModel(IClienteService clienteService)
        {
            this.clienteService = clienteService;
        }

        public void OnGet()
        {
        }

        public IActionResult OnPostCrearCliente()
        {
            Validacion resultado = clienteService.Crear(
                EsConsumidorFinal,
                Nit,
                RazonSocial,
                CorreoElectronico);

            if (resultado.IsFailure)
            {
                Estado.MensajeError = resultado.Error;
                return Page();
            }

            return RedirectToPage("Cliente", new { mensaje = "Cliente registrado correctamente" });
        }
    }
}