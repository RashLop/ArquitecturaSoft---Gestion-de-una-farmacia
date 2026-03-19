using ProyectoArqSoft.FactoryProducts;
using ProyectoArqSoft.Helpers;
using ProyectoArqSoft.Models;
using ProyectoArqSoft.Validaciones;
using System.Data;

namespace ProyectoArqSoft.Services
{
    public class ClienteService : IClienteService
    {
        private readonly IRepository<Cliente> repository;
        private readonly IValidacion<Cliente> validador;

        public ClienteService(
            IRepository<Cliente> repository,
            IValidacion<Cliente> validador)
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

        public Cliente? ObtenerPorId(int id)
        {
            return repository.GetById(id);
        }

        public Validacion Crear(
            bool esConsumidorFinal,
            string nit,
            string razonSocial,
            string? correoElectronico)
        {
            Cliente cliente = new Cliente
            {
                Nit = nit,
                RazonSocial = razonSocial,
                CorreoElectronico = correoElectronico ?? string.Empty
            };

            AplicarConsumidorFinal(cliente, esConsumidorFinal);
            LimpiarCampos(cliente);

            Validacion resultado = validador.Validar(cliente);
            if (!resultado.EsValido)
                return resultado;

            resultado = ValidarDuplicado(cliente);
            if (!resultado.EsValido)
                return resultado;

            int filasAfectadas = repository.Insert(cliente);

            if (filasAfectadas <= 0)
                return new Validacion(false, "No se pudo registrar el cliente.");

            return new Validacion(true);
        }

        public Validacion Actualizar(
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

            Validacion resultado = validador.Validar(cliente);
            if (!resultado.EsValido)
                return resultado;

            resultado = ValidarDuplicado(cliente);
            if (!resultado.EsValido)
                return resultado;

            int filasAfectadas = repository.Update(cliente);

            if (filasAfectadas <= 0)
                return new Validacion(false, "No se pudo actualizar el cliente.");

            return new Validacion(true);
        }

        public Validacion Eliminar(int id)
        {
            Cliente cliente = new Cliente
            {
                IdCliente = id
            };

            int filasAfectadas = repository.Delete(cliente);

            if (filasAfectadas <= 0)
                return new Validacion(false, "No se pudo eliminar el cliente.");

            return new Validacion(true);
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
                return new Validacion(true);

            DataTable clientes = repository.GetAll(cliente.Nit);

            foreach (DataRow row in clientes.Rows)
            {
                string nit = StringHelper.QuitarEspacios(row["nit"].ToString());
                int idCliente = Convert.ToInt32(row["idCliente"]);

                if (nit.Equals(cliente.Nit, StringComparison.OrdinalIgnoreCase) &&
                    idCliente != cliente.IdCliente)
                {
                    return new Validacion(false, "Ya existe un cliente con ese NIT.");
                }
            }

            return new Validacion(true);
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
