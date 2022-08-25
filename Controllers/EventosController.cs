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
        private readonly string connectionString = @"data source=NOTE_STHEVAN\SQLEXPRESS; User Id=sa; Password=Admin1234; Initial Catalog = Mais_Eventos";

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
                return Ok(eventoAtualizado);
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
                if (eventoRepository.DeleteEvento(id))
                {
                    return NotFound(new { msg = "Usuário não encontrado" });
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
