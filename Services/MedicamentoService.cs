using ProyectoArqSoft.FactoryProducts;
using ProyectoArqSoft.Helpers;
using ProyectoArqSoft.Models;
using ProyectoArqSoft.Validaciones;
using System.Data;

namespace ProyectoArqSoft.Services
{
    public class MedicamentoService : IMedicamentoService
    {
        private readonly IRepository<Medicamento> repository;
        private readonly IValidacion<Medicamento> validador;

        public MedicamentoService(
            IRepository<Medicamento> repository,
            IValidacion<Medicamento> validador)
        {
            this.repository = repository;
            this.validador = validador;
        }

        public DataTable ObtenerTodos()
        {
            return repository.GetAll();
        }

        public DataTable ObtenerTodos(string filtro)
        {
            return repository.GetAll(filtro);
        }

        public Medicamento? ObtenerPorId(int id)
        {
            return repository.GetById(id);
        }

        public Validacion Crear(
            string nombre,
            string presentacion,
            string clasificacion,
            string concentracion,
            decimal precio,
            int stock)
        {
            Medicamento medicamento = new Medicamento
            {
                Nombre = nombre,
                Presentacion = presentacion,
                Clasificacion = clasificacion,
                Concentracion = concentracion,
                Precio = precio,
                Stock = stock
            };

            LimpiarCampos(medicamento);

            Validacion resultado = validador.Validar(medicamento);
            if (!resultado.EsValido)
                return resultado;

            int filasAfectadas = repository.Insert(medicamento);

            if (filasAfectadas <= 0)
                return new Validacion(false, "No se pudo registrar el medicamento.");

            return new Validacion(true);
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

            Validacion resultado = validador.Validar(medicamento);
            if (!resultado.EsValido)
                return resultado;

            int filasAfectadas = repository.Update(medicamento);

            if (filasAfectadas <= 0)
                return new Validacion(false, "No se pudo actualizar el medicamento.");

            return new Validacion(true);
        }

        public Validacion EliminarLogicamente(int id)
        {
            Medicamento medicamento = new Medicamento
            {
                Id = id
            };

            int filasAfectadas = repository.Delete(medicamento);

            if (filasAfectadas <= 0)
                return new Validacion(false, "No se pudo eliminar el medicamento.");

            return new Validacion(true);
        }

        private void LimpiarCampos(Medicamento medicamento)
        {
            medicamento.Nombre = StringHelper.QuitarEspacios(medicamento.Nombre);
            medicamento.Presentacion = StringHelper.LimpiarEspacios(medicamento.Presentacion);
            medicamento.Clasificacion = StringHelper.LimpiarEspacios(medicamento.Clasificacion);
            medicamento.Concentracion = StringHelper.LimpiarEspacios(medicamento.Concentracion);
        }
    }
}