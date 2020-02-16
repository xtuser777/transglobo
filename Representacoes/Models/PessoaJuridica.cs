using System;
using System.Collections.Generic;
using System.Data;
using Representacoes.ViewModels;

namespace Representacoes.Models
{
    public class PessoaJuridica : Banco
    {
        private int _id;
        private string _razaoSocial;
        private string _nomeFantasia;
        private string _cnpj;
        private int _contato;

        public int Id { get => _id; set => _id = value; }

        public string RazaoSocial { get => _razaoSocial; set => _razaoSocial = value; }

        public string NomeFantasia { get => _nomeFantasia; set => _nomeFantasia = value; }

        public string Cnpj { get => _cnpj; set => _cnpj = value; }

        public int Contato { get => _contato; set => _contato = value; }

        public PessoaJuridica GetById(int id)
        {
            ComandoSQL.Parameters.Clear();
            ComandoSQL.CommandText = "select id,razao_social,nome_fantasia,cnpj,contato from pessoa_juridica where id = @1;";
            ComandoSQL.Parameters.AddWithValue("@1", id);
            
            DataTable table = ExecutaSelect();
            
            if (table == null || table.Rows.Count <= 0) return null;

            DataRow row = table.Rows[0];
            
            return new PessoaJuridica()
            {
                Id = Convert.ToInt32(row["id"]),
                RazaoSocial = row["razao_social"].ToString(),
                NomeFantasia = row["nome_fantasia"].ToString(),
                Cnpj = row["cnpj"].ToString(),
                Contato = Convert.ToInt32(row["contato"])
            };
        }

        public List<PessoaJuridica> GetAll()
        {
            ComandoSQL.Parameters.Clear();
            ComandoSQL.CommandText = "select id,razao_social,nome_fantasia,cnpj,contato from pessoa_juridica;";
            
            DataTable table = ExecutaSelect();
            
            if (table == null || table.Rows.Count <= 0) return null;

            List<PessoaJuridica> list = new List<PessoaJuridica>();
            for (int i = 0; i < table.Rows.Count; i++)
            {
                DataRow row = table.Rows[i];
                list.Add(new PessoaJuridica()
                {
                    Id = Convert.ToInt32(row["id"]),
                    RazaoSocial = row["razao_social"].ToString(),
                    NomeFantasia = row["nome_fantasia"].ToString(),
                    Cnpj = row["cnpj"].ToString(),
                    Contato = Convert.ToInt32(row["contato"])
                });
            }

            return list;
        }

        public int Gravar()
        {
            ComandoSQL.Parameters.Clear();
            ComandoSQL.CommandText = @"
                insert into pessoa_juridica(razao_social,nome_fantasia,cnpj,contato) values(@1,@2,@3,@4);
                select LAST_INSERT_ID() as last;
            ";
            ComandoSQL.Parameters.AddWithValue("@1", _razaoSocial);
            ComandoSQL.Parameters.AddWithValue("@2", _nomeFantasia);
            ComandoSQL.Parameters.AddWithValue("@3", _cnpj);
            ComandoSQL.Parameters.AddWithValue("@4", _contato);

            DataTable table = ExecutaSelect();

            return table != null && table.Rows.Count > 0 ? Convert.ToInt32(table.Rows[0]["last"]) : -10;
        }

        public int Alterar()
        {
            ComandoSQL.Parameters.Clear();
            ComandoSQL.CommandText = @"
                update pessoa_juridica
                set razao_social = @1,
                nome_fantasia = @2,
                cnpj = @3,
                contato = @4,
                where id = @5;
            ";
            ComandoSQL.Parameters.AddWithValue("@1", _razaoSocial);
            ComandoSQL.Parameters.AddWithValue("@2", _nomeFantasia);
            ComandoSQL.Parameters.AddWithValue("@3", _cnpj);
            ComandoSQL.Parameters.AddWithValue("@4", _contato);
            ComandoSQL.Parameters.AddWithValue("@5", _id);

            return ExecutaComando();
        }

        public int Excluir(int id)
        {
            ComandoSQL.Parameters.Clear();
            ComandoSQL.CommandText = "delete from pessoa_juridica where id = @1;";
            ComandoSQL.Parameters.AddWithValue("@1", id);

            return ExecutaComando();
        }

        public PessoaJuridicaVM ToViewModel()
        {
            return new PessoaJuridicaVM()
            {
                Id = _id,
                RazaoSocial = _razaoSocial,
                NomeFantasia = _nomeFantasia,
                Cpnj = _cnpj,
                Contato = new Contato().GetById(_contato).ToViewModel()
            };
        }
    }
}