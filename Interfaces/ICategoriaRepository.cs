using ApiMaisEventos.Models;
using System.Collections.Generic;

namespace ApiMaisEventos.Interfaces
{
    public interface ICategoriaRepository
    {
        public ICollection<Categoria> GetCategoriasAll();
        public Categoria GetCategoriaById(int id);
        public Categoria Insert(Categoria categoria);
        public bool Update(int id, Categoria categoria);
        public bool Delete(int id);
    }
}
