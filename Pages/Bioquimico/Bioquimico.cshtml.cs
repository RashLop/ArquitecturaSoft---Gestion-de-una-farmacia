using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MySql.Data.MySqlClient;
using System.Data;

namespace ProyectoArqSoft.Pages
{
    public class BioquimicoModel : PageModel
    {
        public DataTable BioquimicoDataTable { get; set; } = new DataTable();
        public string FiltroActual { get; set; } = string.Empty;
        public string Mensaje { get; set; } = string.Empty;
        public string MensajeError { get; set; } = string.Empty;

        private readonly IConfiguration _configuration;

        public BioquimicoModel(IConfiguration configuration) => _configuration = configuration;

        public void OnGet(string? filtro, string? mensaje, string? error)
{
    FiltroActual = filtro ?? string.Empty;
    Mensaje = mensaje ?? string.Empty;
    MensajeError = error ?? string.Empty;

    if (!string.IsNullOrWhiteSpace(FiltroActual))
    {
        
        if (FiltroActual.Length < 3 || FiltroActual.Any(c => !char.IsLetterOrDigit(c) && c != ' '))
        {
            MensajeError = "Criterio inválido";
            return;
        }
    }

    Select(FiltroActual);
}

        private void Select(string filtro)
{
    string connectionString = _configuration.GetConnectionString("MySqlConnection")!;
    bool hayFiltro = !string.IsNullOrWhiteSpace(filtro);
    
   
    bool esCi = hayFiltro && filtro.All(char.IsDigit);

    string query = @"SELECT idBioquimico, nombres, apellidos, ci, telefono, activo 
                     FROM bioquimico WHERE activo = 1";

    if (hayFiltro)
    {
        if (esCi)
        {
            query += " AND ci = @valor";
        }
        else
        {
            
            string[] partes = filtro.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < partes.Length; i++)
            {
                query += $" AND (nombres LIKE @valor{i} OR apellidos LIKE @valor{i})";
            }
        }
    }
    query += " ORDER BY idBioquimico ASC";

    using (var conn = new MySqlConnection(connectionString))
    {
        conn.Open();
        using (var cmd = new MySqlCommand(query, conn))
        {
            if (hayFiltro)
            {
                if (esCi)
                {
                    cmd.Parameters.AddWithValue("@valor", filtro);
                }
                else
                {
                    string[] partes = filtro.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                    for (int i = 0; i < partes.Length; i++)
                    {
                        cmd.Parameters.AddWithValue($"@valor{i}", "%" + partes[i] + "%");
                    }
                }
            }
            var adapter = new MySqlDataAdapter(cmd);
            adapter.Fill(BioquimicoDataTable);
        }
    }
}

        public IActionResult OnPostDelete(int id)
        {
            string connectionString = _configuration.GetConnectionString("MySqlConnection")!;
            
            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                
                string query = "UPDATE bioquimico SET activo = 0 WHERE idBioquimico = @id";
                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.ExecuteNonQuery();
                }
            }
            return RedirectToPage("/Bioquimico/Bioquimico", new { mensaje = "Bioquímico eliminado correctamente" });
        }
    }
}