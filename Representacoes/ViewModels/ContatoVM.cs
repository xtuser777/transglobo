namespace Representacoes.ViewModels
{
    public class ContatoVM
    {
        public int Id { get; set; }
        public string Telefone { get; set; }
        public string Celular { get; set; }
        public string Email { get; set; }
        public EnderecoVM Endereco { get; set; }
    }
}