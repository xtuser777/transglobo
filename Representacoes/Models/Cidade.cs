using System;
using System.Collections.Generic;
using System.Data;
using Representacoes.ViewModels;

namespace Representacoes.Models
{
    public class Cidade : Banco
    {
        private int _id;
        private string _nome;
        private int _estado;

        public int Id { get => _id; set => _id = value; }

        public string Nome { get => _nome; set => _nome = value; }

        public int Estado { get => _estado; set => _estado = value; }

        public Cidade GetById(int id)
        {
            ComandoSQL.Parameters.Clear();
            ComandoSQL.CommandText = "select id,nome,estado from cidade where id = @id;";
            ComandoSQL.Parameters.AddWithValue("@id", id);

            DataTable table = ExecutaSelect();
            
            if (table == null || table.Rows.Count <= 0) return null;

            DataRow row = table.Rows[0];
            
            return new Cidade()
            {
                Id = Convert.ToInt32(row["id"]),
                Nome = row["nome"].ToString(),
                Estado = Convert.ToInt32(row["estado"])
            };
        }

        public List<Cidade> GetByEstado(int estado)
        {
            ComandoSQL.Parameters.Clear();
            ComandoSQL.CommandText = "select id,nome,estado from cidade where estado = @e;";
            ComandoSQL.Parameters.AddWithValue("@e", estado);

            DataTable table = ExecutaSelect();
            
            if (table == null || table.Rows.Count <= 0) return null;
            
            List<Cidade> list = new List<Cidade>();
            for (int i = 0; i < table.Rows.Count; i++)
            {
                DataRow row = table.Rows[i];
                list.Add(new Cidade()
                {
                    Id = Convert.ToInt32(row["id"]),
                    Nome = row["nome"].ToString(),
                    Estado = Convert.ToInt32(row["estado"])
                });
            }

            return list;
        }

        public CidadeVM ToViewModel()
        {
            return new CidadeVM()
            {
                Id = _id,
                Nome = _nome,
                Estado = new Estado().GetById(_estado).ToViewModel()
            };
        }
    }
}