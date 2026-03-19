namespace ProyectoArqSoft.Models
{
    public class Cliente
    {
        public int IdCliente { get; set; }
        public string Nit { get; set; } = string.Empty;
        public string RazonSocial { get; set; } = string.Empty;
        public string? CorreoElectronico { get; set; }
        public DateTime FechaRegistro { get; set; }
        public DateTime? UltimaActualizacion { get; set; }

        public Cliente() { }

        public Cliente(string nit, string razonSocial, string? correoElectronico, DateTime? fechaRegistro = null)
        {
            Nit = nit;
            RazonSocial = razonSocial;
            CorreoElectronico = correoElectronico;
            FechaRegistro = fechaRegistro ?? DateTime.Now;
        }
    }
}