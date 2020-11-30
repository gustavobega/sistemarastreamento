using System;
using System.Collections.Generic;
using System.Linq;
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

        public IActionResult ObterProdDist([FromBody] Dictionary<string, string> dados)
        {
            CamadaNegocio.ProdIndustCamadaNegocio pcn = new CamadaNegocio.ProdIndustCamadaNegocio();
            string codigo = dados["codigo"];
            bool operacao;
            string cod_ref, descricao;
            (operacao, cod_ref, descricao) = pcn.ObterProdDist(codigo);
            return Json(new
            {
                operacao,
                cod_ref,
                descricao
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
            string msg;

            Models.ProdutoDist proddist = new Models.ProdutoDist();
            proddist.Cod_ref = dados["cod_ref"];
            proddist.Id_dist = Convert.ToInt32(HttpContext.User.Claims.ToList()[3].Value);
            proddist.Cod_prod_dist = dados["codigo"];

            CamadaNegocio.ProdDistCamadaNegocio pcn = new CamadaNegocio.ProdDistCamadaNegocio();
            (operacao, msg) = pcn.Criar(proddist, dados["lote"], id);

            if (operacao && id == 0)
            {
                Models.Estoque estoque = new Models.Estoque();
                estoque.Id_dist = proddist.Id_dist;
                estoque.Id_prod = proddist.Id;
                estoque.Lote = dados["lote"];
                estoque.Saldo = Convert.ToInt32(dados["saldo"]);

                CamadaNegocio.EstoqueCamadaNegocio ecn = new CamadaNegocio.EstoqueCamadaNegocio();
                operacao = ecn.AlterarEstoque(estoque);
            }
            
            return Json(new
            {
                operacao,
                msg
            });
        }

        public IActionResult indexListar()
        {
            return View();
        }

        public IActionResult Pesquisar(string descricao, string tipo)
        {
            CamadaNegocio.ProdDistCamadaNegocio pcn = new CamadaNegocio.ProdDistCamadaNegocio();

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
                        id = dr[10], //id do produto distribuidor
                        id_estoque = dr[0], //id do estoque
                        cod_ref = dr["cod_ref"],
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
            CamadaNegocio.EstoqueCamadaNegocio ecn = new CamadaNegocio.EstoqueCamadaNegocio();
            Models.Estoque estoque = ecn.Obter(id);
            operacao = ecn.Excluir(id);

            int qtd = ecn.ObterTodosEstoque(estoque.Id_prod);

            if (operacao && qtd == 0)
            {
                CamadaNegocio.ProdDistCamadaNegocio pdcn = new CamadaNegocio.ProdDistCamadaNegocio();
                pdcn.Excluir(estoque.Id_prod);
            }

            return Json(new
            {
                operacao
            });
        }

        public IActionResult ObterEditar(int id)
        {
            CamadaNegocio.ProdDistCamadaNegocio pcn = new CamadaNegocio.ProdDistCamadaNegocio();
            DataTable dt = pcn.ObterDadosProdDist(id);
            bool operacao = false;
            object produtoLimpo = new object();

            if (dt.Rows.Count > 0)
            {
                operacao = true;
                DataRow dr;
                
                dr = dt.Rows[0];
                produtoLimpo = new
                {
                    id_estoque = dr[0],
                    cod_ref = dr["cod_ref"],
                    codigo = dr["cod_prod_dist"],
                    descricao = dr["descricao"],
                    lote = dr["lote"],
                    saldo = dr["saldo"]
                };
                
            }

            return Json(new
            {
                operacao,
                produtoLimpo
            });

        }
    }
}
