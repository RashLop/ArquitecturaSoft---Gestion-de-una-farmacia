using ProyectoArqSoft.Domain.Entities.Medicamento;
using ProyectoArqSoft.Domain.Enums;
using ProyectoArqSoft.Domain.Interfaces;
using ProyectoArqSoft.Application.Factories;

namespace ProyectoArqSoft.Application.Services
{
    public class MedicamentoService
    {
        private readonly IMedicamentoRepository repository;
        private readonly IMedicamentoFactory factory;

        public MedicamentoService(IMedicamentoRepository repository, IMedicamentoFactory factory)
        {
            this.repository = repository;
            this.factory = factory;
        }

        public void CrearMedicamento(Medicamento medicamento)
        {
            Console.WriteLine("Service: Creando medicamento");
            ClasificacionMedicamento tipo =
                Enum.Parse<ClasificacionMedicamento>(
                    medicamento.Clasificacion.Replace("í", "i")
                                             .Replace("é", "e")
                                             .Replace("ó", "o")
                                             .Replace("á", "a")
                );

            Medicamento medicamentoReal = factory.Crear(tipo);

            medicamentoReal.Nombre = medicamento.Nombre;
            medicamentoReal.Presentacion = medicamento.Presentacion;
            medicamentoReal.Clasificacion = medicamento.Clasificacion;
            medicamentoReal.Concentracion = medicamento.Concentracion;
            medicamentoReal.Precio = medicamento.Precio;
            medicamentoReal.Stock = medicamento.Stock;

            repository.Crear(medicamentoReal);
        }


        public void ActualizarMedicamento(Medicamento medicamento)
        {
            repository.Actualizar(medicamento);
        }

        public void EliminarMedicamento(int id)
        {
            repository.EliminarLogico(id);
        }
    }
}