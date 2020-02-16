using System;

namespace Representacoes.ViewModels
{
    public class FuncionarioVM
    {
        public int Id { get; set; }
        public int Tipo { get; set; }
        public DateTime Admissao { get; set; }
        public DateTime? Demissao { get; set; }
        public PessoaFisicaVM Pessoa { get; set; }
    }
}