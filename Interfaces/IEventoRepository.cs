using ApiMaisEventos.Models;
using System.Collections.Generic;

namespace ApiMaisEventos.Interfaces
{
    public interface IEventoRepository
    {
        public ICollection<Evento> GetEventosAll();
        public Evento GetEventoById(int id);
        public ICollection<Evento> GetEventosComUsuarios();
        public Evento InsertEvento(Evento usuario);
        public bool UpdateEvento(int id, Evento usuario);
        public bool DeleteEvento(int id);
    }
}
