using System;
using System.Collections.Generic;
using System.Data;
using Representacoes.ViewModels;

namespace Representacoes.Models
{
    public class Estado : Banco
    {
        private int _id;
        private string _nome;
        private string _sigla;

        public int Id
        {
            get => _id;
            set => _id = value;
        }

        public string Nome
        {
            get => _nome;
            set => _nome = value;
        }

        public string Sigla
        {
            get => _sigla;
            set => _sigla = value;
        }

        public Estado GetById(int id)
        {
            ComandoSQL.Parameters.Clear();
            ComandoSQL.CommandText = @"select id,nome,sigla from estado where id = @id;";
            ComandoSQL.Parameters.AddWithValue("@id", id);

            var table = ExecutaSelect();

            if (table != null && table.Rows.Count > 0)
            {
                return new Estado()
                {
                    Id = Convert.ToInt32(table.Rows[0]["id"]),
                    Nome = table.Rows[0]["nome"].ToString(),
                    Sigla = table.Rows[0]["sigla"].ToString()
                };
            }

            return null;
        }

        public List<Estado> GetAll()
        {
            ComandoSQL.Parameters.Clear();
            ComandoSQL.CommandText = "select id,nome,sigla from estado;";

            DataTable table = ExecutaSelect();

            if (table == null || table.Rows.Count <= 0) return null;
            
            List<Estado> list = new List<Estado>();
            for (int i = 0; i < table.Rows.Count; i++)
            {
                list.Add(new Estado()
                {
                    Id = Convert.ToInt32(table.Rows[i]["id"]),
                    Nome = table.Rows[i]["nome"].ToString(),
                    Sigla = table.Rows[i]["sigla"].ToString()
                });
            }

            return list;
        }

        public EstadoVM ToViewModel()
        {
            return new EstadoVM()
            {
                Id = _id,
                Nome = _nome,
                Sigla = _sigla
            };
        }
    }
}