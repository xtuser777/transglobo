namespace Representacoes.ViewModels
{
    public class EnderecoVM
    {
        public int Id { get; set; }
        public string Rua { get; set; }
        public string Numero { get; set; }
        public string Bairro { get; set; }
        public string Complemento { get; set; }
        public string Cep { get; set; }
        public CidadeVM Cidade { get; set; }
    }
}