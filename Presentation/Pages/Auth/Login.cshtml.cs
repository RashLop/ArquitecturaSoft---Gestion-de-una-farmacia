using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

public class LoginModel : PageModel
{
    [BindProperty]
    [Required]
    public string Usuario { get; set; } = string.Empty;

    [BindProperty]
    [Required]
    public string Password { get; set; } = string.Empty;

    public IActionResult OnPost()
    {
        if (!ModelState.IsValid)
            return Page();

        if (Usuario == "admin" && Password == "1234")
        {
            HttpContext.Session.SetString("Usuario", Usuario);
            return RedirectToPage("/Index");
        }

        ModelState.AddModelError(string.Empty, "Credenciales incorrectas");
        return Page();
    }
}
