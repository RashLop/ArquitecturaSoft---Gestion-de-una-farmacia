//using ProyectoArqSoft.Application.Interfaces;
using ProyectoArqSoft.Services;
//using ProyectoArqSoft.Application.Ports.Output;
using ProyectoArqSoft.FactoryProducts;
//using ProyectoArqSoft.Domain.Model.Clasificacion;
using ProyectoArqSoft.Models;
//using ProyectoArqSoft.Domain.Validators;
using ProyectoArqSoft.Validaciones;
using ProyectoArqSoft.Helpers;
using System.Data;

//namespace ProyectoArqSoft.Application.Services
namespace ProyectoArqSoft.Services

{
    public class ClasificacionService : IClasificacionService
    {
        private readonly IClasificacionRepository _repository;
        private readonly IResult<Clasificacion> _validador;

        public ClasificacionService(
            IClasificacionRepository repository,
            IResult<Clasificacion> validador)
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

        public Clasificacion? ObtenerPorId(int id)
        {
            return _repository.GetById(id);
        }

        public Result Crear(string nombre)
        {
            Clasificacion clasificacion = ConstruirClasificacion(0, nombre);

            Result validacion = _validador.Validar(clasificacion);
            if (validacion.IsFailure)
                return validacion;
            
            if (_repository.ExisteNombreActivo(clasificacion.Nombre))
                return Result.Fail("Ya existe una clasificación activa con ese nombre.");

            if (_repository.Insert(clasificacion) <= 0)
                return Result.Fail("No se pudo registrar la clasificación.");

            return Result.Ok();
        }

        public Result Actualizar(int id, string nombre)
        {
            Clasificacion clasificacion = ConstruirClasificacion(id, nombre);

            Result validacion = _validador.Validar(clasificacion);
            if (validacion.IsFailure)
                return validacion;

            if (_repository.ExisteNombreActivoExcluyendoId(id, clasificacion.Nombre))
                return Result.Fail("Ya existe otra clasificación activa con ese nombre.");

            if (_repository.Update(clasificacion) <= 0)
                return Result.Fail("No se pudo actualizar la clasificación.");

            return Result.Ok();
        }

        public Result EliminarLogicamente(int id)
        {
            if (_repository.TieneMedicamentosActivosAsociados(id))
                return Result.Fail("No se puede eliminar la clasificación porque tiene medicamentos activos asociados.");

            Clasificacion clasificacion = new Clasificacion
            {
                Id = id
            };

            if (_repository.Delete(clasificacion) <= 0)
                return Result.Fail("No se pudo eliminar la clasificación.");

            return Result.Ok();
        }

        private Clasificacion ConstruirClasificacion(int id, string nombre)
        {
            return new Clasificacion
            {
                Id = id,
                Nombre = StringHelper.LimpiarEspacios(nombre)
            };
        }
    }
}