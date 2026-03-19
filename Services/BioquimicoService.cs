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
            // Usamos tu validador de formulario existente
            _validador = new BioquimicoFormularioValidacion();
            
        }

        public DataTable ObtenerTodos(string filtro) => _repository.GetAll(filtro);

        public Bioquimico? ObtenerPorId(int id) => _repository.GetById(id);

        public Validacion Crear(Bioquimico bioquimico)
        {
            LimpiarDatos(bioquimico);

            // 1. Validación de reglas de negocio (nombres vacíos, etc.)
            var res = _validador.Validar(bioquimico);
            if (!res.EsValido) return res;

            // 2. Validación de duplicados usando tu método GetAll(filtro)
            // Filtramos por CI exacto para ver si ya existe
            var existe = _repository.GetAll(bioquimico.Ci);
            if (existe.Rows.Count > 0)
            {
                // Verificación extra por si el filtro trajo similares pero no idénticos
                foreach (DataRow row in existe.Rows)
                {
                    if (row["ci"].ToString() == bioquimico.Ci && 
                        row["ci_extencion"].ToString() == bioquimico.CiExtencion)
                    {
                        return new Validacion(false, "Ya existe un bioquímico con ese CI y extensión.");
                    }
                }
            }

            // 3. Persistencia
            if (_repository.Insert(bioquimico) <= 0)
                return new Validacion(false, "No se pudo completar el registro en la base de datos.");

            return new Validacion(true);
        }

        public Validacion Actualizar(Bioquimico bioquimico)
        {
            LimpiarDatos(bioquimico);

            var res = _validador.Validar(bioquimico);
            if (!res.EsValido) return res;

            if (_repository.Update(bioquimico) <= 0)
                return new Validacion(false, "Error al actualizar los datos.");

            return new Validacion(true);
        }

        public Validacion Eliminar(int id)
        {
            // Creamos un objeto temporal con el ID para el borrado lógico
            var entidad = new Bioquimico { IdBioquimico = id };
            if (_repository.Delete(entidad) <= 0)
                return new Validacion(false, "No se pudo eliminar el registro.");

            return new Validacion(true);
        }

        private void LimpiarDatos(Bioquimico b)
{
    // Solo hace el Trim/ToUpper si el campo NO es nulo
    if (b.Nombres != null) b.Nombres = b.Nombres.Trim();
    if (b.ApellidoPaterno != null) b.ApellidoPaterno = b.ApellidoPaterno.Trim();
    if (b.ApellidoMaterno != null) b.ApellidoMaterno = b.ApellidoMaterno.Trim();
    if (b.Ci != null) b.Ci = b.Ci.Trim();
    
    if (b.CiExtencion != null) 
        b.CiExtencion = b.CiExtencion.Trim().ToUpper();
}
    }
}