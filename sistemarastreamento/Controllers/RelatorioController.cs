using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Rotativa.AspNetCore;

namespace sistemarastreamento.Controllers
{
    public class RelatorioController : Controller
    {
        [HttpPost]
        public IActionResult ReportVendaIndust([FromBody] Dictionary<string, string> datas)
        {
            var dados = relVendaIndust(Convert.ToDateTime(datas["datainicio"]),Convert.ToDateTime(datas["datafim"]));
            var pdfResult = new ViewAsPdf("ReportVendaIndust", dados)
            {
                CustomSwitches = "--footer-center \"  Data: " + DateTime.Now.Date.ToString("dd/MM/yyyy") + "  Página: [page]/[toPage]\"" + " --footer-line --footer-font-size \"12\" --footer-spacing 1 --footer-font-name \"Segoe UI\""
            };

            return pdfResult;
        }

        [HttpPost]
        public IActionResult ReportVendaDist([FromBody] Dictionary<string, string> datas)
        {
            var dados = relVendaDist(Convert.ToDateTime(datas["datainicio"]), Convert.ToDateTime(datas["datafim"]));
            var pdfResult = new ViewAsPdf("ReportVendaDist", dados)
            {
                CustomSwitches = "--footer-center \"  Data: " + DateTime.Now.Date.ToString("dd/MM/yyyy") + "  Página: [page]/[toPage]\"" + " --footer-line --footer-font-size \"12\" --footer-spacing 1 --footer-font-name \"Segoe UI\""    
            };

            return pdfResult;
        }

        public IActionResult ReportEstoqueDist()
        {
            var dados = relEstoqueDist();
            var pdfResult = new ViewAsPdf("ReportEstoqueDist", dados)
            {
                CustomSwitches = "--footer-center \"  Data: " + DateTime.Now.Date.ToString("dd/MM/yyyy") + "  Página: [page]/[toPage]\"" + " --footer-line --footer-font-size \"12\" --footer-spacing 1 --footer-font-name \"Segoe UI\""
            };

            return pdfResult;
        }

        public IActionResult ReportProdIndust()
        {
            List<Models.ProdutoIndust> dados = relProdIndust();
            var pdfResult = new ViewAsPdf("ReportProdIndust", dados)
            {
                CustomSwitches = "--footer-center \"  Data: " + DateTime.Now.Date.ToString("dd/MM/yyyy") + "  Página: [page]/[toPage]\"" + " --footer-line --footer-font-size \"12\" --footer-spacing 1 --footer-font-name \"Segoe UI\""
            };

            return pdfResult;
        }

        public IActionResult ReportProdDist([FromBody] Dictionary<string, string> info)
        {
            string lote = info["lote"];
            var dados = relProdDist(lote);

            var pdfResult = new ViewAsPdf("ReportProdDist", dados)
            {
                CustomSwitches = "--footer-center \"  Data: " + DateTime.Now.Date.ToString("dd/MM/yyyy") + "  Página: [page]/[toPage]\"" + " --footer-line --footer-font-size \"12\" --footer-spacing 1 --footer-font-name \"Segoe UI\""
            };

            return pdfResult;
        }

        public IActionResult ReportDistribuidor()
        {
            var dados = relDistribuidor();
            var pdfResult = new ViewAsPdf("ReportDistribuidor", dados)
            {
                CustomSwitches = "--footer-center \"  Data: " + DateTime.Now.Date.ToString("dd/MM/yyyy") + "  Página: [page]/[toPage]\"" + " --footer-line --footer-font-size \"12\" --footer-spacing 1 --footer-font-name \"Segoe UI\""
            };

            return pdfResult;
        }

        public DataTable relVendaIndust(DateTime datainicio, DateTime datafim)
        {
            var id_indust = HttpContext.User.Claims.ToList()[3].Value;
            DAO.RelatorioDAO rbd = new DAO.RelatorioDAO();
            DataTable dt = rbd.PesquisarVendaIndust(id_indust, datainicio, datafim);

            return dt;
           
        }

        public DataTable relVendaDist(DateTime datainicio, DateTime datafim)
        {
            var id_dist = HttpContext.User.Claims.ToList()[3].Value;
            DAO.RelatorioDAO rbd = new DAO.RelatorioDAO();
            DataTable dt = rbd.PesquisarVendaDist(id_dist, datainicio, datafim);

            return dt;

        }

        public DataTable relEstoqueDist()
        {
            var id_dist = HttpContext.User.Claims.ToList()[3].Value;
            DAO.RelatorioDAO rbd = new DAO.RelatorioDAO();
            DataTable dt = rbd.PesquisarEstoqueDist(id_dist);

            return dt;

        }

        public List<Models.ProdutoIndust> relProdIndust()
        {
            var id_indust = Convert.ToInt32(HttpContext.User.Claims.ToList()[3].Value);
            DAO.ProdutoIndustDAO pibd = new DAO.ProdutoIndustDAO();
            List<Models.ProdutoIndust> produtos = pibd.getProdutos(id_indust);

            return produtos;
        }

        public DataTable relProdDist(string lote)
        {
            var id_dist = Convert.ToInt32(HttpContext.User.Claims.ToList()[3].Value);
            DAO.ProdutoDistDAO pdbd = new DAO.ProdutoDistDAO();

            return pdbd.getProdutos(id_dist, lote);
        }

        public List<Models.Distribuidor> relDistribuidor()
        {
            DAO.DistribuidorDAO dbd = new DAO.DistribuidorDAO();
            List<Models.Distribuidor> distribuidores = dbd.getDistribuidores();

            return distribuidores;
        }
    }
}
