using ApiMaisEventos.Models;
using System.Collections.Generic;

namespace ApiMaisEventos.Interfaces
{
    public interface IUsuarioEventoRepository
    {
        public ICollection<UsuarioEvento> GetAll();
        public UsuarioEvento GetById(int id);
        public ICollection<Usuario> GetParticipantesComEventos();
        public ICollection<Evento> GetEventosComParticipantes();
        public UsuarioEvento Insert(UsuarioEvento usuario);
        public UsuarioEvento Update(int id, UsuarioEvento usuario);
        public bool Delete(int id);
    }
}
