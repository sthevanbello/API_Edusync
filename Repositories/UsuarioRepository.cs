using ApiMaisEventos.Interfaces;
using ApiMaisEventos.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace ApiMaisEventos.Repositories
{
    public class UsuarioRepository : IUsuarioRepository
    {
        // Criar string de conexão com o banco de dados
        private readonly string connectionString = @"data source=NOTE_STHEVAN\SQLEXPRESS; User Id=sa; Password=Admin1234; Initial Catalog = Mais_Eventos";
        

        public ICollection<Usuario> GetAll()
        {
            IList<Usuario> listaUsuarios = new List<Usuario>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string script = @"SELECT 
                                        * 
                                    FROM TB_USUARIOS";
                using (SqlCommand cmd = new SqlCommand(script, connection))
                {
                    // Ler todos os itens da consulta com foreach e while
                    cmd.CommandType = CommandType.Text;
                    using (var result = cmd.ExecuteReader())
                    {
                        while (result.Read())
                        {
                            listaUsuarios.Add(new Usuario
                            {
                                Id = (int)result[0],
                                Nome = (string)result[1],
                                Email = (string)result[2],
                                Senha = (string)result[3],
                                Imagem = result[4].ToString()
                            });
                        }
                    }

                }
            }
            return listaUsuarios;
        }

        public Usuario GetById(int id)
        {
            Usuario usuario = null;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string script = @"SELECT 
                                        * 
                                    FROM TB_USUARIOS WHERE Id = @id";
                using (SqlCommand cmd = new SqlCommand(script, connection))
                {
                    // Ler todos os itens da consulta com foreach e while
                    cmd.Parameters.Add("Id", SqlDbType.Int).Value = id;
                    cmd.CommandType = CommandType.Text;
                    using (var result = cmd.ExecuteReader())
                    {
                        while (result.Read())
                        {
                            usuario = new Usuario
                            {
                                Id = (int)result[0],
                                Nome = (string)result[1],
                                Email = (string)result[2],
                                Senha = (string)result[3]
                            };
                        }
                    }

                }
            }
            return usuario;
        }

        public ICollection<Usuario> GetUsuariosComEventos()
        {
            IList<Usuario> listausuarios = new List<Usuario>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string script = @"SELECT 
	                                    U.Id AS 'Id_Usuario', 
	                                    U.Nome AS 'Nome_Usuario', 
	                                    U.Email AS 'Email_Usuario',
                                        E.CategoriaId AS 'Id_Categoria', 
	                                    C.NomeCategoria AS 'Nome_Categoria',
                                        E.Id AS 'Id_Evento', 
	                                    E.DataHora AS 'Data_Hora_Evento',
	                                    E.Preco AS 'Preco_Evento',
	                                    E.Ativo AS 'Evento_Ativo'
                                    FROM TB_USUARIOS AS U
                                    INNER JOIN RL_USUARIO_EVENTO AS UE ON U.Id = UE.UsuarioId
                                    INNER JOIN TB_EVENTOS AS E ON E.Id = UE.EventoId
                                    INNER JOIN TB_CATEGORIAS AS C ON E.CategoriaId = C.Id";

                using (SqlCommand cmd = new SqlCommand(script, connection))
                {
                    // Ler todos os itens da consulta com while
                    cmd.CommandType = CommandType.Text;
                    using (var result = cmd.ExecuteReader())
                    {
                        while (result != null && result.HasRows && result.Read())
                        {
                            var evento = new Evento();
                            if (!string.IsNullOrEmpty(result["Id_Evento"].ToString()))
                            {
                                evento = new Evento
                                {
                                    Id = (int)result["Id_Evento"],
                                    DataHora = Convert.ToDateTime(result["Data_Hora_Evento"]),
                                    Ativo = (bool)result["Evento_Ativo"],
                                    Preco = (decimal)result["Preco_Evento"],
                                    Categoria = new Categoria
                                    {
                                        Id = (int)result["Id_Categoria"],
                                        NomeCategoria = result["Nome_Categoria"].ToString()
                                    },

                                };
                            }

                            if (!listausuarios.Any(x => x.Id == (int)result["Id_Usuario"]))
                            {
                                var usuario = new Usuario
                                {
                                    Id = (int)result["Id_Usuario"],
                                    Nome = result["Nome_Usuario"].ToString(),
                                    Email = result["Email_Usuario"].ToString(),
                                    Eventos = new List<Evento>()
                                };

                                if ((usuario?.Id ?? 0) > 0)
                                {
                                    usuario.Eventos.Add(evento);
                                }

                                listausuarios.Add(usuario);
                            }
                            else if ((evento?.Id ?? 0) > 0)
                            {
                                listausuarios.FirstOrDefault(x => x.Id == (int)result["Id_Usuario"]).Eventos.Add(evento);
                            }
                        }
                    }

                }
            }
            return listausuarios;
        }

        public Usuario Insert(Usuario usuario)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string script = @"INSERT INTO TB_USUARIOS 
                                        (Nome, Email, Senha)
                                    VALUES
                                        (@Nome, @Email, @Senha)";

                // Execução no banco
                using (SqlCommand cmd = new SqlCommand(script, connection))
                {
                    // Declarar as variáveis por parâmetros
                    cmd.Parameters.Add("Nome", SqlDbType.NVarChar).Value = usuario.Nome;
                    cmd.Parameters.Add("Email", SqlDbType.NVarChar).Value = usuario.Email;
                    cmd.Parameters.Add("Senha", SqlDbType.NVarChar).Value = usuario.Senha;
                    cmd.CommandType = CommandType.Text;
                    cmd.ExecuteNonQuery();
                }
            }
            return usuario;
        }

        public Usuario InsertUsuarioComImagem(Usuario usuario)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string script = @"INSERT INTO TB_USUARIOS 
                                        (Nome, Email, Senha, Imagem)
                                    VALUES
                                        (@Nome, @Email, @Senha, @Imagem)";

                // Execução no banco
                using (SqlCommand cmd = new SqlCommand(script, connection))
                {
                    // Declarar as variáveis por parâmetros
                    cmd.Parameters.Add("Nome", SqlDbType.NVarChar).Value = usuario.Nome;
                    cmd.Parameters.Add("Email", SqlDbType.NVarChar).Value = usuario.Email;
                    cmd.Parameters.Add("Senha", SqlDbType.NVarChar).Value = usuario.Senha;
                    cmd.Parameters.Add("Imagem", SqlDbType.NVarChar).Value = usuario.Imagem;
                    cmd.CommandType = CommandType.Text;
                    cmd.ExecuteNonQuery();
                }
            }
            return usuario;
        }

        public bool Update(int id, Usuario usuario)
        {
            if (GetById(id) is null)
            {
                return false;
            }
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string script = @"UPDATE TB_USUARIOS 
                                        SET Nome = @Nome, Email = @Email, Senha = @Senha 
                                    WHERE Id = @id";

                // Execução no banco
                using (SqlCommand cmd = new SqlCommand(script, connection))
                {
                    // Declarar as variáveis por parâmetros
                    cmd.Parameters.Add("Id", SqlDbType.NVarChar).Value = id;
                    cmd.Parameters.Add("Nome", SqlDbType.NVarChar).Value = usuario.Nome;
                    cmd.Parameters.Add("Email", SqlDbType.NVarChar).Value = usuario.Email;
                    cmd.Parameters.Add("Senha", SqlDbType.NVarChar).Value = usuario.Senha;
                    cmd.CommandType = CommandType.Text;
                    cmd.ExecuteNonQuery();
                    usuario.Id = id;
                }
            }
            return true;
        }

        public bool Delete(int id)
        {
            if (GetById(id) is null)
            {
                return false;
            }
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string script = @"DELETE FROM TB_USUARIOS WHERE Id = @id";

                // Execução no banco
                using (SqlCommand cmd = new SqlCommand(script, connection))
                {

                    // Declarar as variáveis por parâmetros
                    cmd.Parameters.Add("Id", SqlDbType.NVarChar).Value = id;
                    cmd.CommandType = CommandType.Text;
                    cmd.ExecuteNonQuery();
                }
            }
            return true;
        }
    }
}
