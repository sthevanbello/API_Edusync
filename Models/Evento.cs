using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiMaisEventos.Models
{
    public class Evento
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "DataHora é obrigatório")]
        public DateTime DataHora { get; set; }

        [Required(ErrorMessage ="Campo Ativo é obrigatório")]
        public bool Ativo { get; set; }

        [Required(ErrorMessage = "Campo Preco é obrigatório")]
        public decimal Preco { get; set; }

        [Required(ErrorMessage = "Informar a Categoria é obrigatório")]
        public Categoria Categoria { get; set; }

        public IList<Usuario> Usuarios { get; set; }
    }
}
