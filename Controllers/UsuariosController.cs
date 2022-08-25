using ApiMaisEventos.Interfaces;
using ApiMaisEventos.Models;
using ApiMaisEventos.Repositories;
using ApiMaisEventos.Utils;
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
        private IUsuarioRepository usuarioRepository = new UsuarioRepository();
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
                usuarioRepository.Insert(usuario);
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
        /// Cadastrar Usuário com imagem
        /// </summary>
        /// <param name="usuario"></param>
        /// <returns>Retorna o usuário cadastrado</returns>
        [HttpPost("Imagem")]
        public IActionResult CadastrarUsuarioComImagem([FromForm]Usuario usuario, IFormFile arquivo)
        {
            // Open a data base connection

            try
            {
                #region Upload de Imagem
                string[] extensoesPermitidas = { "jpg", "jpeg", "png", "svg" };
                string uploadResultado = Upload.UploadFile(arquivo, extensoesPermitidas, "Images");
                if (string.IsNullOrEmpty(uploadResultado))
                {
                    return BadRequest("Arquivo não encontrado ou extensão não permitida");
                }
                usuario.Imagem = uploadResultado;
                #endregion

                usuarioRepository.InsertUsuarioComImagem(usuario);
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
                return Ok(usuarioRepository.GetAll());
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
        /// Lista o usuários da aplicação de acordo com o id fornecido
        /// </summary>
        /// <param name="id">Id do usuário</param>
        /// <returns>Lista de todos os usuários do banco de dados</returns>
        [HttpGet("{id}")]
        public IActionResult GetUsuarioById(int id)
        {
            try
            {
                var usuario = usuarioRepository.GetById(id);
                if (usuario is null)
                {
                    return NotFound(new {msg = "Usuário não encontrado. Verificar se o Id está correto."});  
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
        /// Listar os participantes com eventos contidos na base de dados
        /// </summary>
        /// <returns>listaEventosComParticipantes</returns>
        [HttpGet]
        [Route("Eventos")]
        public IActionResult GetUsuarioComEventos()
        {
            try
            {
                return Ok(usuarioRepository.GetUsuariosComEventos());
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
        public IActionResult UpdateUsuario(int id, Usuario usuario)
        {
            try
            {
                var result = usuarioRepository.Update(id, usuario);
                if (!result)
                {
                    return NotFound(new { msg = "Usuário não encontrado. Verificar se o Id está correto." });
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
                if (usuarioRepository.Delete(id))
                {
                    return Ok(new { msg = "Usuário excluído com sucesso" });
                }
                return NotFound(new { msg = "Usuário não encontrado. Verificar se o Id está correto." });
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