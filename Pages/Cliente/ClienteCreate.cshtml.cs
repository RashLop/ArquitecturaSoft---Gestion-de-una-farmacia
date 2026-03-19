using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ProyectoArqSoft.Services;
using ProyectoArqSoft.Validaciones;

namespace ProyectoArqSoft.Pages
{
    public class ClienteCreateModel : PageModel
    {
        private readonly IClienteService _clienteService;

        [BindProperty]
        public string Nit { get; set; }

        [BindProperty]
        public string RazonSocial { get; set; }

        [BindProperty]
        public string? CorreoElectronico { get; set; }

        [BindProperty]
        public DateTime FechaRegistro { get; set; }

        public ClienteCreateModel(IClienteService clienteService)
        {
            _clienteService = clienteService;
        }

        public void OnGet()
        {
            // Valor por defecto: fecha actual
            FechaRegistro = DateTime.Today;
        }

        public IActionResult OnPost()
        {
            Validacion resultado = _clienteService.Crear(Nit, RazonSocial, CorreoElectronico, FechaRegistro);

            if (!resultado.EsValido)
            {
                TempData["ErrorMessage"] = resultado.MensajeError;
                return Page();
            }

            TempData["SuccessMessage"] = resultado.MensajeError ?? "Cliente creado exitosamente";
            return RedirectToPage("/Cliente/Cliente");
        }
    }
}