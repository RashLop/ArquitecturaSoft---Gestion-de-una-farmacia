using ProyectoArqSoft.FactoryProducts;
using ProyectoArqSoft.Helpers;
using ProyectoArqSoft.Domain.Models;
using ProyectoArqSoft.Domain.Validators;
using System.Data;

namespace ProyectoArqSoft.Services
{
    public class MedicamentoService : IMedicamentoService
    {
        private readonly IRepository<Medicamento> _repository;
        private readonly IResult<Medicamento> _validador;

        public MedicamentoService(
            IRepository<Medicamento> repository,
            IResult<Medicamento> validador)
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

        public Result Crear(
            string nombre,
            string presentacion,
            int idClasificacion,
            string concentracion,
            decimal precio,
            int stock)
        {
            Medicamento medicamento = ConstruirMedicamento(
                0,
                nombre,
                presentacion,
                idClasificacion,
                concentracion,
                precio,
                stock);

            var validacion = _validador.Validar(medicamento);
            if (validacion.IsFailure)
                return validacion;

            if (_repository.Insert(medicamento) <= 0)
                return Result.Fail("No se pudo registrar el medicamento.");

            return Result.Ok();
        }

        public Result Actualizar(
            int id,
            string nombre,
            string presentacion,
            int idClasificacion,
            string concentracion,
            decimal precio,
            int stock)
        {
            Medicamento medicamento = ConstruirMedicamento(
                id,
                nombre,
                presentacion,
                idClasificacion,
                concentracion,
                precio,
                stock);

            var validacion = _validador.Validar(medicamento);
            if (validacion.IsFailure)
                return validacion;

            if (_repository.Update(medicamento) <= 0)
                return Result.Fail("No se pudo actualizar el medicamento.");

            return Result.Ok();
        }

        public Result EliminarLogicamente(int id)
        {
            Medicamento medicamento = new Medicamento
            {
                Id = id
            };

            if (_repository.Delete(medicamento) <= 0)
                return Result.Fail("No se pudo eliminar el medicamento.");

            return Result.Ok();
        }

        private Medicamento ConstruirMedicamento(
            int id,
            string nombre,
            string presentacion,
            int idClasificacion,
            string concentracion,
            decimal precio,
            int stock)
        {
            Medicamento medicamento = new Medicamento
            {
                Id = id,
                Nombre = nombre,
                Presentacion = presentacion,
                IdClasificacion = idClasificacion,
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
            medicamento.Concentracion = StringHelper.LimpiarEspacios(medicamento.Concentracion);
        }
    }
}