using ApiMaisEventos.Interfaces;
using ApiMaisEventos.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace ApiMaisEventos.Repositories
{
    public class EventoRepository : IEventoRepository
    {
        private readonly string connectionString = @"data source=NOTE_STHEVAN\SQLEXPRESS; User Id=sa; Password=Admin1234; Initial Catalog = Mais_Eventos";
        

        public ICollection<Evento> GetEventosAll()
        {
            IList<Evento> listaEventos = new List<Evento>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                //string script = @"SELECT E.Id as Id_Evento, E.DataHora as Data_Hora_Evento, E.Ativo AS Evento_Ativo, 
                //                    E.Preco as Preco_Evento, E.CategoriaId as Id_Categoria
                //                    FROM TB_EVENTOS AS E";

                string script = @"SELECT 
                                        E.Id AS 'Id_Evento',    
                                        E.DataHora AS 'Data_Hora_Evento', 
                                        E.Ativo AS 'Evento_Ativo', 
                                        E.Preco AS 'Preco_Evento',
                                        E.CategoriaId AS 'Id_Categoria', 
                                        C.NomeCategoria AS 'Nome_Categoria'
                                    FROM TB_EVENTOS AS E
                                    JOIN TB_CATEGORIAS AS C ON E.CategoriaId = C.Id";

                using (SqlCommand cmd = new SqlCommand(script, connection))
                {
                    // Ler todos os itens da consulta com while
                    cmd.CommandType = CommandType.Text;
                    using (var result = cmd.ExecuteReader())
                    {
                        while (result != null && result.HasRows && result.Read())
                        {
                            listaEventos.Add(new Evento
                            {
                                Id = (int)result["Id_Evento"],
                                DataHora = Convert.ToDateTime(result["Data_Hora_Evento"]),
                                Ativo = (bool)result["Evento_Ativo"],
                                Preco = (decimal)result["Preco_Evento"],
                                Categoria = new Categoria
                                {
                                    Id = (int)result["Id_Categoria"],
                                    NomeCategoria = result["Nome_Categoria"].ToString()
                                }

                            });
                        }
                    }

                }
            }
            return listaEventos;
        }

        public Evento GetEventoById(int id)
        {
            Evento evento = null;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string script = @"SELECT 
                                        E.Id AS 'Id_Evento',    
                                        E.DataHora AS 'Data_Hora_Evento', 
                                        E.Ativo AS 'Evento_Ativo', 
                                        E.Preco AS 'Preco_Evento',
                                        E.CategoriaId AS 'Id_Categoria', 
                                        C.NomeCategoria AS 'Nome_Categoria'
                                    FROM TB_EVENTOS AS E
                                    JOIN TB_CATEGORIAS AS C ON E.CategoriaId = C.Id
                                    WHERE E.Id = @Id" ;
                using (SqlCommand cmd = new SqlCommand(script, connection))
                {
                    // Ler todos os itens da consulta com foreach e while
                    cmd.Parameters.Add("Id", SqlDbType.Int).Value = id;
                    cmd.CommandType = CommandType.Text;
                    using (var result = cmd.ExecuteReader())
                    {
                        if (result != null && result.HasRows && result.Read())
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
                                }
                            };
                        }
                    }
                }
            }
            return evento;
        }

        public ICollection<Evento> GetEventosComUsuarios()
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

        public Evento InsertEvento(Evento evento)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string script = @"INSERT INTO TB_EVENTOS (DataHora, Ativo, Preco, CategoriaId)
                                    VALUES
                                    (@DataHora, @Ativo, @Preco, @CategoriaId)";

                // Execução no banco
                using (SqlCommand cmd = new SqlCommand(script, connection))
                {
                    // Declarar as variáveis por parâmetros
                    cmd.Parameters.Add("DataHora", SqlDbType.DateTime).Value = evento.DataHora;
                    cmd.Parameters.Add("Ativo", SqlDbType.Bit).Value = evento.Ativo;
                    cmd.Parameters.Add("Preco", SqlDbType.Decimal).Value = evento.Preco;
                    cmd.Parameters.Add("CategoriaId", SqlDbType.Int).Value = evento?.Categoria?.Id ?? 0;
                    cmd.CommandType = CommandType.Text;
                    cmd.ExecuteNonQuery();
                }

            }
            return evento;
        }

        public bool UpdateEvento(int id, Evento evento)
        {
            if (GetEventoById(id) is null)
            {
                return false;
            }

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string script = @"UPDATE TB_EVENTOS SET DataHora = @DataHora, Ativo =  @Ativo, Preco = @Preco, CategoriaId = @CategoriaId WHERE Id = @id";

                // Execução no banco
                using (SqlCommand cmd = new SqlCommand(script, connection))
                {
                    // Declarar as variáveis por parâmetros
                    cmd.Parameters.Add("Id", SqlDbType.Int).Value = id;
                    cmd.Parameters.Add("DataHora", SqlDbType.NVarChar).Value = evento.DataHora;
                    cmd.Parameters.Add("Ativo", SqlDbType.Bit).Value = evento.Ativo;
                    cmd.Parameters.Add("Preco", SqlDbType.Decimal).Value = evento.Preco;
                    cmd.Parameters.Add("CategoriaId", SqlDbType.Int).Value = evento.Categoria.Id;
                    cmd.CommandType = CommandType.Text;
                    cmd.ExecuteNonQuery();
                }
            }
            return true;
        }
        public bool DeleteEvento(int id)
        {
            if (GetEventoById(id) is null)
            {
                return false;
            }
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string script = @"DELETE FROM TB_EVENTOS WHERE id = @id";

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
