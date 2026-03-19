using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ProyectoArqSoft.Services;
using ProyectoArqSoft.Validaciones;

namespace ProyectoArqSoft.Pages
{
    public class ClienteEditModel : PageModel
    {
        private readonly IClienteService _clienteService;

        [BindProperty]
        public int IdCliente { get; set; }

        [BindProperty]
        public string Nit { get; set; }

        [BindProperty]
        public string RazonSocial { get; set; }

        [BindProperty]
        public string? CorreoElectronico { get; set; }

        [BindProperty]
        public DateTime FechaRegistro { get; set; }

        public ClienteEditModel(IClienteService clienteService)
        {
            _clienteService = clienteService;
        }

        public IActionResult OnGet(int id)
        {
            var cliente = _clienteService.ObtenerPorId(id);

            if (cliente == null)
            {
                TempData["ErrorMessage"] = "Cliente no encontrado";
                return RedirectToPage("/Cliente/Cliente");
            }

            IdCliente = cliente.IdCliente;
            Nit = cliente.Nit;
            RazonSocial = cliente.RazonSocial;
            CorreoElectronico = cliente.CorreoElectronico;
            FechaRegistro = cliente.FechaRegistro;

            return Page();
        }

        public IActionResult OnPost()
        {
            Validacion resultado = _clienteService.Actualizar(IdCliente, Nit, RazonSocial, CorreoElectronico, FechaRegistro);

            if (!resultado.EsValido)
            {
                TempData["ErrorMessage"] = resultado.MensajeError;
                return Page();
            }

            TempData["SuccessMessage"] = resultado.MensajeError ?? "Cliente actualizado exitosamente";
            return RedirectToPage("/Cliente/Cliente");
        }
    }
}