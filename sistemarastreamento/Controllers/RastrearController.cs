using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace sistemarastreamento.Controllers
{
    [Authorize("CookieAuth")]
    public class RastrearController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Buscar([FromBody] Dictionary<string,string> dados)
        {
            bool operacao = false;

            string lote = dados["lote"];
            string codigo = dados["codigo"];
            string hospital = dados["hospital"];

            CamadaNegocio.IndustriaCamadaNegocio icn = new CamadaNegocio.IndustriaCamadaNegocio();
            CamadaNegocio.DistribuidorCamadaNegocio dcn = new CamadaNegocio.DistribuidorCamadaNegocio();
            CamadaNegocio.CidadeCamadaNegocio ccn = new CamadaNegocio.CidadeCamadaNegocio();

            CamadaNegocio.RastroCamadaNegocio rcn = new CamadaNegocio.RastroCamadaNegocio();
            
            DataTable dt = rcn.Buscar(lote, codigo, hospital);

            Models.Industria indust = new Models.Industria();
            Models.Distribuidor dist = new Models.Distribuidor();
            string cidadeIndust = "";
            string estadoIndust = "";
            string cidadeDist = "";
            string estadoDist = "";
            var rastreioDestino = new object();
            if (dt.Rows.Count > 0)
            {
                operacao = true;
                DataRow dr;
                
                dr = dt.Rows[0];

                indust = icn.Obter(Convert.ToInt32(dr["ri_id_indust"]));
                dist = dcn.Obter(Convert.ToInt32(dr["rd_id_dist"]));
                cidadeIndust = ccn.ObterNomeCidade(indust.Cidade);
                estadoIndust = ccn.ObterNomeEstado(indust.Estado);

                cidadeDist = ccn.ObterNomeCidade(dist.Cidade);
                estadoDist = ccn.ObterNomeEstado(dist.Estado);

                rastreioDestino = new
                {
                    descricao = dr["descricao"],
                    ri_id_indust = dr["ri_id_indust"],
                    rd_id_dist = dr["rd_id_dist"],
                    rdest_nome = dr["rdest_nome"],
                    rdest_rua = dr["rdest_rua"],
                    rdest_numero = dr["rdest_numero"],
                    rdest_bairro = dr["rdest_bairro"],
                    rdest_cidade = dr["rdest_cidade"],
                    rdest_estado = dr["rdest_estado"],
                    rdest_cep = dr["rdest_cep"]
                };
                
            }

            return Json(new
            {
                operacao,
                indust,
                cidadeIndust,
                estadoIndust,
                cidadeDist,
                estadoDist,
                dist,
                rastreioDestino
            });
        }

    }
}