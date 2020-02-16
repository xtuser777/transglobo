using System;
using System.Collections.Generic;
using System.Data;
using Representacoes.ViewModels;

namespace Representacoes.Models
{
    public class Funcionario : Banco
    {
        private int _id;
        private int _tipo;
        private DateTime _admissao;
        private DateTime? _demissao;
        private int _pessoa;

        public int Id { get => _id; set => _id = value; }

        public int Tipo { get => _tipo; set => _tipo = value; }

        public DateTime Admissao { get => _admissao; set => _admissao = value; }

        public DateTime? Demissao { get => _demissao; set => _demissao = value; }

        public int Pessoa { get => _pessoa; set => _pessoa = value; }

        public Funcionario GetById(int id)
        {
            ComandoSQL.Parameters.Clear();
            ComandoSQL.CommandText = "select id,tipo,admissao,demissao,pessoa from funcionario where id = @1;";
            ComandoSQL.Parameters.AddWithValue("@1", id);
            
            DataTable table = ExecutaSelect();
            
            if (table == null || table.Rows.Count <= 0) return null;

            DataRow row = table.Rows[0];
            
            return new Funcionario()
            {
                Id = Convert.ToInt32(row["id"]),
                Tipo = Convert.ToInt32(row["tipo"]),
                Admissao = Convert.ToDateTime(row["admissao"]),
                Demissao = (row["demissao"] is DBNull) ? (DateTime?)null : Convert.ToDateTime(row["demissao"]),
                Pessoa = Convert.ToInt32(row["pessoa"])
            };
        }

        public List<Funcionario> GetAll()
        {
            ComandoSQL.Parameters.Clear();
            ComandoSQL.CommandText = "select id,tipo,admissao,demissao,pessoa from funcionario;";
            
            DataTable table = ExecutaSelect();
            
            if (table == null || table.Rows.Count <= 0) return null;

            List<Funcionario> list = new List<Funcionario>();
            for (int i = 0; i < table.Rows.Count; i++)
            {
                DataRow row = table.Rows[i];
                list.Add(new Funcionario()
                {
                    Id = Convert.ToInt32(row["id"]),
                    Tipo = Convert.ToInt32(row["tipo"]),
                    Admissao = Convert.ToDateTime(row["admissao"]),
                    Demissao = row["demissao"] == null ? (DateTime?)null : Convert.ToDateTime(row["demissao"]),
                    Pessoa = Convert.ToInt32(row["pessoa"])
                });
            }

            return list;
        }
        
        public Funcionario GetVendedorById(int id)
        {
            ComandoSQL.Parameters.Clear();
            ComandoSQL.CommandText = @"select id,tipo,admissao,demissao,pessoa from funcionario where id = @id and tipo = 2;";
            ComandoSQL.Parameters.AddWithValue("@id", id);
            
            DataTable table = ExecutaSelect();
            
            if (table == null || table.Rows.Count <= 0) return null;

            DataRow row = table.Rows[0];
            
            return new Funcionario()
            {
                Id = Convert.ToInt32(row["id"]),
                Tipo = Convert.ToInt32(row["tipo"]),
                Admissao = Convert.ToDateTime(row["admissao"]),
                Demissao = row["demissao"] == null ? (DateTime?)null : Convert.ToDateTime(row["demissao"]),
                Pessoa = Convert.ToInt32(row["pessoa"])
            };
        }

        public List<Funcionario> GetVendedores()
        {
            ComandoSQL.Parameters.Clear();
            ComandoSQL.CommandText = @"select id,tipo,admissao,demissao,pessoa from funcionario where tipo = 2;";
            
            DataTable table = ExecutaSelect();
            
            if (table == null || table.Rows.Count <= 0) return null;

            List<Funcionario> list = new List<Funcionario>();
            for (int i = 0; i < table.Rows.Count; i++)
            {
                DataRow row = table.Rows[i];
                list.Add(new Funcionario()
                {
                    Id = Convert.ToInt32(row["id"]),
                    Tipo = Convert.ToInt32(row["tipo"]),
                    Admissao = Convert.ToDateTime(row["admissao"]),
                    Demissao = row["demissao"] == null ? (DateTime?)null : Convert.ToDateTime(row["demissao"]),
                    Pessoa = Convert.ToInt32(row["pessoa"])
                });
            }

            return list;
        }

        public int Gravar()
        {
            ComandoSQL.Parameters.Clear();
            ComandoSQL.CommandText = @"
                insert into funcionario(tipo,admissao,demissao,pessoa) values(@1,@2,@3,@4);
                select LAST_INSERT_ID() as last;
            ";
            ComandoSQL.Parameters.AddWithValue("@1", _tipo);
            ComandoSQL.Parameters.AddWithValue("@2", _admissao);
            ComandoSQL.Parameters.AddWithValue("@3", _demissao);
            ComandoSQL.Parameters.AddWithValue("@4", _pessoa);

            DataTable table = ExecutaSelect();

            return table != null && table.Rows.Count > 0 ? Convert.ToInt32(table.Rows[0]["last"]) : -10;
        }

        public int Alterar()
        {
            ComandoSQL.Parameters.Clear();
            ComandoSQL.CommandText = @"
                update funcionario
                set tipo = @1,
                admissao = @2,
                demissao = @3,
                pessoa = @4,
                where id = @5;
            ";
            ComandoSQL.Parameters.AddWithValue("@1", _tipo);
            ComandoSQL.Parameters.AddWithValue("@2", _admissao);
            ComandoSQL.Parameters.AddWithValue("@3", _demissao);
            ComandoSQL.Parameters.AddWithValue("@4", _pessoa);
            ComandoSQL.Parameters.AddWithValue("@5", _id);

            return ExecutaComando();
        }

        public int Excluir(int id)
        {
            ComandoSQL.Parameters.Clear();
            ComandoSQL.CommandText = "delete from funcionario where id = @1;";
            ComandoSQL.Parameters.AddWithValue("@1", id);

            return ExecutaComando();
        }
        
        public int Desativar(int id)
        {
            ComandoSQL.Parameters.Clear();
            ComandoSQL.CommandText = @"update funcionario set demissao = now() where id = @id;
                                       update usuario set ativo = false where id = @id;";
            ComandoSQL.Parameters.AddWithValue("@id", id);

            return ExecutaComando();
        }

        public int Reativar(int id)
        {
            ComandoSQL.Parameters.Clear();
            ComandoSQL.CommandText = @"update funcionario set demissao = null where id = @id;
                                       update usuario set ativo = true where id = @id;";
            ComandoSQL.Parameters.AddWithValue("@id", id);

            return ExecutaComando();
        }

        public FuncionarioVM ToViewModel()
        {
            return new FuncionarioVM()
            {
                Id = _id,
                Tipo = _tipo,
                Admissao = _admissao,
                Demissao = _demissao,
                Pessoa = new PessoaFisica().GetById(_pessoa).ToViewModel()
            };
        }
    }
}