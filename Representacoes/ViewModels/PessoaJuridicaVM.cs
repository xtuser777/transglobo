namespace Representacoes.ViewModels
{
    public class PessoaJuridicaVM
    {
        public int Id { get; set; }
        public string RazaoSocial { get; set; }
        public string NomeFantasia { get; set; }
        public string Cpnj { get; set; }
        public ContatoVM Contato { get; set; }
    }
}