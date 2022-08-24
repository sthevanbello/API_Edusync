using System.ComponentModel.DataAnnotations;

namespace ApiMaisEventos.Models
{
    public class Categoria
    {
        public int Id { get; set; }
        //[Required]
        public string NomeCategoria { get; set; }
    }
}
