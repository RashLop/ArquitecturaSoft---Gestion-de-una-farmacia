<<<<<<< Updated upstream
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR;
using MySql.Data;
using MySql.Data.MySqlClient;
using System.Data;//ADO .NET
=======
﻿using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using ProyectoArqSoft.Helpers;
using ProyectoArqSoft.Pages.Base;
using ProyectoArqSoft.Validaciones;
using System.Data;
>>>>>>> Stashed changes

namespace ProyectoArqSoft.Pages
{
    public class ClienteModel : BasePageModel
    {
<<<<<<< Updated upstream
    //variable global datatable    
    public DataTable Clientes_DataTable{get; set;}=new DataTable();


    [BindProperty(SupportsGet = true)]
    public string TipoBusqueda { get; set; }

    [BindProperty(SupportsGet = true)]
    public string TextoBusqueda { get; set; }

    public string Mensaje { get; set; } = "";


    private readonly IConfiguration configuration;

       public ClienteModel(IConfiguration configuration)
       {
        this.configuration=configuration;
       }

        public void OnGet()
         {
          select();
          BuscarCliente();
         }

        void select()
        {
           string connectionString=configuration.GetConnectionString("MySqlConnection")!;
           string query=@"select idCliente,nombre,tipo_cliente,ci,edad,telefono
           from cliente
           order by 2";


           using(MySqlConnection connection=new MySqlConnection(connectionString))
            {
                  connection.Open();  
                  MySqlCommand comando= new MySqlCommand(query,connection); 
                  MySqlDataAdapter adapter=new MySqlDataAdapter(comando);
                  adapter.Fill(Clientes_DataTable);

            }
        }





void BuscarCliente()
{
    if (TextoBusqueda == null || TextoBusqueda.Trim() == "" ||
        TipoBusqueda == null || TipoBusqueda.Trim() == "")
    {
        return;
    }

    string texto = TextoBusqueda.Trim();

    if (TipoBusqueda == "ci" || TipoBusqueda == "telefono")
    {
        if (EsNumero(texto) == false)
        {
            Mensaje = "Criterio inválido";
            Clientes_DataTable.Rows.Clear();
            return;
        }
    }

    string connectionString = configuration.GetConnectionString("MySqlConnection")!;
    string query = "";

    if (TipoBusqueda == "nombre")
    {
        query = @"select idCliente,nombre,tipo_cliente,ci,edad,telefono
                  from cliente
                  where nombre like @dato
                  order by nombre";
    }
    else if (TipoBusqueda == "ci")
    {
        query = @"select idCliente,nombre,tipo_cliente,ci,edad,telefono
                  from cliente
                  where ci like @dato
                  order by nombre";
    }
    else if (TipoBusqueda == "telefono")
    {
        query = @"select idCliente,nombre,tipo_cliente,ci,edad,telefono
                  from cliente
                  where telefono like @dato
                  order by nombre";
    }

    Clientes_DataTable.Rows.Clear();

    using (MySqlConnection connection = new MySqlConnection(connectionString))
    {
        connection.Open();

        MySqlCommand comando = new MySqlCommand(query, connection);
        comando.Parameters.AddWithValue("@dato", "%" + texto + "%");

        MySqlDataAdapter adapter = new MySqlDataAdapter(comando);
        adapter.Fill(Clientes_DataTable);
    }

    if (Clientes_DataTable.Rows.Count == 0)
    {
        Mensaje = "No se encontraron clientes";
    }
}

 bool EsNumero(string texto)
    {
        for (int i = 0; i < texto.Length; i++)
        {
            if (char.IsDigit(texto[i]) == false)
            {
                return false;
            }
        }

        return true;
    }


    }
}

=======
        private readonly IConfiguration configuration;

        public DataTable ClienteDataTable { get; set; } = new DataTable();

        public ClienteModel(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public void OnGet(string? filtro, string? mensaje, string? error)
        {
            CargarParametros(filtro, mensaje, error);

            Validacion resultado = FiltroHelper.ValidarFiltro(Estado.FiltroActual);
            Estado.MensajeError = resultado.MensajeError;

            if (!resultado.EsValido)
                return;

            CargarClientes(Estado.FiltroActual);
        }

        public IActionResult OnPostEliminarClienteLogicamente(int id)
        {
            EliminarClienteLogicamente(id);
            return RedirectToPage("Cliente", new { mensaje = "Cliente eliminado correctamente" });
        }

        private void CargarParametros(string? filtro, string? mensaje, string? error)
        {
            Estado.FiltroActual = FiltroHelper.LimpiarFiltro(filtro);
            Estado.Mensaje = mensaje ?? string.Empty;
            Estado.MensajeError = error ?? string.Empty;
        }

        private void CargarClientes(string filtro)
        {
            string connectionString = configuration.GetConnectionString("MySqlConnection")!;

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                string query = ConstruirQuery(filtro);
                MySqlCommand command = new MySqlCommand(query, connection);

                FiltroSqlHelper.AgregarParametrosLike(command, filtro);

                MySqlDataAdapter adapter = new MySqlDataAdapter(command);
                adapter.Fill(ClienteDataTable);
            }
        }

        private string ConstruirQuery(string filtro)
        {
            string query = @"SELECT idCliente,
                                    tipo_cliente,
                                    nombre,
                                    apellido_materno,
                                    apellido_paterno,
                                    ci_extencion,
                                    ci,
                                    fecha_de_nacimiento,
                                    telefono
                             FROM cliente
                             WHERE estado = 1";

            query += FiltroSqlHelper.ConstruirCondicionLike(
                filtro,
                "nombre",
                "apellido_materno",
                "apellido_paterno",
                "tipo_cliente",
                "ci",
                "ci_extencion",
                "telefono"
            );

            query += " ORDER BY nombre, apellido_paterno, apellido_materno";

            return query;
        }

        private void EliminarClienteLogicamente(int id)
        {
            string connectionString = configuration.GetConnectionString("MySqlConnection")!;
            string query = @"UPDATE cliente
                             SET estado = 0,
                                 ultima_actualizacion = NOW()
                             WHERE idCliente = @id";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@id", id);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }
    }
}
>>>>>>> Stashed changes
