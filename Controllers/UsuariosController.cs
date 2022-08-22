using ApiMaisEventos.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;

namespace ApiMaisEventos.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        // Criar string de conexão com o banco de dados
        // private readonly string connectionString = @"data source=NOTE_STHEVAN\SQLEXPRESS; Integrated Security = true; Initial Catalog = Mais_Eventos";
        private readonly string connectionString = @"data source=NOTE_STHEVAN\SQLEXPRESS; User Id=sa; Password=Admin1234; Initial Catalog = Mais_Eventos";

        // POST - Cadastrar
        [HttpPost]
        public IActionResult Cadastrar(Usuario usuario)
        {
            // Open a data base connection

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string script = @"INSERT INTO TB_USUARIOS (Nome, Email, Senha)
                                    VALUES
                                    (@Nome, @Email, @Senha)";

                    // Execução no banco
                    using (SqlCommand cmd = new SqlCommand(script, connection))
                    {
                        // Declarar as variáveis por parâmetros
                        cmd.Parameters.Add("Nome", SqlDbType.NVarChar).Value = usuario.Nome;
                        cmd.Parameters.Add("Email", SqlDbType.NVarChar).Value = usuario.Email;
                        cmd.Parameters.Add("Senha", SqlDbType.NVarChar).Value = usuario.Senha;
                        cmd.CommandType = CommandType.Text;
                        cmd.ExecuteNonQuery();
                    }
                }
                return Ok(usuario);
            }
            catch (System.Exception ex)
            {

                return StatusCode(500, new
                {
                    msg = "Falha na conexão",
                    erro = ex.Message,

                });
            }
        }
        // GET - Listar

        // PUT - Alterar

        // Delete - Excluir
    }
}
