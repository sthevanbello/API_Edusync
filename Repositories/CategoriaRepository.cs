using ApiMaisEventos.Interfaces;
using ApiMaisEventos.Models;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace ApiMaisEventos.Repositories
{
    public class CategoriaRepository : ICategoriaRepository
    {

        private readonly string connectionString = @"data source=NOTE_STHEVAN\SQLEXPRESS; User Id=sa; Password=Admin1234; Initial Catalog = Mais_Eventos";
        public ICollection<Categoria> GetCategoriasAll()
        {
            IList<Categoria> listaCategorias = new List<Categoria>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string script = @"SELECT 
                                    * 
                                FROM TB_CATEGORIAS";
                using (SqlCommand cmd = new SqlCommand(script, connection))
                {
                    // Ler todos os itens da consulta com foreach e while
                    cmd.CommandType = CommandType.Text;
                    using (var result = cmd.ExecuteReader())
                    {
                        while (result != null && result.HasRows && result.Read())
                        {
                            listaCategorias.Add(new Categoria
                            {
                                Id = (int)result[0],
                                NomeCategoria = (string)result[1]
                            });
                        }
                    }
                }
            }
            return listaCategorias;
        }

        public Categoria GetCategoriaById(int id)
        {
            Categoria categoria = null;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string script = @"SELECT 
                                    C.Id AS Id,
                                    C.NomeCategoria AS NomeCategoria    
                                FROM TB_CATEGORIAS AS C 
                                WHERE Id = @Id";
                using (SqlCommand cmd = new SqlCommand(script, connection))
                {
                    // Ler todos os itens da consulta com foreach e while
                    cmd.Parameters.Add("Id", SqlDbType.Int).Value = id;
                    cmd.CommandType = CommandType.Text;
                    using (var result = cmd.ExecuteReader())
                    {
                        while (result != null && result.HasRows && result.Read())
                        {
                            categoria = new Categoria
                            {
                                Id = (int)result["Id"],
                                NomeCategoria = result["NomeCategoria"].ToString()
                            };
                        }
                    }
                }
            }
            return categoria;
        }

        public Categoria Insert(Categoria categoria)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string script = @"INSERT INTO TB_CATEGORIAS (NomeCategoria)
                                VALUES
                                    (@NomeCategoria)";

                // Execução no banco
                using (SqlCommand cmd = new SqlCommand(script, connection))
                {
                    // Declarar as variáveis por parâmetros
                    cmd.Parameters.Add("NomeCategoria", SqlDbType.NVarChar).Value = categoria.NomeCategoria;
                    cmd.CommandType = CommandType.Text;
                    cmd.ExecuteNonQuery();
                }
            }
            return categoria;
        }

        public Categoria Update(int id, Categoria categoria)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string script = @"UPDATE TB_CATEGORIAS 
                                    SET NomeCategoria = @NomeCategoria 
                                WHERE id = @id";

                // Execução no banco
                using (SqlCommand cmd = new SqlCommand(script, connection))
                {
                    // Declarar as variáveis por parâmetros
                    cmd.Parameters.Add("Id", SqlDbType.Int).Value = id;
                    cmd.Parameters.Add("NomeCategoria", SqlDbType.NVarChar).Value = categoria.NomeCategoria;
                    cmd.CommandType = CommandType.Text;
                    cmd.ExecuteNonQuery();
                }
            }
            return categoria;
        }
        public bool Delete(int id)
        {
            if (GetCategoriaById(id) is null)
            {
                return false;
            }
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string script = @"DELETE FROM TB_CATEGORIAS WHERE id = @id";

                // Execução no banco
                using (SqlCommand cmd = new SqlCommand(script, connection))
                {
                    // Declarar as variáveis por parâmetros
                    cmd.Parameters.Add("Id", SqlDbType.Int).Value = id;
                    cmd.CommandType = CommandType.Text;
                    cmd.ExecuteNonQuery();
                }
            }
            return true;
        }
    }
}
