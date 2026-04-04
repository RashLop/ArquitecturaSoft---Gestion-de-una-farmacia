//namespace ProyectoArqSoft.Domain.Model
namespace ProyectoArqSoft.Models 

{
    public class Clasificacion
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;

        public short Estado { get; set; } = 1;
        public DateTime FechaRegistro { get; set; }
        public DateTime? UltimaActualizacion { get; set; }

        public Clasificacion()
        {
        }

        public Clasificacion(string nombre)
        {   
            Nombre = nombre;
        }
    }
}