using Microsoft.AspNetCore.Mvc;
using ProyectoArqSoft.Pages.Base;
using ProyectoArqSoft.Services;
using ProyectoArqSoft.DTO;
using ProyectoArqSoft.Validaciones; // Aquí debe estar la clase Result

namespace ProyectoArqSoft.Pages.Usuario
{
    public class UsuarioCreateModel : BasePageModel
    {
        private readonly IUsuarioService usuarioService;

        [BindProperty] public string nombres { get; set; } = string.Empty;
        [BindProperty] public string apPaterno { get; set; } = string.Empty;
        [BindProperty] public string apMaterno { get; set; } = string.Empty;
        [BindProperty] public string ci { get; set; } = string.Empty;
        [BindProperty] public string ciExtencion { get; set; } = string.Empty;
        [BindProperty] public string telefono { get; set; } = string.Empty;
        [BindProperty] public string email { get; set; } = string.Empty;
        [BindProperty] public string role { get; set; } = "Bioquimico";

        public UsuarioCreateModel(IUsuarioService usuarioService) => this.usuarioService = usuarioService;

        public IActionResult OnPostCrearUsuario()
        {
            // 1. PROTOCOLO DE GENERACIÓN DE USERNAME (nombre.apellido)
            string primerNombre = nombres.Trim().Split(' ')[0].ToLower();
            string primerApellido = apPaterno.Trim().ToLower();
            string userNameGenerado = $"{primerNombre}.{primerApellido}";

            // 2. GENERACIÓN DE CONTRASEÑA ALEATORIA (Seguridad exigida por rúbrica)
            string passAleatorio = Guid.NewGuid().ToString().Substring(0, 8);

            var dto = new UsuarioRegistroDto
            {
                Nombres = nombres,
                ApellidoPaterno = apPaterno,
                ApellidoMaterno = apMaterno,
                Ci = ci,
                CiExtencion = ciExtencion,
                Telefono = telefono,
                Email = email,
                UserName = userNameGenerado,
                Password = passAleatorio
            };

            // USAMOS 'Result' QUE ES EL TIPO REAL EN TU PROYECTO
            Result resultado = usuarioService.CrearUsuario(dto, role);

            if (resultado.IsFailure)
            {
                Estado.MensajeError = resultado.Error;
                return Page();
            }

            return RedirectToPage("Usuario", new { mensaje = $"Usuario registrado. Username: {userNameGenerado}. Credenciales enviadas por mail." });
        }
    }
}