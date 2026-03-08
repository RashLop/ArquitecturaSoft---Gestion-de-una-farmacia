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

    private readonly IConfiguration configuration;

       public ClienteModel(IConfiguration configuration)
       {
        this.configuration=configuration;
       }

        public void OnGet()
        {
          select();
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

    }
}

