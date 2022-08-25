using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiMaisEventos.Models
{
    public class UsuarioEvento
    {
        public int Id { get; set; }

        public Usuario Usuario { get; set; }
        public Evento Evento { get; set; }


        [Required(ErrorMessage = "Informar o Evento associado é obrigatório")]
        public int UsuarioId { get; set; }
        [Required(ErrorMessage = "Informar o Usuário associado é obrigatório")]
        public int EventoId { get; set; }
    }
}
