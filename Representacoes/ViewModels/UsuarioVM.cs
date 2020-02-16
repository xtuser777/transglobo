namespace Representacoes.ViewModels
{
    public class UsuarioVM
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public string Senha { get; set; }
        public bool Ativo { get; set; }
        public FuncionarioVM Funcionario { get; set; }
        public NivelVM Nivel { get; set; }
    }
}