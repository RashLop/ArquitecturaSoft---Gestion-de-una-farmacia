using ProyectoArqSoft.FactoryProducts;
using ProyectoArqSoft.Helpers;
using ProyectoArqSoft.Models;
using ProyectoArqSoft.Validaciones;
using System.Data;

namespace ProyectoArqSoft.Services
{
    public class ClienteService : IClienteService
    {
        private readonly IRepository<Cliente> _repository;
        private readonly IValidacion<Cliente> _validador;
        private readonly ILogger<ClienteService> _logger;

        public ClienteService(
            IRepository<Cliente> repository,
            IValidacion<Cliente> validador,
            ILogger<ClienteService> logger)
        {
            _repository = repository;
            _validador = validador;
            _logger = logger;
        }

        public DataTable ObtenerTodos(string? filtro = null)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(filtro))
                    return _repository.GetAll();

                return _repository.GetAll(filtro);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en ObtenerTodos");
                throw;
            }
        }

        public Cliente? ObtenerPorId(int id)
        {
            try
            {
                return _repository.GetById(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener cliente por ID {Id}", id);
                throw;
            }
        }

        public Validacion Crear(string nit, string razonSocial, string? correoElectronico, DateTime fechaRegistro)
        {
            _logger.LogInformation("Creando nuevo cliente: {RazonSocial}", razonSocial);

            Cliente cliente = new Cliente
            {
                Nit = StringHelper.QuitarEspacios(nit),
                RazonSocial = StringHelper.LimpiarEspacios(razonSocial),
                CorreoElectronico = string.IsNullOrWhiteSpace(correoElectronico) ? null : StringHelper.QuitarEspacios(correoElectronico),
                FechaRegistro = fechaRegistro
            };

            Validacion resultado = _validador.Validar(cliente);
            if (!resultado.EsValido)
                return resultado;

            try
            {
                int filasAfectadas = _repository.Insert(cliente);

                if (filasAfectadas <= 0)
                    return new Validacion(false, "No se pudo registrar el cliente.");

                return new Validacion(true, "Cliente registrado exitosamente.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al insertar cliente");
                return new Validacion(false, $"Error al registrar: {ex.Message}");
            }
        }

        public Validacion Actualizar(int id, string nit, string razonSocial, string? correoElectronico, DateTime fechaRegistro)
        {
            _logger.LogInformation("Actualizando cliente ID: {Id}", id);

            Cliente cliente = new Cliente
            {
                IdCliente = id,
                Nit = StringHelper.QuitarEspacios(nit),
                RazonSocial = StringHelper.LimpiarEspacios(razonSocial),
                CorreoElectronico = string.IsNullOrWhiteSpace(correoElectronico) ? null : StringHelper.QuitarEspacios(correoElectronico),
                FechaRegistro = fechaRegistro
            };

            Validacion resultado = _validador.Validar(cliente);
            if (!resultado.EsValido)
                return resultado;

            try
            {
                int filasAfectadas = _repository.Update(cliente);

                if (filasAfectadas <= 0)
                    return new Validacion(false, "No se pudo actualizar el cliente.");

                return new Validacion(true, "Cliente actualizado exitosamente.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar cliente {Id}", id);
                return new Validacion(false, $"Error al actualizar: {ex.Message}");
            }
        }

        public Validacion EliminarLogicamente(int id)
        {
            try
            {
                Cliente cliente = new Cliente { IdCliente = id };
                int filasAfectadas = _repository.Delete(cliente);

                if (filasAfectadas <= 0)
                    return new Validacion(false, "No se pudo eliminar el cliente.");

                return new Validacion(true, "Cliente eliminado exitosamente.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar cliente {Id}", id);
                return new Validacion(false, $"Error al eliminar: {ex.Message}");
            }
        }
    }
}