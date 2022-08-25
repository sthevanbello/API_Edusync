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
    public class EventosController : ControllerBase
    {
        IEventoRepository eventoRepository = new EventoRepository();
        /// <summary>
        /// Cadastrar um evento
        /// </summary>
        /// <param name="evento">Recebe o evento como parâmetro</param>
        /// <returns>Retorna o evento cadastrado</returns>
        [HttpPost]
        public IActionResult PostEvento(Evento evento)
        {
            try
            {
                var eventoInserido = eventoRepository.InsertEvento(evento);
                return Ok(eventoInserido);
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
        /// Listar os eventos contidos na base de dados
        /// </summary>
        /// <returns>listaEventos</returns>
        [HttpGet]
        public IActionResult GetEventos()
        {
            try
            {
                var listaEventos = eventoRepository.GetEventosAll();
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
        /// Lista o evento de acordo com o id informado
        /// </summary>
        /// <param name="id">Id do evento a ser buscado</param>
        /// <returns>Retorna o evento</returns>
        [HttpGet("{id}")]
        public IActionResult GetEventoById(int id)
        {
            try
            {
                var evento = eventoRepository.GetEventoById(id);
                if (evento is null)
                {
                    return NotFound(new { msg = "Não foi encontrado um evento com o id informado. Verificar se o Id está correto" });
                }
                return Ok(evento);
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
                var listaEventos = eventoRepository.GetEventosComUsuarios();
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
        /// Modifica um evento ao receber um id e os dados do evento.
        /// </summary>
        /// <param name="evento">Dados atualizados do evento</param>
        /// <param name="id">Id do evento a ser modificado</param>
        /// <returns>Retorna o evento com os dados atualizados</returns>
        // PUT
        [HttpPut("{id}")]
        public IActionResult PutEvento(Evento evento, int id)
        {
            try
            {
                var eventoAtualizado = eventoRepository.UpdateEvento(id, evento);
                if (!eventoAtualizado)
                {
                    return NotFound(new {msg = "Não foi encontrado um Evento com o id informado. Verificar se o Id está correto" });
                }
                return Ok(evento);
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
        /// Apaga o evento ao receber o id
        /// </summary>
        /// <param name="id">Id do evento a ser apagado</param>
        /// <returns>Retorna uma mensagem após apagar o evento</returns>
        // DELETE
        [HttpDelete("{id}")]
        public IActionResult DeleteEvento(int id)
        {
            try
            {
                if (!eventoRepository.DeleteEvento(id))
                {
                    return NotFound(new { msg = "Evento não foi deletado. Verificar se o Id está correto" });
                }
                return Ok(new
                {
                    msg = "Evento deletado com sucesso"
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
