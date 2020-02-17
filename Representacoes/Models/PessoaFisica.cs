using System;
using System.Collections.Generic;
using System.Data;
using Representacoes.ViewModels;

namespace Representacoes.Models
{
    public class PessoaFisica : Banco
    {
        private int _id;
        private string _nome;
        private string _rg;
        private string _cpf;
        private DateTime _nascimento;
        private int _contato;

        public int Id { get => _id; set => _id = value; }

        public string Nome { get => _nome; set => _nome = value; }

        public string Rg { get => _rg; set => _rg = value; }

        public string Cpf { get => _cpf; set => _cpf = value; }

        public DateTime Nascimento { get => _nascimento; set => _nascimento = value; }

        public int Contato { get => _contato; set => _contato = value; }

        public PessoaFisica GetById(int id)
        {
            ComandoSQL.Parameters.Clear();
            ComandoSQL.CommandText = "select id,nome,rg,cpf,nascimento,contato from pessoa_fisica where id = @1;";
            ComandoSQL.Parameters.AddWithValue("@1", id);
            
            DataTable table = ExecutaSelect();
            
            if (table == null || table.Rows.Count <= 0) return null;

            DataRow row = table.Rows[0];
            
            return new PessoaFisica()
            {
                Id = Convert.ToInt32(row["id"]),
                Nome = row["nome"].ToString(),
                Rg = row["rg"].ToString(),
                Cpf = row["cpf"].ToString(),
                Nascimento = Convert.ToDateTime(row["nascimento"]),
                Contato = Convert.ToInt32(row["contato"])
            };
        }

        public List<PessoaFisica> GetAll()
        {
            ComandoSQL.Parameters.Clear();
            ComandoSQL.CommandText = "select id,nome,rg,cpf,nascimento,contato from pessoa_fisica;";
            
            DataTable table = ExecutaSelect();
            
            if (table == null || table.Rows.Count <= 0) return null;
            
            List<PessoaFisica> list = new List<PessoaFisica>();
            for (int i = 0; i < table.Rows.Count; i++)
            {
                DataRow row = table.Rows[i];
                list.Add(new PessoaFisica()
                {
                    Id = Convert.ToInt32(row["id"]),
                    Nome = row["nome"].ToString(),
                    Rg = row["rg"].ToString(),
                    Cpf = row["cpf"].ToString(),
                    Nascimento = Convert.ToDateTime(row["nascimento"]),
                    Contato = Convert.ToInt32(row["contato"])
                });
            }

            return list;
        }

        public int CountCpf(string cpf)
        {
            ComandoSQL.Parameters.Clear();
            ComandoSQL.CommandText = @"select count(id) as cnt from pessoa_fisica where cpf = @cpf;";
            ComandoSQL.Parameters.AddWithValue("@cpf", cpf);

            DataTable dt = ExecutaSelect();

            return dt != null && dt.Rows.Count > 0 ? Convert.ToInt32(dt.Rows[0]["cnt"]) : -10;
        }

        public bool VerifyCpf(string cpf)
        {
            return !string.IsNullOrEmpty(cpf) && CountCpf(cpf) > 0;
        }

        public int Gravar()
        {
            ComandoSQL.Parameters.Clear();
            ComandoSQL.CommandText = @"
                insert into pessoa_fisica(nome,rg,cpf,nascimento,contato) values(@1,@2,@3,@4,@5);
                select LAST_INSERT_ID() as last;
            ";
            ComandoSQL.Parameters.AddWithValue("@1", _nome);
            ComandoSQL.Parameters.AddWithValue("@2", _rg);
            ComandoSQL.Parameters.AddWithValue("@3", _cpf);
            ComandoSQL.Parameters.AddWithValue("@4", _nascimento);
            ComandoSQL.Parameters.AddWithValue("@5", _contato);

            DataTable table = ExecutaSelect();

            return table != null && table.Rows.Count > 0 ? Convert.ToInt32(table.Rows[0]["last"]) : -10;
        }

        public int Alterar()
        {
            ComandoSQL.Parameters.Clear();
            ComandoSQL.CommandText = @"
                update pessoa_fisica
                set nome = @1,
                rg = @2,
                cpf = @3,
                nascimento = @4,
                contato = @5,
                where id = @6;
            ";
            ComandoSQL.Parameters.AddWithValue("@1", _nome);
            ComandoSQL.Parameters.AddWithValue("@2", _rg);
            ComandoSQL.Parameters.AddWithValue("@3", _cpf);
            ComandoSQL.Parameters.AddWithValue("@4", _nascimento);
            ComandoSQL.Parameters.AddWithValue("@5", _contato);
            ComandoSQL.Parameters.AddWithValue("@6", _id);

            return ExecutaComando();
        }

        public int Excluir(int id)
        {
            ComandoSQL.Parameters.Clear();
            ComandoSQL.CommandText = "delete from pessoa_fisica where id = @1;";
            ComandoSQL.Parameters.AddWithValue("@1", id);

            return ExecutaComando();
        }

        public PessoaFisicaVM ToViewModel()
        {
            return new PessoaFisicaVM()
            {
                Id = _id,
                Nome = _nome,
                Rg = _rg,
                Cpf = _cpf,
                Nascimento = _nascimento,
                Contato = new Contato().GetById(_contato).ToViewModel()
            };
        }
    }
}