using ApiMaisEventos.Interfaces;
using ApiMaisEventos.Models;
using ApiMaisEventos.Repositories;
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
        public ICategoriaRepository categoriaRepository = new CategoriaRepository();
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
                var categoriaInserida = categoriaRepository.Insert(categoria);
                return Ok(categoriaInserida);
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
                var listaCategorias = categoriaRepository.GetCategoriasAll();
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
        /// Listar todas as categorias existente no banco
        /// </summary>
        /// <returns></returns>
        [HttpGet("{id}")]
        public IActionResult GetCategoriaById(int id)
        {
            try
            {
                var categoria = categoriaRepository.GetCategoriaById(id);
                if (categoria is null)
                {
                    return NotFound(new { msg = "Não foi encontrado um evento com o id informado" });
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
                var categoriaAtualizada = categoriaRepository.Update(id, categoria);
                if (!categoriaAtualizada)
                {
                    return NotFound(new { msg = "Não foi encontrada uma categoria com o id informado" });
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
                if (!categoriaRepository.Delete(id))
                {
                    return NotFound(new { msg = "Categoria não foi deletada. Verificar se o Id está correto" });
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
