using ApiMaisEventos.Models;
using System.Collections.Generic;

namespace ApiMaisEventos.Interfaces
{
    /// <summary>
    /// Interface para repositório de usuário
    /// </summary>
    public interface IUsuarioRepository
    {
        public ICollection<Usuario> GetAll();
        public Usuario GetById(int id);
        public ICollection<Usuario> GetUsuariosComEventos();
        public Usuario Insert(Usuario usuario);
        public Usuario InsertUsuarioComImagem(Usuario usuario);
        public bool Update(int id, Usuario usuario);
        public bool Delete(int id);
    }
}
