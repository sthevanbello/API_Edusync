using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ApiMaisEventos.Models
{
    public class Usuario
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "informe o seu nome")]
        public string Nome { get; set; }

        [Required(ErrorMessage ="informe o seu e-mail")]
        [RegularExpression(".+\\@.+\\..+", ErrorMessage ="Insira um e-mail válido")]
        public string Email { get; set; }

        [Required(ErrorMessage ="Informe a sua senha")]
        [MinLength(8, ErrorMessage ="A senha deverá ter no mínimo 8 caracteres")]
        public string Senha { get; set; }

        public IList<Evento> Eventos { get; set; }

        public string Imagem { get; set; }
    }
}
