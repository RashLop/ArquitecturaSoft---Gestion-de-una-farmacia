using Microsoft.AspNetCore.Mvc.RazorPages;
using ProyectoArqSoft.Pages.EstadoPaginas;
namespace ProyectoArqSoft.Pages.Base
{
    public abstract class BasePageModel : PageModel
    {
        public EstadoPagina Estado { get; set; } = new EstadoPagina();
    }
}