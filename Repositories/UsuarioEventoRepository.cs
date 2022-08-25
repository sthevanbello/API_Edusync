using ApiMaisEventos.Interfaces;
using ApiMaisEventos.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace ApiMaisEventos.Repositories
{
    public class UsuarioEventoRepository : IUsuarioEventoRepository
    {
        private readonly string connectionString = @"data source=NOTE_STHEVAN\SQLEXPRESS; User Id=sa; Password=Admin1234; Initial Catalog = Mais_Eventos";

        public UsuarioEvento Insert(UsuarioEvento usuarioEvento)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string script = @"INSERT INTO RL_USUARIO_EVENTO (UsuarioId, EventoId)
                                    VALUES
                                    (@UsuarioId, @EventoId)";
                // Execução no banco
                using (SqlCommand cmd = new SqlCommand(script, connection))
                {
                    // Declarar as variáveis por parâmetros
                    cmd.Parameters.Add("UsuarioId", SqlDbType.NVarChar).Value = usuarioEvento.UsuarioId;
                    cmd.Parameters.Add("EventoId", SqlDbType.NVarChar).Value = usuarioEvento.EventoId;
                    cmd.CommandType = CommandType.Text;
                    cmd.ExecuteNonQuery();
                }
            }
            return usuarioEvento;
        }
        public ICollection<UsuarioEvento> GetAll()
        {
            List<UsuarioEvento> listaUsuarioEventos = new List<UsuarioEvento>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string script = @"SELECT UE.Id, UE.UsuarioId, UE.EventoId FROM RL_USUARIO_EVENTO AS UE";

                // Execução no banco
                using (SqlCommand cmd = new SqlCommand(script, connection))
                {
                    // Declarar as variáveis por parâmetros
                    cmd.CommandType = CommandType.Text;
                    using (var result = cmd.ExecuteReader())
                    {
                        while (result != null && result.HasRows && result.Read())
                        {
                            listaUsuarioEventos.Add(new UsuarioEvento
                            {
                                Id = (int)result["Id"],
                                UsuarioId = (int)result["UsuarioId"],
                                EventoId = (int)result["EventoId"]
                            });
                        }
                    }

                }
            }
            return listaUsuarioEventos;
        }

        public UsuarioEvento GetById(int id)
        {
            UsuarioEvento usuarioEvento = null;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string script = @"SELECT UE.UsuarioId, UE.EventoId FROM RL_USUARIO_EVENTO AS UE";

                // Execução no banco
                using (SqlCommand cmd = new SqlCommand(script, connection))
                {
                    // Declarar as variáveis por parâmetros
                    cmd.CommandType = CommandType.Text;
                    using (var result = cmd.ExecuteReader())
                    {
                        while (result != null && result.HasRows && result.Read())
                        {
                            usuarioEvento = new UsuarioEvento
                            {
                                EventoId = (int)result["UsuarioId"],
                                UsuarioId = (int)result["EventoId"]
                            };
                        }
                    }

                }
            }
            return usuarioEvento;
        }

        public ICollection<Usuario> GetParticipantesComEventos()
        {
            IList<Usuario> listaUsuarios = new List<Usuario>();
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

                            if (!listaUsuarios.Any(x => x.Id == (int)result["Id_Usuario"]))
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

                                listaUsuarios.Add(usuario);
                            }
                            else if ((evento?.Id ?? 0) > 0)
                            {
                                listaUsuarios.FirstOrDefault(x => x.Id == (int)result["Id_Usuario"]).Eventos.Add(evento);
                            }
                        }
                    }

                }
            }
            return listaUsuarios;
        }

        public UsuarioEvento Update(int id, UsuarioEvento usuarioEvento)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string script = @"UPDATE RL_USUARIO_EVENTO SET UsuarioId = @UsuarioId, EventoId = @EventoId WHERE Id = @Id";

                // Execução no banco
                using (SqlCommand cmd = new SqlCommand(script, connection))
                {
                    // Declarar as variáveis por parâmetros
                    cmd.Parameters.Add("Id", SqlDbType.Int).Value = id;
                    cmd.Parameters.Add("UsuarioId", SqlDbType.NVarChar).Value = usuarioEvento.UsuarioId;
                    cmd.Parameters.Add("EventoId", SqlDbType.NVarChar).Value = usuarioEvento.EventoId;
                    cmd.CommandType = CommandType.Text;
                    cmd.ExecuteNonQuery();
                }
            }
            return usuarioEvento;
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

                string script = @"DELETE FROM RL_USUARIO_EVENTO WHERE Id = @Id";

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



        public ICollection<Evento> GetEventosComParticipantes()
        {
            IList<Evento> listaEventos = new List<Evento>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string script = @"SELECT 
	                                    E.Id AS 'Id_Evento', 
	                                    E.DataHora AS 'Data_Hora_Evento', 
	                                    E.Ativo AS 'Evento_Ativo', 
	                                    E.Preco AS 'Preco_Evento',
	                                    E.CategoriaId AS 'Id_Categoria', 
	                                    C.NomeCategoria AS 'Nome_Categoria',
	                                    U.Id AS 'Id_Usuario', 
	                                    U.Nome AS 'Nome_Usuario', 
	                                    U.Email AS 'Email_Usuario'
                                    FROM TB_EVENTOS AS E
                                    INNER JOIN TB_CATEGORIAS AS C ON E.CategoriaId = C.Id
                                    LEFT JOIN RL_USUARIO_EVENTO AS UE ON UE.EventoId = E.Id
                                    LEFT JOIN TB_USUARIOS AS U ON U.Id = UE.UsuarioId";

                using (SqlCommand cmd = new SqlCommand(script, connection))
                {
                    // Ler todos os itens da consulta com while
                    cmd.CommandType = CommandType.Text;
                    using (var result = cmd.ExecuteReader())
                    {
                        while (result != null && result.HasRows && result.Read())
                        {
                            var usuario = new Usuario();
                            if (!string.IsNullOrEmpty(result["Id_Usuario"].ToString()))
                            {
                                usuario = new Usuario
                                {
                                    Id = (int)result["Id_Usuario"],
                                    Nome = result["Nome_Usuario"].ToString(),
                                    Email = result["Email_Usuario"].ToString()
                                };
                            }

                            if (!listaEventos.Any(x => x.Id == (int)result["Id_Evento"]))
                            {
                                var evento = new Evento
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
                                    Usuarios = new List<Usuario>()
                                };

                                if ((usuario?.Id ?? 0) > 0)
                                {
                                    evento.Usuarios.Add(usuario);
                                }

                                listaEventos.Add(evento);
                            }
                            else if ((usuario?.Id ?? 0) > 0)
                            {
                                listaEventos.FirstOrDefault(x => x.Id == (int)result["Id_Evento"]).Usuarios.Add(usuario);
                            }
                        }
                    }

                }
            }
            return listaEventos;
        }
    }
}
