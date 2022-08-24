using ApiMaisEventos.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Http.Headers;

namespace ApiMaisEventos.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        // Criar string de conexão com o banco de dados
        // private readonly string connectionString = @"data source=NOTE_STHEVAN\SQLEXPRESS; Integrated Security = true; Initial Catalog = Mais_Eventos";
        private readonly string connectionString = @"data source=NOTE_STHEVAN\SQLEXPRESS; User Id=sa; Password=Admin1234; Initial Catalog = Mais_Eventos";

        /// <summary>
        /// Cadastra usuário no banco de dados
        /// </summary>
        /// <param name="usuario"> Dados do usuário novo</param>
        /// <returns>Retorna os dados do usuário se tudo estiver correto</returns>
        // POST - Cadastrar
        [HttpPost]
        public IActionResult CadastrarUsuario(Usuario usuario)
        {
            // Open a data base connection

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string script = @"INSERT INTO TB_USUARIOS 
                                        (Nome, Email, Senha)
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
        // GET - Listar
        /// <summary>
        /// Lista todos os usuários da aplicação com foreach e while
        /// </summary>
        /// <returns>Lista de todos os usuários do banco de dados</returns>
        [HttpGet]
        public IActionResult GetUsuarios()
        {
            try
            {
                IList<Usuario> listaUsuarios = new List<Usuario>();
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string script = @"SELECT 
                                        * 
                                    FROM TB_USUARIOS";
                    using (SqlCommand cmd = new SqlCommand(script, connection))
                    {
                        // Ler todos os itens da consulta com foreach e while
                        cmd.CommandType = CommandType.Text;
                        using (var result = cmd.ExecuteReader())
                        {
                            #region foreach
                            //foreach ( IDataRecord item in result)
                            //{
                            //    listaUsuarios.Add(new Usuario
                            //    {
                            //        Id = (int)item[0],
                            //        Nome = (string)item[1],
                            //        Email = (string)item[2],
                            //        Senha = (string)item[3]
                            //    });
                            //}
                            #endregion

                            while (result.Read())
                            {
                                listaUsuarios.Add(new Usuario
                                {
                                    Id = (int)result[0],
                                    Nome = (string)result[1],
                                    Email = (string)result[2],
                                    Senha = (string)result[3]
                                });
                            }
                        }

                    }
                }
                //IFormFile arquivo = new FormFile();
                //string nomeArquivo = ContentDispositionHeaderValue.Parse(arquivo.ContentDisposition).FileName.Trim('"');
                return Ok(listaUsuarios);
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
                                    LEFT JOIN TB_EVENTOS AS E ON E.Id = UE.EventoId
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
        /// Altera a tupla de acordo com o id fornecido como parâmetro
        /// </summary>
        /// <param name="id">Id do usuário</param>
        /// <param name="usuario">Usuário a ser inserido</param>
        /// <returns>Retorna os dados atualizados do usuário</returns>
        // PUT - Alterar
        [HttpPut("{id}")]
        public IActionResult PutUsuario(int id, Usuario usuario)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string script = @"UPDATE TB_USUARIOS 
                                        SET Nome = @Nome, Email = @Email, Senha = @Senha 
                                    WHERE Id = @id";

                    // Execução no banco
                    using (SqlCommand cmd = new SqlCommand(script, connection))
                    {
                        // Declarar as variáveis por parâmetros
                        cmd.Parameters.Add("Id", SqlDbType.NVarChar).Value = id;
                        cmd.Parameters.Add("Nome", SqlDbType.NVarChar).Value = usuario.Nome;
                        cmd.Parameters.Add("Email", SqlDbType.NVarChar).Value = usuario.Email;
                        cmd.Parameters.Add("Senha", SqlDbType.NVarChar).Value = usuario.Senha;
                        cmd.CommandType = CommandType.Text;
                        cmd.ExecuteNonQuery();
                        usuario.Id = id;
                    }
                }
                return Ok(usuario);
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
        /// Deleta um usuário de acordo com o id fornecido como parâmetro
        /// </summary>
        /// <param name="id">Id do usuário a ser deletado</param>
        /// <returns>Retorna uma mensagem de exclusão</returns>
        // Delete - Excluir
        [HttpDelete("{id}")]
        public IActionResult DeleteUsuario(int id)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string script = @"DELETE FROM TB_USUARIOS WHERE Id = @id";

                    // Execução no banco
                    using (SqlCommand cmd = new SqlCommand(script, connection))
                    {
                        // Declarar as variáveis por parâmetros
                        cmd.Parameters.Add("Id", SqlDbType.NVarChar).Value = id;
                        cmd.CommandType = CommandType.Text;
                        cmd.ExecuteNonQuery();
                    }
                }
                return Ok(new {msg = "Usuário excluído com sucesso"});
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