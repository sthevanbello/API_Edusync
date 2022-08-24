using ApiMaisEventos.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace ApiMaisEventos.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosEventosController : ControllerBase
    {
        private readonly string connectionString = @"data source=NOTE_STHEVAN\SQLEXPRESS; User Id=sa; Password=Admin1234; Initial Catalog = Mais_Eventos";

        /// <summary>
        /// Cadastra um relacionamento de usuário com relação a um evento
        /// </summary>
        /// <param name="usuario">Usuário a ser cadastrado ao evento</param>
        /// <param name="evento">Evento a ser cadastrado para o usuário</param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult PostUsuarioEvento(UsuarioEvento usuarioEvento)
        {

            // Open a data base connection

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string script = @"INSERT INTO RL_USUARIO_EVENTO (UsuarioId, EventoId)
                                    VALUES
                                    (@UsuarioId, @EventoId)";

                    // Execução no banco
                    using (SqlCommand cmd = new SqlCommand(script, connection))
                    {
                        // Declarar as variáveis por parâmetros
                        cmd.Parameters.Add("UsuarioId", SqlDbType.NVarChar).Value = usuarioEvento.UsuarioId;
                        cmd.Parameters.Add("EventoId", SqlDbType.NVarChar).Value = usuarioEvento.EventoId;
                        cmd.CommandType = CommandType.Text;
                        cmd.ExecuteNonQuery();
                    }
                }
                return Ok(new
                {
                    msg = $"{usuarioEvento}"
                });
            }
            catch (InvalidOperationException ex)
            {
                return StatusCode(500, new
                {
                    msg = "Falha na conexão",
                    erro = ex.Message,
                });
            }
            catch (SqlException ex)
            {
                return StatusCode(500, new
                {
                    msg = "Falha na sintaxe do código SQL",
                    erro = ex.Message,
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    msg = "Falha na definição do código",
                    erro = ex.Message
                });
            }
        }
        /// <summary>
        /// Lista a relação de usuários com eventos
        /// </summary>
        /// <returns>Retorna a lista de eventos associada aos usuários</returns>
        [HttpGet]
        public IActionResult GetUsuarioEvento()
        {
            try
            {
                // Open a data base connection
                List<UsuarioEvento> listaUsuarioEventos = new List<UsuarioEvento>();
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string script = @"SELECT UE.UsuarioId, UE.EventoId FROM RL_USUARIO_EVENTO AS UE";

                    // Execução no banco
                    using (SqlCommand cmd = new SqlCommand(script, connection))
                    {
                        // Declarar as variáveis por parâmetros
                        cmd.CommandType = CommandType.Text;
                        using (var result = cmd.ExecuteReader())
                        {
                            while (result != null && result.HasRows && result.Read())
                            {
                                listaUsuarioEventos.Add(new UsuarioEvento
                                {
                                    EventoId = (int)result["UsuarioId"],
                                    UsuarioId = (int)result["EventoId"]
                                });
                            }
                        }

                    }
                }
                return Ok(listaUsuarioEventos);
            }
            catch (InvalidOperationException ex)
            {
                return StatusCode(500, new
                {
                    msg = "Falha na conexão",
                    erro = ex.Message,
                });
            }
            catch (SqlException ex)
            {
                return StatusCode(500, new
                {
                    msg = "Falha na sintaxe do código SQL",
                    erro = ex.Message,
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    msg = "Falha na definição do código",
                    erro = ex.Message
                });
            }
        }

        /// <summary>
        /// Atualiza a relação entre usuário e evento
        /// </summary>
        /// <param name="usuarioEvento">Dados atualizados</param>
        /// <param name="id">Id do relacionamento</param>
        /// <returns>Retorna os dados atualizados</returns>
        [HttpPut("{id}")]
        public IActionResult PutUsuarioEvento(UsuarioEvento usuarioEvento, int id)
        {

            // Open a data base connection

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string script = @"UPDATE RL_USUARIO_EVENTO SET UsuarioId = @UsuarioId, EventoId = @EventoId WHERE Id = @Id";

                    // Execução no banco
                    using (SqlCommand cmd = new SqlCommand(script, connection))
                    {
                        // Declarar as variáveis por parâmetros
                        cmd.Parameters.Add("Id", SqlDbType.Int).Value = id;
                        cmd.Parameters.Add("UsuarioId", SqlDbType.NVarChar).Value = usuarioEvento.UsuarioId;
                        cmd.Parameters.Add("EventoId", SqlDbType.NVarChar).Value = usuarioEvento.EventoId;
                        cmd.CommandType = CommandType.Text;
                        cmd.ExecuteNonQuery();
                    }
                }
                return Ok(usuarioEvento);
            }
            catch (InvalidOperationException ex)
            {
                return StatusCode(500, new
                {
                    msg = "Falha na conexão",
                    erro = ex.Message,
                });
            }
            catch (SqlException ex)
            {
                return StatusCode(500, new
                {
                    msg = "Falha na sintaxe do código SQL",
                    erro = ex.Message,
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    msg = "Falha na definição do código",
                    erro = ex.Message
                });
            }
        }
        [HttpDelete("{id}")]
        public IActionResult DeleteUsuarioEvento(int id)
        {

            // Open a data base connection

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string script = @"DELETE FROM RL_USUARIO_EVENTO WHERE Id = @Id";

                    // Execução no banco
                    using (SqlCommand cmd = new SqlCommand(script, connection))
                    {
                        // Declarar as variáveis por parâmetros
                        cmd.Parameters.Add("Id", SqlDbType.Int).Value = id;
                        cmd.CommandType = CommandType.Text;
                        cmd.ExecuteNonQuery();
                    }
                }
                return Ok(new
                {
                    msg = "Registro deletado com sucesso"
                });
            }
            catch (InvalidOperationException ex)
            {
                return StatusCode(500, new
                {
                    msg = "Falha na conexão",
                    erro = ex.Message,
                });
            }
            catch (SqlException ex)
            {
                return StatusCode(500, new
                {
                    msg = "Falha na sintaxe do código SQL",
                    erro = ex.Message,
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    msg = "Falha na definição do código",
                    erro = ex.Message
                });
            }
        }
    }
}
