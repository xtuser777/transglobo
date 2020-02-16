using System;

namespace Representacoes.ViewModels
{
    public class PessoaFisicaVM
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Rg { get; set; }
        public string Cpf { get; set; }
        public DateTime Nascimento { get; set; }
        public ContatoVM Contato { get; set; }
    }
}