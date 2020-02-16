using System;
using System.Collections.Generic;
using System.Data;
using Representacoes.ViewModels;

namespace Representacoes.Models
{
    public class Contato : Banco
    {
        private int _id;
        private string _telefone;
        private string _celular;
        private string _email;
        private int _endereco;

        public int Id { get => _id; set => _id = value; }

        public string Telefone { get => _telefone; set => _telefone = value; }

        public string Celular { get => _celular; set => _celular = value; }

        public string Email { get => _email; set => _email = value; }

        public int Endereco { get => _endereco; set => _endereco = value; }

        public Contato GetById(int id)
        {
            ComandoSQL.Parameters.Clear();
            ComandoSQL.CommandText = "select id,telefone,celular,email,endereco from contato where id = @1;";
            ComandoSQL.Parameters.AddWithValue("@1", id);

            DataTable table = ExecutaSelect();
            
            if (table == null || table.Rows.Count <= 0) return null;

            DataRow row = table.Rows[0];
            
            return new Contato()
            {
                Id = Convert.ToInt32(row["id"]),
                Telefone = row["telefone"].ToString(),
                Celular = row["celular"].ToString(),
                Email = row["email"].ToString(),
                Endereco = Convert.ToInt32(row["endereco"])
            };
        }
        
        public List<Contato> GetAll()
        {
            ComandoSQL.Parameters.Clear();
            ComandoSQL.CommandText = "select id,telefone,celular,email,endereco from contato;";
            
            DataTable table = ExecutaSelect();
            
            if (table == null || table.Rows.Count <= 0) return null;
            
            List<Contato> list = new List<Contato>();
            for (int i = 0; i < table.Rows.Count; i++)
            {
                DataRow row = table.Rows[i];
                list.Add(new Contato()
                {
                    Id = Convert.ToInt32(row["id"]),
                    Telefone = row["telefone"].ToString(),
                    Celular = row["celular"].ToString(),
                    Email = row["email"].ToString(),
                    Endereco = Convert.ToInt32(row["endereco"])
                });
            }

            return list;
        }

        public int Gravar()
        {
            ComandoSQL.Parameters.Clear();
            ComandoSQL.CommandText = @"
                insert into contato(telefone,celular,email,endereco) values(@1,@2,@3,@4);
                select LAST_INSERT_ID() as last;
            ";
            ComandoSQL.Parameters.AddWithValue("@1", _telefone);
            ComandoSQL.Parameters.AddWithValue("@2", _celular);
            ComandoSQL.Parameters.AddWithValue("@3", _email);
            ComandoSQL.Parameters.AddWithValue("@4", _endereco);

            DataTable table = ExecutaSelect();

            return table != null && table.Rows.Count > 0 ? Convert.ToInt32(table.Rows[0]["last"]) : -10;
        }

        public int Alterar()
        {
            ComandoSQL.Parameters.Clear();
            ComandoSQL.CommandText = @"
                update contato
                set telefone = @1,
                celular = @2,
                email = @3,
                endereco = @4,
                where id = @5;
            ";
            ComandoSQL.Parameters.AddWithValue("@1", _telefone);
            ComandoSQL.Parameters.AddWithValue("@2", _celular);
            ComandoSQL.Parameters.AddWithValue("@3", _email);
            ComandoSQL.Parameters.AddWithValue("@4", _endereco);
            ComandoSQL.Parameters.AddWithValue("@5", _id);

            return ExecutaComando();
        }

        public int Excluir(int id)
        {
            ComandoSQL.Parameters.Clear();
            ComandoSQL.CommandText = "delete from contato where id = @1;";
            ComandoSQL.Parameters.AddWithValue("@1", id);

            return ExecutaComando();
        }

        public ContatoVM ToViewModel()
        {
            return new ContatoVM()
            {
                Id = _id,
                Telefone = _telefone,
                Celular = _celular,
                Email = _email,
                Endereco = new Endereco().GetById(_endereco).ToViewModel()
            };
        }
    }
}