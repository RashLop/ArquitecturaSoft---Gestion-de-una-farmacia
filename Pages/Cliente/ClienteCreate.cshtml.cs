using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MySql.Data.MySqlClient;


namespace ProyectoArqSoft.Pages
{

    public class ClienteCreateModel : PageModel
    {
        private readonly IConfiguration Configuration;
     
        [BindProperty]
        public string Nombre{ get; set; }

         [BindProperty]
        public string Tipo_Cliente{ get; set; }

        [BindProperty]
        public string Carnet{ get; set; }

        [BindProperty]
        public string Edad{ get; set; }

         [BindProperty]
        public string Telefono{ get; set; }

        public ClienteCreateModel(IConfiguration configuration) 
        {
            this.Configuration=configuration;
        }       
        public IActionResult OnPost()
        {
            string connectionString = Configuration.GetConnectionString("MySqlConnection")!;
            string query = @"INSERT INTO cliente (tipo_cliente, nombre, ci, edad, telefono)
                 VALUES(@tipo_cliente, @nombre, @ci, @edad, @telefono)";

            using (MySqlConnection connection= new MySqlConnection(connectionString))
            { 
               MySqlCommand command= new MySqlCommand(query, connection);
               command.Parameters.AddWithValue("@tipo_cliente", Tipo_Cliente);
               command.Parameters.AddWithValue("@nombre", Nombre);
               command.Parameters.AddWithValue("@ci",Carnet );
               command.Parameters.AddWithValue("@edad", Edad);
               command.Parameters.AddWithValue("@telefono", Telefono);

               connection.Open();
               command.ExecuteNonQuery();
            
            }

            return RedirectToPage("Cliente");
        }
    
    
    }


}
