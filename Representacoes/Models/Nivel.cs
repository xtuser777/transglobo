using System;
using System.Collections.Generic;
using System.Data;
using Representacoes.ViewModels;

namespace Representacoes.Models
{
    public class Nivel : Banco
    {
        private int _id;
        private string _descricao;

        public int Id { get => _id; set => _id = value; }

        public string Descricao { get => _descricao; set => _descricao = value; }

        public Nivel GetById(int id)
        {
            ComandoSQL.Parameters.Clear();
            ComandoSQL.CommandText = "select id,descricao from nivel where id = @1;";
            ComandoSQL.Parameters.AddWithValue("@1", id);
            
            DataTable table = ExecutaSelect();
            
            if (table == null || table.Rows.Count <= 0) return null;

            DataRow row = table.Rows[0];
            
            return new Nivel()
            {
                Id = Convert.ToInt32(row["id"]),
                Descricao = row["descricao"].ToString()
            };
        }

        public List<Nivel> GetAll()
        {
            ComandoSQL.Parameters.Clear();
            ComandoSQL.CommandText = "select id,descricao from nivel;";
            
            DataTable table = ExecutaSelect();
            
            if (table == null || table.Rows.Count <= 0) return null;

            List<Nivel> list = new List<Nivel>();
            for (int i = 0; i < table.Rows.Count; i++)
            {
                DataRow row = table.Rows[i];
                list.Add(new Nivel()
                {
                    Id = Convert.ToInt32(row["id"]),
                    Descricao = row["descricao"].ToString()
                });
            }

            return list;
        }

        public NivelVM ToViewModel()
        {
            return new NivelVM()
            {
                Id = _id,
                Descricao = _descricao
            };
        }
    }
}