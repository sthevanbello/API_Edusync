using System;

namespace ApiMaisEventos.Models
{
    public class Evento
    {
        public int Id { get; set; }
        public DateTime DataHora { get; set; }
        public bool Ativo { get; set; }
        public decimal Preco { get; set; }
        public int CategoriaId { get; set; }
    }
}
