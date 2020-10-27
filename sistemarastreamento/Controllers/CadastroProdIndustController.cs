using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace sistemarastreamento.Controllers
{
    [Authorize("CookieAuth")]
    public class CadastroProdIndustController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Editar(int id)
        {
            ViewBag.IdEditar = id;
            return View("Index");
        }

        public IActionResult Criar([FromBody] Dictionary<string, string> dados,int id)
        {
            bool operacao;

            Models.ProdutoIndust prodindust = new Models.ProdutoIndust();
            prodindust.Id = id;
            prodindust.Cod_ref = dados["cod_ref"];
            prodindust.Descricao = dados["descricao"];
            prodindust.Id_indust = Convert.ToInt32(HttpContext.User.Claims.ToList()[3].Value);

            CamadaNegocio.ProdIndustCamadaNegocio pcn = new CamadaNegocio.ProdIndustCamadaNegocio();
            operacao = pcn.Criar(prodindust);

            return Json(new
            {
                operacao
            });
        }

        public IActionResult indexListar()
        {
            return View();
        }

        public IActionResult Pesquisar(string produto, string tipo)
        {
            CamadaNegocio.ProdIndustCamadaNegocio pcn = new CamadaNegocio.ProdIndustCamadaNegocio();
            var id_indust = Convert.ToInt32(HttpContext.User.Claims.ToList()[3].Value);

            List<Models.ProdutoIndust> prodindust = pcn.Pesquisar(produto, id_indust, tipo);

            //objeto anônimo
            var produtosLimpos = new List<object>();

            foreach (var p in prodindust)
            {
                var produtoLimpo = new
                {
                    id = p.Id,
                    cod_ref = p.Cod_ref,
                    descricao = p.Descricao,
                };

                produtosLimpos.Add(produtoLimpo);
            }


            return Json(produtosLimpos);
        }

        public IActionResult Excluir(int id)
        {
            CamadaNegocio.ProdIndustCamadaNegocio pcn = new CamadaNegocio.ProdIndustCamadaNegocio();
            bool operacao = pcn.Excluir(id);

            return Json(new
            {
                operacao
            });
        }

        public IActionResult ObterEditar(int id)
        {
            CamadaNegocio.ProdIndustCamadaNegocio pcn = new CamadaNegocio.ProdIndustCamadaNegocio();
            Models.ProdutoIndust prodindust = pcn.Obter(id);
            return Json(new
            {
                prodindust
            });
        }
       
    }
}
