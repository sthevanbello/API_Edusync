using ApiMaisEventos.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

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
        /// Listar os participantes com eventos contidos na base de dados
        /// </summary>
        /// <returns>listaEventosComParticipantes</returns>
        [HttpGet]
        [Route("Eventos")]
        public IActionResult GetParticipantesComEventos()
        {
            try
            {
                IList<Usuario> listausuarios = new List<Usuario>();
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string script = @"SELECT 
	                                    U.Id AS 'Id_Usuario', 
	                                    U.Nome AS 'Nome_Usuario', 
	                                    U.Email AS 'Email_Usuario',
                                        E.CategoriaId AS 'Id_Categoria', 
	                                    C.NomeCategoria AS 'Nome_Categoria',
                                        E.Id AS 'Id_Evento', 
	                                    E.DataHora AS 'Data_Hora_Evento',
	                                    E.Preco AS 'Preco_Evento',
	                                    E.Ativo AS 'Evento_Ativo'
                                    FROM TB_USUARIOS AS U
                                    INNER JOIN RL_USUARIO_EVENTO AS UE ON U.Id = UE.UsuarioId
                                    INNER JOIN TB_EVENTOS AS E ON E.Id = UE.EventoId
                                    INNER JOIN TB_CATEGORIAS AS C ON E.CategoriaId = C.Id";

                    using (SqlCommand cmd = new SqlCommand(script, connection))
                    {
                        // Ler todos os itens da consulta com while
                        cmd.CommandType = CommandType.Text;
                        using (var result = cmd.ExecuteReader())
                        {
                            while (result != null && result.HasRows && result.Read())
                            {
                                var evento = new Evento();
                                if (!string.IsNullOrEmpty(result["Id_Evento"].ToString()))
                                {
                                    evento = new Evento
                                    {
                                        Id = (int)result["Id_Evento"],
                                        DataHora = Convert.ToDateTime(result["Data_Hora_Evento"]),
                                        Ativo = (bool)result["Evento_Ativo"],
                                        Preco = (decimal)result["Preco_Evento"],
                                        Categoria = new Categoria
                                        {
                                            Id = (int)result["Id_Categoria"],
                                            NomeCategoria = result["Nome_Categoria"].ToString()
                                        },

                                    };
                                }

                                if (!listausuarios.Any(x => x.Id == (int)result["Id_Usuario"]))
                                {
                                    var usuario = new Usuario
                                    {
                                        Id = (int)result["Id_Usuario"],
                                        Nome = result["Nome_Usuario"].ToString(),
                                        Email = result["Email_Usuario"].ToString(),
                                        Eventos = new List<Evento>()
                                    };

                                    if ((usuario?.Id ?? 0) > 0)
                                    {
                                        usuario.Eventos.Add(evento);
                                    }

                                    listausuarios.Add(usuario);
                                }
                                else if ((evento?.Id ?? 0) > 0)
                                {
                                    listausuarios.FirstOrDefault(x => x.Id == (int)result["Id_Usuario"]).Eventos.Add(evento);
                                }
                            }
                        }

                    }
                }
                return Ok(listausuarios);
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
        /// Listar os eventos com participantes contidos na base de dados
        /// </summary>
        /// <returns>listaEventosComParticipantes</returns>
        [HttpGet]
        [Route("Usuarios")]
        public IActionResult GetEventosComParticipantes()
        {
            try
            {
                IList<Evento> listaEventos = new List<Evento>();
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string script = @"SELECT 
	                                    E.Id AS 'Id_Evento', 
	                                    E.DataHora AS 'Data_Hora_Evento', 
	                                    E.Ativo AS 'Evento_Ativo', 
	                                    E.Preco AS 'Preco_Evento',
	                                    E.CategoriaId AS 'Id_Categoria', 
	                                    C.NomeCategoria AS 'Nome_Categoria',
	                                    U.Id AS 'Id_Usuario', 
	                                    U.Nome AS 'Nome_Usuario', 
	                                    U.Email AS 'Email_Usuario'
                                    FROM TB_EVENTOS AS E
                                    INNER JOIN TB_CATEGORIAS AS C ON E.CategoriaId = C.Id
                                    LEFT JOIN RL_USUARIO_EVENTO AS UE ON UE.EventoId = E.Id
                                    LEFT JOIN TB_USUARIOS AS U ON U.Id = UE.UsuarioId";

                    using (SqlCommand cmd = new SqlCommand(script, connection))
                    {
                        // Ler todos os itens da consulta com while
                        cmd.CommandType = CommandType.Text;
                        using (var result = cmd.ExecuteReader())
                        {
                            while (result != null && result.HasRows && result.Read())
                            {
                                var usuario = new Usuario();
                                if (!string.IsNullOrEmpty(result["Id_Usuario"].ToString()))
                                {
                                    usuario = new Usuario
                                    {
                                        Id = (int)result["Id_Usuario"],
                                        Nome = result["Nome_Usuario"].ToString(),
                                        Email = result["Email_Usuario"].ToString()
                                    };
                                }

                                if (!listaEventos.Any(x => x.Id == (int)result["Id_Evento"]))
                                {
                                    var evento = new Evento
                                    {
                                        Id = (int)result["Id_Evento"],
                                        DataHora = Convert.ToDateTime(result["Data_Hora_Evento"]),
                                        Ativo = (bool)result["Evento_Ativo"],
                                        Preco = (decimal)result["Preco_Evento"],
                                        Categoria = new Categoria
                                        {
                                            Id = (int)result["Id_Categoria"],
                                            NomeCategoria = result["Nome_Categoria"].ToString()
                                        },
                                        Usuarios = new List<Usuario>()
                                    };

                                    if ((usuario?.Id ?? 0) > 0)
                                    {
                                        evento.Usuarios.Add(usuario);
                                    }

                                    listaEventos.Add(evento);
                                }
                                else if ((usuario?.Id ?? 0) > 0)
                                {
                                    listaEventos.FirstOrDefault(x => x.Id == (int)result["Id_Evento"]).Usuarios.Add(usuario);
                                }
                            }
                        }

                    }
                }
                return Ok(listaEventos);
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
        /// <summary>
        /// Apaga um registro do relacionamento Usuário Evento
        /// </summary>
        /// <param name="id">Id do relacionamento que será apagado</param>
        /// <returns>Retorna uma mensagem se o registro for apagado</returns>
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
