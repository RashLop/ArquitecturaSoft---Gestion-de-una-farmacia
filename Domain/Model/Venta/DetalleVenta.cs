namespace ProyectoArqSoft.Domain.Models
{
    public class DetalleVenta
    {
        public int Id { get; set; }
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
        public decimal Subtotal { get; set; }
        public int IdVenta { get; set; }
        public int IdMedicamento { get; set; }
    }
}