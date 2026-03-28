using ProyectoArqSoft.FactoryProducts;
using ProyectoArqSoft.Helpers;
using ProyectoArqSoft.Models;
using ProyectoArqSoft.Validaciones;
using System.Data;

namespace ProyectoArqSoft.Services
{
    public class MedicamentoService : IMedicamentoService
    {
        private readonly IRepository<Medicamento> _repository;
        private readonly IValidacion<Medicamento> _validador;

        public MedicamentoService(
            IRepository<Medicamento> repository,
            IValidacion<Medicamento> validador)
        {
            _repository = repository;
            _validador = validador;
        }

        public DataTable ObtenerTodos()
        {
            return _repository.GetAll();
        }

        public DataTable ObtenerTodos(string filtro)
        {
            return _repository.GetAll(filtro);
        }

        public Medicamento? ObtenerPorId(int id)
        {
            return _repository.GetById(id);
        }

        public Validacion Crear(
            string nombre,
            string presentacion,
            string clasificacion,
            string concentracion,
            decimal precio,
            int stock)
        {
            Medicamento medicamento = ConstruirMedicamento(
                0,
                nombre,
                presentacion,
                clasificacion,
                concentracion,
                precio,
                stock);

            var validacion = _validador.Validar(medicamento);
            if (validacion.IsFailure)
                return validacion;

            if (_repository.Insert(medicamento) <= 0)
                return Validacion.Fail("No se pudo registrar el medicamento.");

            return Validacion.Ok();
        }

        public Validacion Actualizar(
            int id,
            string nombre,
            string presentacion,
            string clasificacion,
            string concentracion,
            decimal precio,
            int stock)
        {
            Medicamento medicamento = ConstruirMedicamento(
                id,
                nombre,
                presentacion,
                clasificacion,
                concentracion,
                precio,
                stock);

            var validacion = _validador.Validar(medicamento);
            if (validacion.IsFailure)
                return validacion;

            if (_repository.Update(medicamento) <= 0)
                return Validacion.Fail("No se pudo actualizar el medicamento.");

            return Validacion.Ok();
        }

        public Validacion EliminarLogicamente(int id)
        {
            Medicamento medicamento = new Medicamento
            {
                Id = id
            };

            if (_repository.Delete(medicamento) <= 0)
                return Validacion.Fail("No se pudo eliminar el medicamento.");

            return Validacion.Ok();
        }

        private Medicamento ConstruirMedicamento(
            int id,
            string nombre,
            string presentacion,
            string clasificacion,
            string concentracion,
            decimal precio,
            int stock)
        {
            Medicamento medicamento = new Medicamento
            {
                Id = id,
                Nombre = nombre,
                Presentacion = presentacion,
                Clasificacion = clasificacion,
                Concentracion = concentracion,
                Precio = precio,
                Stock = stock
            };

            LimpiarCampos(medicamento);

            return medicamento;
        }

        private static void LimpiarCampos(Medicamento medicamento)
        {
            medicamento.Nombre = StringHelper.LimpiarEspacios(medicamento.Nombre);
            medicamento.Presentacion = StringHelper.LimpiarEspacios(medicamento.Presentacion);
            medicamento.Clasificacion = StringHelper.LimpiarEspacios(medicamento.Clasificacion);
            medicamento.Concentracion = StringHelper.LimpiarEspacios(medicamento.Concentracion);
        }
    }
}