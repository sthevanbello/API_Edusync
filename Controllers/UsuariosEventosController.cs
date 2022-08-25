using ApiMaisEventos.Interfaces;
using ApiMaisEventos.Models;
using ApiMaisEventos.Repositories;
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
        IUsuarioEventoRepository usuarioEventoRepository = new UsuarioEventoRepository();
        /// <summary>
        /// Cadastra um relacionamento de usuário com relação a um evento
        /// </summary>
        /// <param name="usuarioEvento">UsuárioEvento a ser cadastrado no banco</param>        /// <returns></returns>
        [HttpPost]
        public IActionResult PostUsuarioEvento(UsuarioEvento usuarioEvento)
        {

            // Open a data base connection

            try
            {
                var usuarioEventoInserido = usuarioEventoRepository.Insert(usuarioEvento);
                return Ok(usuarioEventoInserido);
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
        /// Lista a relação de relacionamento de usuários com eventos
        /// </summary>
        /// <returns>Retorna a lista de eventos associada aos usuários</returns>
        [HttpGet]
        public IActionResult GetUsuariosEventos()
        {
            try
            {
                var listaUsuarioEventos = usuarioEventoRepository.GetAll();                
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
        public IActionResult GetUsuariosComEventos()
        {
            try
            {
                var listaUsuarios = usuarioEventoRepository.GetParticipantesComEventos();
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
        /// Listar os eventos com participantes contidos na base de dados
        /// </summary>
        /// <returns>listaEventosComParticipantes</returns>
        [HttpGet]
        [Route("Usuarios")]
        public IActionResult GetEventosComUsuarios()
        {
            try
            {
                var listaEventos = usuarioEventoRepository.GetEventosComParticipantes();
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
                var eventoAtualizado = usuarioEventoRepository.Update(id, usuarioEvento);
                if (!eventoAtualizado)
                {
                    return NotFound(new { msg = "Não foi encontrado um relacionamento de UsuarioEvento com o id informado. Verificar se o Id está correto." });
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
                if (usuarioEventoRepository.Delete(id))
                {
                    return NotFound(new { msg = "Usuário não encontrado" });
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
