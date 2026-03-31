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

        public ClienteService(
            IRepository<Cliente> repository,
            IValidacion<Cliente> validador)
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

        public Cliente? ObtenerPorId(int id)
        {
            return _repository.GetById(id);
        }

        public Validacion Crear(
            bool esConsumidorFinal,
            string nit,
            string razonSocial,
            string? correoElectronico)
        {
            Cliente cliente = ConstruirCliente(0, esConsumidorFinal, nit, razonSocial, correoElectronico);

            var validacion = _validador.Validar(cliente);
            if (validacion.IsFailure)
                return validacion;

            var validacionDuplicado = ValidarDuplicado(cliente);
            if (validacionDuplicado.IsFailure)
                return validacionDuplicado;

            if (_repository.Insert(cliente) <= 0)
                return Validacion.Fail("No se pudo registrar el cliente.");

            return Validacion.Ok();
        }

        public Validacion Actualizar(
            int id,
            bool esConsumidorFinal,
            string nit,
            string razonSocial,
            string? correoElectronico)
        {
            Cliente cliente = ConstruirCliente(id, esConsumidorFinal, nit, razonSocial, correoElectronico);

            var validacion = _validador.Validar(cliente);
            if (validacion.IsFailure)
                return validacion;

            var validacionDuplicado = ValidarDuplicado(cliente);
            if (validacionDuplicado.IsFailure)
                return validacionDuplicado;

            if (_repository.Update(cliente) <= 0)
                return Validacion.Fail("No se pudo actualizar el cliente.");

            return Validacion.Ok();
        }

        public Validacion Eliminar(int id)
        {
            Cliente cliente = new Cliente
            {
                IdCliente = id
            };

            if (_repository.Delete(cliente) <= 0)
                return Validacion.Fail("No se pudo eliminar el cliente.");

            return Validacion.Ok();
        }

        private Cliente ConstruirCliente(
            int id,
            bool esConsumidorFinal,
            string nit,
            string razonSocial,
            string? correoElectronico)
        {
            Cliente cliente = new Cliente
            {
                IdCliente = id,
                Nit = nit,
                RazonSocial = razonSocial,
                CorreoElectronico = correoElectronico ?? string.Empty
            };

            AplicarConsumidorFinal(cliente, esConsumidorFinal);
            LimpiarCampos(cliente);

            return cliente;
        }

        private static void LimpiarCampos(Cliente cliente)
        {
            cliente.Nit = StringHelper.QuitarEspacios(cliente.Nit);
            cliente.RazonSocial = StringHelper.LimpiarEspacios(cliente.RazonSocial);
            cliente.CorreoElectronico = StringHelper.QuitarEspacios(cliente.CorreoElectronico);
        }

        private Validacion ValidarDuplicado(Cliente cliente)
        {
            if (cliente.Nit.Equals("CF", StringComparison.OrdinalIgnoreCase))
                return Validacion.Ok();

            DataTable clientes = _repository.GetAll(cliente.Nit);

            foreach (DataRow row in clientes.Rows)
            {
                string nit = StringHelper.QuitarEspacios(row["nit"]?.ToString());
                int idCliente = Convert.ToInt32(row["idCliente"]);

                if (nit.Equals(cliente.Nit, StringComparison.OrdinalIgnoreCase) &&
                    idCliente != cliente.IdCliente)
                {
                    return Validacion.Fail("Ya existe un cliente con ese NIT.");
                }
            }

            return Validacion.Ok();
        }

        private static void AplicarConsumidorFinal(Cliente cliente, bool esConsumidorFinal)
        {
            if (!esConsumidorFinal)
                return;

            cliente.Nit = "CF";
            cliente.RazonSocial = "Consumidor Final";
        }
    }
}