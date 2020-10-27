using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace sistemarastreamento.Controllers
{
    public class CadastroProdDistController : Controller
    {
        [Authorize("CookieAuth")]
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult ObterProd([FromBody] Dictionary<string, string> dados)
        {
            CamadaNegocio.ProdIndustCamadaNegocio pcn = new CamadaNegocio.ProdIndustCamadaNegocio();
            string cod_ref = dados["cod_ref"];
            Models.ProdutoIndust prodemp = pcn.ObterProd(cod_ref);
            return Json(new
            {
                prodemp
            });
        }

        public IActionResult Editar(int id)
        {
            ViewBag.IdEditar = id;
            return View("Index");
        }

        public IActionResult Criar([FromBody] Dictionary<string, string> dados, int id)
        {
            bool operacao;

            Models.ProdutoDist proddist = new Models.ProdutoDist();
            proddist.Id = id;
            proddist.Cod_ref = dados["cod_ref"];
            proddist.Id_dist = Convert.ToInt32(HttpContext.User.Claims.ToList()[3].Value);
            proddist.Cod_prod_dist = dados["codigo"];

            CamadaNegocio.ProdDistCamadaNegocio pcn = new CamadaNegocio.ProdDistCamadaNegocio();
            operacao = pcn.Criar(proddist);

            if (operacao)
            {
                //cadastro no estoque
                Models.ProdutoIndust pi = new Models.ProdutoIndust();
                CamadaNegocio.ProdIndustCamadaNegocio picn = new CamadaNegocio.ProdIndustCamadaNegocio();
                pi = picn.ObterProd(proddist.Cod_ref);

                Models.Estoque estoque = new Models.Estoque();
                estoque.Id_dist = proddist.Id_dist;
                estoque.Id_prod = pi.Id;
                estoque.Lote = dados["lote"];
                estoque.Saldo = Convert.ToInt32(dados["saldo"]);

                CamadaNegocio.EstoqueCamadaNegocio ecn = new CamadaNegocio.EstoqueCamadaNegocio();
                operacao = ecn.CriarEstoque(estoque);
            }
            
            return Json(new
            {
                operacao
            });
        }

        public IActionResult indexListar()
        {
            return View();
        }

        public IActionResult Pesquisar(string descricao, string tipo)
        {
            CamadaNegocio.ProdDistCamadaNegocio pcn = new CamadaNegocio.ProdDistCamadaNegocio();

            CamadaNegocio.ProdIndustCamadaNegocio picn = new CamadaNegocio.ProdIndustCamadaNegocio();
            Models.ProdutoIndust pi = new Models.ProdutoIndust();

            var id_dist = Convert.ToInt32(HttpContext.User.Claims.ToList()[3].Value);

            DataTable dt = pcn.Pesquisar(descricao, id_dist, tipo);
            
            //objeto anônimo
            var produtosLimpos = new List<object>();

            if (dt.Rows.Count > 0)
            {
                DataRow dr;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    dr = dt.Rows[i];
                    var produtoLimpo = new
                    {
                        id = dr["id"],
                        codigo = dr["cod_prod_dist"],
                        descricao = dr["descricao"],
                        lote = dr["lote"],
                        saldo = dr["saldo"]
                    };
                    produtosLimpos.Add(produtoLimpo);
                }
            }

            return Json(new
            {
                produtosLimpos
            });
        }

        public IActionResult Excluir(int id)
        {
            bool operacao;
            CamadaNegocio.ProdDistCamadaNegocio pdcn = new CamadaNegocio.ProdDistCamadaNegocio();
            Models.ProdutoDist pd = new Models.ProdutoDist();

            pd = pdcn.Obter(id);

            CamadaNegocio.ProdIndustCamadaNegocio picn = new CamadaNegocio.ProdIndustCamadaNegocio();
            Models.ProdutoIndust pi = new Models.ProdutoIndust();
            pi = picn.ObterProd(pd.Cod_ref);

            CamadaNegocio.EstoqueCamadaNegocio ecn = new CamadaNegocio.EstoqueCamadaNegocio();
            operacao = ecn.Excluir(pi.Id);

            if (operacao)
            {
                CamadaNegocio.ProdDistCamadaNegocio pcn = new CamadaNegocio.ProdDistCamadaNegocio();
                operacao = pcn.Excluir(pd.Cod_ref);
            }

            return Json(new
            {
                operacao
            });
        }

        public IActionResult ObterEditar(int id)
        {
            CamadaNegocio.ProdDistCamadaNegocio pcn = new CamadaNegocio.ProdDistCamadaNegocio();
            Models.ProdutoDist proddist = pcn.Obter(id);

            CamadaNegocio.ProdIndustCamadaNegocio picn = new CamadaNegocio.ProdIndustCamadaNegocio();
            Models.ProdutoIndust pi = new Models.ProdutoIndust();

            pi = picn.ObterProd(proddist.Cod_ref);

            CamadaNegocio.EstoqueCamadaNegocio ecn = new CamadaNegocio.EstoqueCamadaNegocio();
            Models.Estoque estoque = new Models.Estoque();

            estoque = ecn.Obter(pi.Id);

            return Json(new
            {
                proddist,
                estoque
            });
        }
    }
}
