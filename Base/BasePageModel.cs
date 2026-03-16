using Microsoft.AspNetCore.Mvc.RazorPages;
namespace ProyectoArqSoft.Base
{
    public abstract class BasePageModel : PageModel
    {
        public EstadoPagina Estado { get; set; } = new EstadoPagina();
    }
}