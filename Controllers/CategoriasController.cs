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
    public class CategoriasController : ControllerBase
    {
        private readonly string connectionString = @"data source=NOTE_STHEVAN\SQLEXPRESS; User Id=sa; Password=Admin1234; Initial Catalog = Mais_Eventos";
        /// <summary>
        /// Insere no banco uma nova categoria
        /// </summary>
        /// <param name="categoria">Categoria nova a ser inserida</param>
        /// <returns>Retorna os dados da categoria nova</returns>
        // POST
        [HttpPost]
        public IActionResult PostCategoria(Categoria categoria)
        {

            // Open a data base connection

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string script = @"INSERT INTO TB_CATEGORIAS (NomeCategoria)
                                    VALUES
                                    (@NomeCategoria)";

                    // Execução no banco
                    using (SqlCommand cmd = new SqlCommand(script, connection))
                    {
                        // Declarar as variáveis por parâmetros
                        cmd.Parameters.Add("NomeCategoria", SqlDbType.NVarChar).Value = categoria.NomeCategoria;
                        cmd.CommandType = CommandType.Text;
                        cmd.ExecuteNonQuery();
                    }
                }
                return Ok(categoria);
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
        /// Listar todas as categorias existente no banco
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetCategorias()
        {
            try
            {
                IList<Categoria> listaCategorias = new List<Categoria>();
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string script = "SELECT * FROM TB_CATEGORIAS";
                    using (SqlCommand cmd = new SqlCommand(script, connection))
                    {
                        // Ler todos os itens da consulta com foreach e while
                        cmd.CommandType = CommandType.Text;
                        using (var result = cmd.ExecuteReader())
                        {
                            while (result != null && result.HasRows && result.Read())
                            {
                                listaCategorias.Add(new Categoria
                                {
                                    Id = (int)result[0],
                                    NomeCategoria = (string)result[1],
                                });
                            }
                        }

                    }
                }
                return Ok(listaCategorias);
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
        /// Atualiza a categoria de acordo com o id informado como parâmetro
        /// </summary>
        /// <param name="categoria">Nome da categoria</param>
        /// <param name="id">Id da categoria que será atualizada</param>
        /// <returns>Retorna a categoria atualizada</returns>
        [HttpPut("{id}")]
        public IActionResult PutCategoria(Categoria categoria, int id)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string script = @"UPDATE TB_CATEGORIAS SET NomeCategoria = @NomeCategoria WHERE id = @id";

                    // Execução no banco
                    using (SqlCommand cmd = new SqlCommand(script, connection))
                    {
                        // Declarar as variáveis por parâmetros
                        cmd.Parameters.Add("Id", SqlDbType.Int).Value = id;
                        cmd.Parameters.Add("NomeCategoria", SqlDbType.NVarChar).Value = categoria.NomeCategoria;
                        cmd.CommandType = CommandType.Text;
                        cmd.ExecuteNonQuery();
                    }
                }
                return Ok(categoria);
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
        /// Apaga uma categoria existente no banco
        /// </summary>
        /// <param name="id">Id da categoria que será apagada</param>
        /// <returns>Retorna uma mensagem informando que a categoria foi apagada</returns>
        [HttpDelete("{id}")]
        public IActionResult DeleteCategoria(int id)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string script = @"DELETE FROM TB_CATEGORIAS WHERE id = @id";

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
                    msg = "Categoria deletada com sucesso"
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
