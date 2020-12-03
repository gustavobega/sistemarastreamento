using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace sistemarastreamento.Controllers
{
    public class CadastroIndustriaController : Controller
    {
        // GET: /<controller>/
        public IActionResult Index()
        {
            //var id = HttpContext.User.Claims.ToList()[0].Value;
            return View();
        }

        public IActionResult Editar(int id)
        {
            ViewBag.IdEditar = id;
            return View("Index");
        }

        [HttpPost]
        public IActionResult Criar([FromBody] Dictionary<string, string> dados,int id)
        {
            bool operacao;

            Models.Industria industria = new Models.Industria();

            industria.Id = id;
            industria.CNPJ = dados["cnpj"];
            industria.Nome = dados["nome"];
            industria.Ie = dados["ie"];
            industria.Representante = dados["representante"];
            industria.Rua = dados["rua"];
            industria.Numero = Convert.ToInt32(dados["numero"]);
            industria.Bairro = dados["bairro"];
            industria.Telefone = dados["telefone"];
            industria.Email = dados["email"];
            industria.Senha = dados["senha"];
            industria.Estado = Convert.ToInt32(dados["estado"]);
            industria.Cidade = Convert.ToInt32(dados["cidade"]);

            CamadaNegocio.IndustriaCamadaNegocio
                icn = new CamadaNegocio.IndustriaCamadaNegocio();
            operacao = icn.Criar(industria);
            string email = industria.Email;
            string senha = industria.Senha;
            return Json(new
            {
                operacao,
                email,
                senha
            });
        }

        [Authorize("CookieAuth")]
        public IActionResult indexListar()
        {
            return View();
        }

        public IActionResult Pesquisar(string nome, string tipo)
        {
            CamadaNegocio.IndustriaCamadaNegocio icn = new CamadaNegocio.IndustriaCamadaNegocio();

            List<Models.Industria> industrias = icn.Pesquisar(nome,tipo);

            //objeto anônimo
            var industriasLimpos = new List<object>();

            if (industrias != null)
            {
                foreach (var i in industrias)
                {
                    var industriaLimpo = new
                    {
                        id = i.Id,
                        nome = i.Nome,
                        cnpj = i.CNPJ,
                        telefone = i.Telefone,
                        representante = i.Representante
                    };

                    industriasLimpos.Add(industriaLimpo);
                }
            }

            return Json(new {
                operacao = industrias != null,
                industriasLimpos
            });
        }
        [HttpDelete]
        public IActionResult Excluir(int id)
        {
            CamadaNegocio.IndustriaCamadaNegocio icn = new CamadaNegocio.IndustriaCamadaNegocio();
            bool operacao = icn.Excluir(id);

            return Json(new
            {
                operacao
            });
        }

        public IActionResult Obter(int id)
        {
            CamadaNegocio.IndustriaCamadaNegocio icn = new CamadaNegocio.IndustriaCamadaNegocio();

            return Json (icn.Obter(id));
        }

        public IActionResult ObterEditar(int id)
        {
            CamadaNegocio.IndustriaCamadaNegocio icn = new CamadaNegocio.IndustriaCamadaNegocio();
            Models.Industria industria = icn.Obter(id);
            Models.Perfil perfil = icn.ObterPerfil(industria.Email);
            return Json(new {
                industria,
                perfil
            });
        }
        
        public IActionResult IndexObterPerfis()
        {
            return View();
        }

        public IActionResult IndexVisualizar(int id)
        {
            Models.Industria industria = new Models.Industria();
            CamadaNegocio.IndustriaCamadaNegocio icn = new CamadaNegocio.IndustriaCamadaNegocio();
            CamadaNegocio.CidadeCamadaNegocio ccn = new CamadaNegocio.CidadeCamadaNegocio();
            industria = icn.Obter(id);

            ViewBag.cnpj = industria.CNPJ;
            ViewBag.ie = industria.Ie;
            ViewBag.nome = industria.Nome;
            ViewBag.representante = industria.Representante;
            ViewBag.telefone = industria.Telefone;
            var estado = ccn.ObterNomeEstado(industria.Estado);
            var cidade = ccn.ObterNomeCidade(industria.Cidade);
            ViewBag.uf = estado;
            ViewBag.cidade = cidade;
            ViewBag.rua = industria.Rua;
            ViewBag.numero = industria.Numero;
            ViewBag.bairro = industria.Bairro;
            DAO.UsuarioDAO ubd = new DAO.UsuarioDAO();
            var perfil = ubd.ObterPerfil(industria.Email);
            ViewBag.perfil = perfil.Nome;
            ViewBag.email = industria.Email;

            return View();
        }

        public IActionResult ObterPerfis(string nome)
        {
            CamadaNegocio.IndustriaCamadaNegocio icn = new CamadaNegocio.IndustriaCamadaNegocio();
            return Json (icn.ObterPerfis(nome));
        }
    }
}
