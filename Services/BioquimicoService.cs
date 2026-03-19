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

        public DataTable ObtenerTodos(string filtro) => _repository.GetAll(filtro);

        public Bioquimico? ObtenerPorId(int id) => _repository.GetById(id);

        public Validacion Crear(Bioquimico bioquimico)
{
    LimpiarDatos(bioquimico);

    
    var res = _validador.Validar(bioquimico);
    if (!res.EsValido) return res;

   
    var repo = (Repositories.BioquimicoRepository)_repository; 
    DataTable dtExiste = repo.GetByDocumento(bioquimico.Ci, bioquimico.CiExtencion);

    if (dtExiste.Rows.Count > 0)
    {
        return new Validacion(false, "Ya existe un bioquímico registrado con ese número de carnet y extensión.");
    }

    
    if (_repository.Insert(bioquimico) <= 0)
        return new Validacion(false, "No se pudo completar el registro en la base de datos.");

    return new Validacion(true);
}

public Validacion Actualizar(Bioquimico bioquimico)
{
    LimpiarDatos(bioquimico);

    
    var res = _validador.Validar(bioquimico);
    if (!res.EsValido) return res;

   
    var repo = (ProyectoArqSoft.Repositories.BioquimicoRepository)_repository;
    
    
    DataTable dtExiste = repo.GetByDocumento(bioquimico.Ci, bioquimico.CiExtencion);

    if (dtExiste.Rows.Count > 0)
    {
        
        int idEncontrado = Convert.ToInt32(dtExiste.Rows[0]["idBioquimico"]);

        
        if (idEncontrado != bioquimico.IdBioquimico)
        {
            return new Validacion(false, "No se puede actualizar: El número de carnet ya pertenece a otro bioquímico.");
        }
    }

   
    if (_repository.Update(bioquimico) <= 0)
        return new Validacion(false, "No se realizaron cambios en el registro o error en la BD.");

    return new Validacion(true);
}

        public Validacion Eliminar(int id)
        {
            
            var entidad = new Bioquimico { IdBioquimico = id };
            if (_repository.Delete(entidad) <= 0)
                return new Validacion(false, "No se pudo eliminar el registro.");

            return new Validacion(true);
        }

        private void LimpiarDatos(Bioquimico b)
        {
            b.Nombres = StringHelper.LimpiarTexto(b.Nombres);
            b.ApellidoPaterno = StringHelper.LimpiarTexto(b.ApellidoPaterno);
            b.ApellidoMaterno = StringHelper.LimpiarTexto(b.ApellidoMaterno);

            b.Ci = StringHelper.LimpiarCI(b.Ci);
            b.CiExtencion = StringHelper.LimpiarTextoMayus(b.CiExtencion);
        }
    }
}