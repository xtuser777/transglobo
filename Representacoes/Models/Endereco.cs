using System;
using System.Collections.Generic;
using System.Data;
using Representacoes.ViewModels;

namespace Representacoes.Models
{
    public class Endereco : Banco
    {
        private int _id;
        private string _rua;
        private string _numero;
        private string _bairro;
        private string _complemento;
        private string _cep;
        private int _cidade;

        public int Id { get => _id; set => _id = value; }

        public string Rua { get => _rua; set => _rua = value; }

        public string Numero { get => _numero; set => _numero = value; }

        public string Bairro { get => _bairro; set => _bairro = value; }

        public string Complemento { get => _complemento; set => _complemento = value; }

        public string Cep { get => _cep; set => _cep = value; }

        public int Cidade { get => _cidade; set => _cidade = value; }

        public Endereco GetById(int id)
        {
            ComandoSQL.Parameters.Clear();
            ComandoSQL.CommandText = "select id,rua,numero,bairro,complemento,cep,cidade from endereco where id = @id;";
            ComandoSQL.Parameters.AddWithValue("@id", id);

            DataTable table = ExecutaSelect();
            
            if (table == null || table.Rows.Count <= 0) return null;

            DataRow row = table.Rows[0];
            
            return new Endereco()
            {
                Id = Convert.ToInt32(row["id"]),
                Rua = row["rua"].ToString(),
                Numero = row["numero"].ToString(),
                Bairro = row["bairro"].ToString(),
                Complemento = row["complemento"].ToString(),
                Cep = row["cep"].ToString(),
                Cidade = Convert.ToInt32(row["cidade"])
            };
        }

        public List<Endereco> GetAll()
        {
            ComandoSQL.Parameters.Clear();
            ComandoSQL.CommandText = "select id,rua,numero,bairro,complemento,cep,cidade from endereco;";

            DataTable table = ExecutaSelect();
            
            if (table == null || table.Rows.Count <= 0) return null;
            
            List<Endereco> list = new List<Endereco>();
            for (int i = 0; i < table.Rows.Count; i++)
            {
                DataRow row = table.Rows[i];
                list.Add(new Endereco()
                {
                    Id = Convert.ToInt32(row["id"]),
                    Rua = row["rua"].ToString(),
                    Numero = row["numero"].ToString(),
                    Bairro = row["bairro"].ToString(),
                    Complemento = row["complemento"].ToString(),
                    Cep = row["cep"].ToString(),
                    Cidade = Convert.ToInt32(row["cidade"])
                });
            }

            return list;
        }

        public int Gravar()
        {
            ComandoSQL.Parameters.Clear();
            ComandoSQL.CommandText = @"
                insert into endereco(rua,numero,bairro,complemento,cep,cidade) values(@1,@2,@3,@4,@5,@6);
                select LAST_INSERT_ID() as last;
            ";
            ComandoSQL.Parameters.AddWithValue("@1", _rua);
            ComandoSQL.Parameters.AddWithValue("@2", _numero);
            ComandoSQL.Parameters.AddWithValue("@3", _bairro);
            ComandoSQL.Parameters.AddWithValue("@4", _complemento);
            ComandoSQL.Parameters.AddWithValue("@5", _cep);
            ComandoSQL.Parameters.AddWithValue("@6", _cidade);

            DataTable table = ExecutaSelect();

            return table != null && table.Rows.Count > 0 ? Convert.ToInt32(table.Rows[0]["last"]) : -10;
        }

        public int Alterar()
        {
            ComandoSQL.Parameters.Clear();
            ComandoSQL.CommandText = @"
                update endereco
                set rua = @1,
                numero = @2,
                bairro = @3,
                complemento = @4,
                cep = @5,
                cidade = @6
                where id = @7;
            ";
            ComandoSQL.Parameters.AddWithValue("@1", _rua);
            ComandoSQL.Parameters.AddWithValue("@2", _numero);
            ComandoSQL.Parameters.AddWithValue("@3", _bairro);
            ComandoSQL.Parameters.AddWithValue("@4", _complemento);
            ComandoSQL.Parameters.AddWithValue("@5", _cep);
            ComandoSQL.Parameters.AddWithValue("@6", _cidade);
            ComandoSQL.Parameters.AddWithValue("@7", _id);

            return ExecutaComando();
        }

        public int Excluir(int id)
        {
            ComandoSQL.Parameters.Clear();
            ComandoSQL.CommandText = "delete from endereco where id = @1;";
            ComandoSQL.Parameters.AddWithValue("@1", id);

            return ExecutaComando();
        }

        public EnderecoVM ToViewModel()
        {
            return new EnderecoVM()
            {
                Id = _id,
                Rua = _rua,
                Numero = _numero,
                Bairro = _bairro,
                Complemento = _complemento,
                Cep = _cep,
                Cidade = new Cidade().GetById(_cidade).ToViewModel()
            };
        }
    }
}