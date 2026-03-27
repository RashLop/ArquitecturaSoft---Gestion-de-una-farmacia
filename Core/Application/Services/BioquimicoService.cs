using ProyectoArqSoft.FactoryProducts;
using ProyectoArqSoft.Helpers;
using ProyectoArqSoft.Models;
using ProyectoArqSoft.Validaciones;
using System.Data;

namespace ProyectoArqSoft.Services
{
    public class BioquimicoService : IBioquimicoService
    {
        private readonly IRepository<Bioquimico> _repository;
        private readonly IValidacion<Bioquimico> _validador;

        public BioquimicoService(IRepository<Bioquimico> repository)
        {
            _repository = repository;
            _validador = new BioquimicoFormularioValidacion();
        }

        public DataTable ObtenerTodos(string filtro)
        {
            return _repository.GetAll(filtro);
        }

        public Bioquimico? ObtenerPorId(int id)
        {
            return _repository.GetById(id);
        }

        public Validacion Crear(Bioquimico bioquimico)
        {
            LimpiarDatos(bioquimico);

            var validacion = _validador.Validar(bioquimico);
            if (validacion.IsFailure)
                return validacion;

            var validacionDuplicado = ValidarDocumentoDuplicado(bioquimico);
            if (validacionDuplicado.IsFailure)
                return validacionDuplicado;

            if (_repository.Insert(bioquimico) <= 0)
                return Validacion.Fail("No se pudo completar el registro en la base de datos.");

            return Validacion.Ok();
        }

        public Validacion Actualizar(Bioquimico bioquimico)
        {
            LimpiarDatos(bioquimico);

            var validacion = _validador.Validar(bioquimico);
            if (validacion.IsFailure)
                return validacion;

            var validacionDuplicado = ValidarDocumentoDuplicadoEnActualizacion(bioquimico);
            if (validacionDuplicado.IsFailure)
                return validacionDuplicado;

            if (_repository.Update(bioquimico) <= 0)
                return Validacion.Fail("No se realizaron cambios en el registro o hubo un error en la base de datos.");

            return Validacion.Ok();
        }

        public Validacion Eliminar(int id)
        {
            var entidad = new Bioquimico { IdBioquimico = id };

            if (_repository.Delete(entidad) <= 0)
                return Validacion.Fail("No se pudo eliminar el registro.");

            return Validacion.Ok();
        }

        private Validacion ValidarDocumentoDuplicado(Bioquimico bioquimico)
        {
            var repository = ObtenerRepositorioConcreto();
            DataTable dtExiste = repository.GetByDocumento(bioquimico.Ci, bioquimico.CiExtencion);

            if (dtExiste.Rows.Count > 0)
                return Validacion.Fail("Ya existe un bioquímico registrado con ese número de carnet y extensión.");

            return Validacion.Ok();
        }

        private Validacion ValidarDocumentoDuplicadoEnActualizacion(Bioquimico bioquimico)
        {
            var repository = ObtenerRepositorioConcreto();
            DataTable dtExiste = repository.GetByDocumento(bioquimico.Ci, bioquimico.CiExtencion);

            if (dtExiste.Rows.Count == 0)
                return Validacion.Ok();

            int idEncontrado = Convert.ToInt32(dtExiste.Rows[0]["idBioquimico"]);

            if (idEncontrado != bioquimico.IdBioquimico)
                return Validacion.Fail("No se puede actualizar: el número de carnet ya pertenece a otro bioquímico.");

            return Validacion.Ok();
        }

        private Repositories.BioquimicoRepository ObtenerRepositorioConcreto()
        {
            return (Repositories.BioquimicoRepository)_repository;
        }

        private void LimpiarDatos(Bioquimico bioquimico)
        {
            bioquimico.Nombres = StringHelper.LimpiarTexto(bioquimico.Nombres);
            bioquimico.ApellidoPaterno = StringHelper.LimpiarTexto(bioquimico.ApellidoPaterno);
            bioquimico.ApellidoMaterno = StringHelper.LimpiarTexto(bioquimico.ApellidoMaterno);
            bioquimico.Ci = StringHelper.LimpiarCI(bioquimico.Ci);
            bioquimico.CiExtencion = StringHelper.LimpiarTextoMayus(bioquimico.CiExtencion);
        }
    }
}