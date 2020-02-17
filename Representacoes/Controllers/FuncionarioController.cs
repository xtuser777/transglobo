using Representacoes.Filters;
using Representacoes.Models;
using Representacoes.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Representacoes.Controllers
{
    [ValidarUsuario]
    public class FuncionarioController : Controller
    {
        private static List<UsuarioVM> _funcs;

        public FuncionarioController()
        {
            List<Usuario> usuarios = new Usuario().GetAll();
            _funcs = new List<UsuarioVM>();
            for (int i = 0; usuarios != null && i < usuarios.Count; i++)
            {
                _funcs.Add(usuarios[i].ToViewModel());
            }
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Novo()
        {
            return View();
        }

        public ActionResult Detalhes()
        {
            return View();
        }

        [HttpPost]
        public JsonResult Enviar(string id)
        {
            if (string.IsNullOrEmpty(id)) return Json("Parâmetro inválido...");
            HttpContext.Session["idfunc"] = id;
            return Json("");
        }

        public ActionResult Dados()
        {
            return View();
        }

        public JsonResult Obter()
        {
            return Json(_funcs, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ObterPorChave(string chave)
        {
            var filtrado = _funcs.FindAll(u =>
                u.Funcionario.Pessoa.Nome.Equals(chave,StringComparison.CurrentCultureIgnoreCase) ||
                u.Login.Equals(chave, StringComparison.CurrentCultureIgnoreCase) ||
                u.Funcionario.Pessoa.Contato.Email.Equals(chave, StringComparison.CurrentCultureIgnoreCase)
            );

            return Json(filtrado);
        }

        [HttpPost]
        public JsonResult ObterPorAdmissao(string adm)
        {
            var filtrado = _funcs.FindAll(u =>
                u.Funcionario.Admissao.ToString("yyyy-MM-dd") == adm
            );

            return Json(filtrado);
        }

        [HttpPost]
        public JsonResult ObterPorChaveAdm(string chave, string adm)
        {
            var filtrado = _funcs.FindAll(u =>
                (u.Funcionario.Pessoa.Nome.Equals(chave, StringComparison.CurrentCultureIgnoreCase) ||
                u.Login.Equals(chave, StringComparison.CurrentCultureIgnoreCase) ||
                u.Funcionario.Pessoa.Contato.Email.Equals(chave, StringComparison.CurrentCultureIgnoreCase)) &&
                u.Funcionario.Admissao.ToString("yyyy-MM-dd") == adm
            );

            return Json(filtrado);
        }

        public JsonResult ObterFuncInfo()
        {
            var id = HttpContext.Session["idfunc"];

            return Json(_funcs.Find(u => u.Id == Convert.ToInt32(id)));
        }

        public JsonResult ObterFuncData()
        {
            var id = HttpContext.Session["id"];

            return Json(new Usuario().GetById(Convert.ToInt32(id)).ToViewModel());
        }

        public JsonResult ObterEstados()
        {
            List<Estado> estados = new Estado().GetAll();
            List<EstadoVM> evms = new List<EstadoVM>();
            for (int i = 0; estados!=null && i<estados.Count; i++)
            {
                evms.Add(estados[i].ToViewModel());
            }

            return Json(evms);
        }

        [HttpPost]
        public JsonResult ObterCidades(FormCollection form)
        {
            List<Cidade> cidades = new Cidade().GetByEstado(Convert.ToInt32(form["estado"]));
            List<CidadeVM> cvms = new List<CidadeVM>();
            for (int i=0; cidades!=null && i<cidades.Count; i++)
            {
                cvms.Add(cidades[i].ToViewModel());
            }

            return Json(cvms);
        }

        [HttpGet]
        public JsonResult ObterNiveis()
        {
            List<Nivel> niveis = new Nivel().GetAll();
            List<NivelVM> nvms = new List<NivelVM>();
            for (int i=0; niveis!=null && i<niveis.Count; i++)
            {
                nvms.Add(niveis[i].ToViewModel());
            }

            return Json(nvms);
        }

        [HttpPost]
        public JsonResult VerificaLogin(string login)
        {
            return Json(new Usuario().VerificarLogin(login) == true ? "true" : "false");
        }

        [HttpPost]
        public JsonResult Ordenar(string col)
        {
            var ord = new List<UsuarioVM>();

            switch (col)
            {
                case "1":
                    ord = _funcs.OrderBy(u => u.Id).ToList();
                    break;
                case "2":
                    ord = _funcs.OrderByDescending(u => u.Id).ToList();
                    break;
                case "3":
                    ord = _funcs.OrderBy(u => u.Funcionario.Pessoa.Nome).ToList();
                    break;
                case "4":
                    ord = _funcs.OrderByDescending(u => u.Funcionario.Pessoa.Nome).ToList();
                    break;
                case "5":
                    ord = _funcs.OrderBy(u => u.Login).ToList();
                    break;
                case "6":
                    ord = _funcs.OrderByDescending(u => u.Login).ToList();
                    break;
                case "7":
                    ord = _funcs.OrderBy(u => u.Nivel.Id).ToList();
                    break;
                case "8":
                    ord = _funcs.OrderByDescending(u => u.Nivel.Id).ToList();
                    break;
                case "9":
                    ord = _funcs.OrderBy(u => u.Funcionario.Pessoa.Cpf).ToList();
                    break;
                case "10":
                    ord = _funcs.OrderByDescending(u => u.Funcionario.Pessoa.Cpf).ToList();
                    break;
                case "11":
                    ord = _funcs.OrderBy(u => u.Funcionario.Admissao).ToList();
                    break;
                case "12":
                    ord = _funcs.OrderByDescending(u => u.Funcionario.Admissao).ToList();
                    break;
                case "13":
                    ord = _funcs.OrderBy(u => u.Funcionario.Tipo).ToList();
                    break;
                case "14":
                    ord = _funcs.OrderByDescending(u => u.Funcionario.Tipo).ToList();
                    break;
                case "15":
                    ord = _funcs.OrderBy(u => u.Ativo).ToList();
                    break;
                case "16":
                    ord = _funcs.OrderByDescending(u => u.Ativo).ToList();
                    break;
                case "17":
                    ord = _funcs.OrderBy(u => u.Funcionario.Pessoa.Contato.Email).ToList();
                    break;
                case "18":
                    ord = _funcs.OrderByDescending(u => u.Funcionario.Pessoa.Contato.Email).ToList();
                    break;
            }

            return Json(ord);
        }

        [HttpPost]
        public JsonResult VerificarCpf(string cpf)
        {
            return Json(new PessoaFisica().VerifyCpf(cpf));
        }

        [HttpPost]
        public JsonResult Gravar(FormCollection form)
        {
            string nome = form["nome"];
            string nasc = form["nasc"];
            string rg = form["rg"];
            string cpf = form["cpf"];
            string adm = form["adm"];
            string tipo = form["tipo"];
            string rua = form["rua"];
            string numero = form["numero"];
            string bairro = form["bairro"];
            string complemento = form["complemento"];
            string cep = form["cep"];
            string cidade = form["cidade"];
            string telefone = form["telefone"];
            string celular = form["celular"];
            string email = form["email"];
            string nivel = "";
            string login = "";
            string senha = "";
            if (tipo != "2")
            {
                nivel = form["nivel"];
                login = form["login"];
                senha = form["senha"];
            }

            int.TryParse(tipo, out int tipo1);
            int.TryParse(cidade, out int cidade1);
            int.TryParse(nivel, out int nivel1);
            DateTime.TryParse(nasc, out DateTime dataNasc);
            DateTime.TryParse(adm, out DateTime dataAdm);

            int res1 = new Endereco()
            {
                Id = 0,
                Rua = rua,
                Numero = numero,
                Bairro = bairro,
                Complemento = complemento,
                Cep = cep,
                Cidade = cidade1
            }.Gravar();

            if (res1 == -10) return Json("Ocorreu um problema ao executar o comando SQL ao gravar o endereço.");

            int res2 = new Contato()
            {
                Id = 0,
                Telefone = telefone,
                Celular = celular,
                Email = email,
                Endereco = res1
            }.Gravar();

            if (res2 == -10)
            {
                new Endereco().Excluir(res1);
                return Json("Ocorreu um problema ao executar o comando SQL ao gravar o contato.");
            }

            int res3 = new PessoaFisica()
            {
                Id = 0,
                Nome = nome,
                Rg = rg,
                Cpf = cpf,
                Nascimento = dataNasc,
                Contato = res2
            }.Gravar();

            if (res3 == -10)
            {
                new Contato().Excluir(res2);
                new Endereco().Excluir(res1);
                return Json("Ocorreu um problema ao executar o comando SQL ao gravar a pessoa.");
            }

            int res4 = new Funcionario()
            {
                Id = 0,
                Tipo = tipo1,
                Admissao = dataAdm,
                Demissao = null,
                Pessoa = res3
            }.Gravar();

            if (res4 == -10)
            {
                new PessoaFisica().Excluir(res3);
                new Contato().Excluir(res2);
                new Endereco().Excluir(res1);
                return Json("Ocorreu um problema ao executar o comando SQL ao gravar o funcionário.");
            }

            int res5 = new Usuario()
            {
                Id = 0,
                Login = login,
                Senha = senha,
                Ativo = true,
                Funcionario = res4,
                Nivel = nivel1
            }.Gravar();

            if (res5 == -10)
            {
                new Funcionario().Excluir(res4);
                new PessoaFisica().Excluir(res3);
                new Contato().Excluir(res2);
                new Endereco().Excluir(res1);
                return Json("Ocorreu um problema ao executar o comando SQL ao gravar o usuário.");
            }

            return Json("");
        }

        [HttpPost]
        public JsonResult Alterar(FormCollection form)
        {
            string idendereco = form["idendereco"];
            string idcontato = form["idcontato"];
            string idpessoa = form["idpessoa"];
            string idfuncionario = form["idfuncionario"];
            string idusuario = form["idusuario"];
            string ativo = form["ativo"];
            string nome = form["nome"];
            string nasc = form["nasc"];
            string rg = form["rg"];
            string cpf = form["cpf"];
            string adm = form["adm"];
            string tipo = form["tipo"];
            string rua = form["rua"];
            string numero = form["numero"];
            string bairro = form["bairro"];
            string complemento = form["complemento"];
            string cep = form["cep"];
            string cidade = form["cidade"];
            string telefone = form["telefone"];
            string celular = form["celular"];
            string email = form["email"];
            string nivel = "";
            string login = "";
            string senha = "";
            if (tipo != "2")
            {
                nivel = form["nivel"];
                login = form["login"];
                senha = form["senha"];
            }

            int.TryParse(idendereco, out int endereco);
            int.TryParse(idcontato, out int contato);
            int.TryParse(idpessoa, out int pessoa);
            int.TryParse(idfuncionario, out int func);
            int.TryParse(idusuario, out int usu);
            int.TryParse(tipo, out int tipo1);
            int.TryParse(cidade, out int cidade1);
            int.TryParse(nivel, out int nivel1);
            DateTime.TryParse(nasc, out DateTime dataNasc);
            DateTime.TryParse(adm, out DateTime dataAdm);
            bool.TryParse(ativo, out var ativo1);

            int res1 = new Endereco()
            {
                Id = endereco,
                Rua = rua,
                Numero = numero,
                Bairro = bairro,
                Complemento = complemento,
                Cep = cep,
                Cidade = cidade1
            }.Alterar();

            if (res1 <= 0) return Json("Ocorreu um problema ao executar o comando SQL ao alterar o endereço.");

            int res2 = new Contato()
            {
                Id = contato,
                Telefone = telefone,
                Celular = celular,
                Email = email,
                Endereco = endereco
            }.Alterar();

            if (res2 <= 0) return Json("Ocorreu um problema ao executar o comando SQL ao alterar o contato.");

            int res3 = new PessoaFisica()
            {
                Id = pessoa,
                Nome = nome,
                Rg = rg,
                Cpf = cpf,
                Nascimento = dataNasc,
                Contato = contato
            }.Alterar();

            if (res3 <= 0) return Json("Ocorreu um problema ao executar o comando SQL ao alterar a pessoa.");

            int res4 = new Funcionario()
            {
                Id = func,
                Tipo = tipo1,
                Admissao = dataAdm,
                Demissao = null,
                Pessoa = pessoa
            }.Alterar();

            if (res4 <= 0) return Json("Ocorreu um problema ao executar o comando SQL ao alterar o funcionário.");

            int res5 = new Usuario()
            {
                Id = usu,
                Login = login,
                Senha = senha,
                Ativo = true,
                Funcionario = func,
                Nivel = nivel1
            }.Alterar();

            if (res5 <= 0) return Json("Ocorreu um problema ao executar o comando SQL ao alterar o usuário.");

            return Json("");
        }

        public JsonResult IsLastAdmin()
        {
            return Json(new Usuario().IsLastAdmin());
        }

        [HttpPost]
        public JsonResult Excluir(int id)
        {
            var res = 0;
            UsuarioVM func = _funcs.Find(u => u.Id == id);

            res = new Usuario().Excluir(id);
            if (res <= 0) return Json("Ocorreu um problema ao excluir o usuário...");

            res = new Funcionario().Excluir(id);
            if (res <= 0) return Json("Ocorreu um problema ao excluir o funcionário...");

            res = new PessoaFisica().Excluir(func.Funcionario.Pessoa.Id);
            if (res <= 0) return Json("Ocorreu um problema ao excluir a pessoa...");

            res = new Contato().Excluir(func.Funcionario.Pessoa.Contato.Id);

            res = new Endereco().Excluir(func.Funcionario.Pessoa.Contato.Endereco.Id);
            if (res <= 0) return Json("Ocorreu um problema ao excluir o endereço...");

            _funcs.Remove(func);

            return Json("");
        }

        [HttpPost]
        public JsonResult Desativar(int id)
        {
            return Json(new Funcionario().Desativar(id));
        }

        [HttpPost]
        public JsonResult Reativar(int id)
        {
            return Json(new Funcionario().Reativar(id));
        }
    }
}