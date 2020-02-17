using System;
using System.Collections.Generic;
using System.Data;
using Representacoes.ViewModels;

namespace Representacoes.Models
{
    public class Usuario : Banco
    {
        private int _id;
        private string _login;
        private string _senha;
        private bool _ativo;
        private int _funcionario;
        private int _nivel;

        public int Id { get => _id; set => _id = value; }

        public string Login { get => _login; set => _login = value; }

        public string Senha { get => _senha; set => _senha = value; }

        public bool Ativo { get => _ativo; set => _ativo = value; }

        public int Funcionario { get => _funcionario; set => _funcionario = value; }

        public int Nivel { get => _nivel; set => _nivel = value; }

        public Usuario GetById(int id)
        {
            ComandoSQL.Parameters.Clear();
            ComandoSQL.CommandText = "select id,login,senha,ativo,funcionario,nivel from usuario where id = @1;";
            ComandoSQL.Parameters.AddWithValue("@1", id);
            
            DataTable table = ExecutaSelect();
            
            if (table == null || table.Rows.Count <= 0) return null;

            DataRow row = table.Rows[0];
            
            return new Usuario()
            {
                Id = Convert.ToInt32(row["id"]),
                Login = row["login"].ToString(),
                Senha = row["senha"].ToString(),
                Ativo = Convert.ToBoolean(row["ativo"]),
                Funcionario = Convert.ToInt32(row["funcionario"]),
                Nivel = Convert.ToInt32(row["nivel"])
            };
        }

        public List<Usuario> GetAll()
        {
            ComandoSQL.Parameters.Clear();
            ComandoSQL.CommandText = "select id,login,senha,ativo,funcionario,nivel from usuario;";
            
            DataTable table = ExecutaSelect();
            
            if (table == null || table.Rows.Count <= 0) return null;
            
            List<Usuario> list = new List<Usuario>();
            for (int i = 0; i < table.Rows.Count; i++)
            {
                DataRow row = table.Rows[i];
                list.Add(new Usuario()
                {
                    Id = Convert.ToInt32(row["id"]),
                    Login = row["login"].ToString(),
                    Senha = row["senha"].ToString(),
                    Ativo = Convert.ToBoolean(row["ativo"]),
                    Funcionario = Convert.ToInt32(row["funcionario"]),
                    Nivel = Convert.ToInt32(row["nivel"])
                });
            }

            return list;
        }

        public Usuario Autenticar(string login, string senha)
        {
            ComandoSQL.Parameters.Clear();
            ComandoSQL.CommandText = "select id,login,senha,ativo,funcionario,nivel from usuario where login = @1 and senha = @2 and ativo = true;";
            ComandoSQL.Parameters.AddWithValue("@1", login);
            ComandoSQL.Parameters.AddWithValue("@2", senha);
            
            DataTable table = ExecutaSelect();
            
            if (table == null || table.Rows.Count <= 0) return null;

            DataRow row = table.Rows[0];
            
            return new Usuario()
            {
                Id = Convert.ToInt32(row["id"]),
                Login = row["login"].ToString(),
                Senha = row["senha"].ToString(),
                Ativo = Convert.ToBoolean(row["ativo"]),
                Funcionario = Convert.ToInt32(row["funcionario"]),
                Nivel = Convert.ToInt32(row["nivel"])
            };
        }

        public int Gravar()
        {
            ComandoSQL.Parameters.Clear();
            ComandoSQL.CommandText = @"
                insert into usuario(login,senha,ativo,funcionario,nivel) values(@1,@2,@3,@4,@5);
                select LAST_INSERT_ID() as last;
            ";
            ComandoSQL.Parameters.AddWithValue("@1", _login);
            ComandoSQL.Parameters.AddWithValue("@2", _senha);
            ComandoSQL.Parameters.AddWithValue("@3", _ativo);
            ComandoSQL.Parameters.AddWithValue("@4", _funcionario);
            ComandoSQL.Parameters.AddWithValue("@5", _nivel);

            DataTable table = ExecutaSelect();

            return table != null && table.Rows.Count > 0 ? Convert.ToInt32(table.Rows[0]["last"]) : -10;
        }

        public int Alterar()
        {
            ComandoSQL.Parameters.Clear();
            ComandoSQL.CommandText = @"
                update usuario
                set login = @1,
                senha = @2,
                ativo = @3,
                funcionario = @4,
                nivel = @5, 
                where id = @6;
            ";
            ComandoSQL.Parameters.AddWithValue("@1", _login);
            ComandoSQL.Parameters.AddWithValue("@2", _senha);
            ComandoSQL.Parameters.AddWithValue("@3", _ativo);
            ComandoSQL.Parameters.AddWithValue("@4", _funcionario);
            ComandoSQL.Parameters.AddWithValue("@5", _nivel);
            ComandoSQL.Parameters.AddWithValue("@6", _id);

            return ExecutaComando();
        }

        public bool VerificarLogin(string login)
        {
            return LoginCount(login) > 0;
        }

        public int Excluir(int id)
        {
            ComandoSQL.Parameters.Clear();
            ComandoSQL.CommandText = "delete from usuario where id = @1;";
            ComandoSQL.Parameters.AddWithValue("@1", id);

            return ExecutaComando();
        }
        
        public int LoginCount(string login)
        {
            int count = 0;
            ComandoSQL.Parameters.Clear();
            ComandoSQL.CommandText = @"select count(id) as logins from usuario where login = @login;";
            ComandoSQL.Parameters.AddWithValue("@login", login);
            DataTable dt = ExecutaSelect();
            if (dt != null && dt.Rows.Count > 0)
            {
                count = Convert.ToInt32(dt.Rows[0]["logins"]);
            }
            return count;
        }

        public int AdminCount()
        {
            ComandoSQL.Parameters.Clear();
            ComandoSQL.CommandText = @"select count(id) as admins 
                                        from usuario 
                                        inner join funcionario on funcionario.id = usuario.funcionario 
                                        where usuario.nivel = 1 
                                        and funcionario.demissao is null;";
            DataTable dt = ExecutaSelect();
            if (dt != null && dt.Rows.Count > 0)
            {
                return Convert.ToInt32(dt.Rows[0]["admins"]);
            }

            return 0;
        }

        public bool IsLastAdmin()
        {
            return (AdminCount() == 1);
        }

        public UsuarioVM ToViewModel()
        {
            return new UsuarioVM()
            {
                Id = _id,
                Login = _login,
                Senha = _senha,
                Ativo = _ativo,
                Funcionario = new Funcionario().GetById(_funcionario).ToViewModel(),
                Nivel = new Nivel().GetById(_nivel).ToViewModel()
            };
        }
    }
}