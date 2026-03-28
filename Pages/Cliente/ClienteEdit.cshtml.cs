using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using ProyectoArqSoft.Pages.Base;
using ProyectoArqSoft.Services;
using ProyectoArqSoft.Validaciones;
using ClienteEntidad = ProyectoArqSoft.Models.Cliente;

namespace ProyectoArqSoft.Pages
{
    public class ClienteEditModel : BasePageModel
    {
        private readonly IClienteService clienteService;

        [BindProperty]
        public bool EsConsumidorFinal { get; set; }

        [BindProperty]
        public int IdCliente { get; set; }

        [BindProperty]
        [Display(Name = "NIT")]
        public string Nit { get; set; } = string.Empty;

        [BindProperty]
        [Display(Name = "Razón Social")]
        public string RazonSocial { get; set; } = string.Empty;

        [BindProperty]
        [Display(Name = "Correo Electrónico")]
        public string CorreoElectronico { get; set; } = string.Empty;

        public ClienteEditModel(IClienteService clienteService)
        {
            this.clienteService = clienteService;
        }

        public void OnGet()
        {
        }

        public IActionResult OnPostCargarClienteParaEdicion(int id)
        {
            ClienteEntidad? cliente = clienteService.ObtenerPorId(id);

            if (cliente == null)
                return RedirectToPage("Cliente", new { error = "Cliente no encontrado" });

            IdCliente = cliente.IdCliente;
            Nit = cliente.Nit;
            RazonSocial = cliente.RazonSocial;
            CorreoElectronico = cliente.CorreoElectronico;
            EsConsumidorFinal =
                cliente.Nit.Equals("CF", StringComparison.OrdinalIgnoreCase) &&
                cliente.RazonSocial.Equals("Consumidor Final", StringComparison.OrdinalIgnoreCase);

            return Page();
        }

        public IActionResult OnPostActualizarCliente()
        {
            Validacion resultado = clienteService.Actualizar(
                IdCliente,
                EsConsumidorFinal,
                Nit,
                RazonSocial,
                CorreoElectronico);

            if (resultado.IsFailure)
            {
                Estado.MensajeError = resultado.Error;
                return Page();
            }

            return RedirectToPage("Cliente", new { mensaje = "Cliente actualizado correctamente" });
        }
    }
}