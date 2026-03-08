using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR;
using MySql.Data;
using MySql.Data.MySqlClient;
using System.Data;//ADO .NET

namespace ProyectoArqSoft.Pages
{
    public class ClienteModel : PageModel
    {
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

