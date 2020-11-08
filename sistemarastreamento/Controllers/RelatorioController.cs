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
        public IActionResult ReportVendaIndust()
        {
            var dados = relVendaIndust();
            var pdfResult = new ViewAsPdf("ReportVendaIndust", dados)
            {
                CustomSwitches = "--footer-center \"  Data: " + DateTime.Now.Date.ToString("dd/MM/yyyy") + "  Página: [page]/[toPage]\"" + " --footer-line --footer-font-size \"12\" --footer-spacing 1 --footer-font-name \"Segoe UI\""
            };

            return pdfResult;
        }

        public IActionResult ReportVendaDist()
        {
            var dados = relVendaDist();
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

        public DataTable relVendaIndust()
        {
            var id_indust = HttpContext.User.Claims.ToList()[3].Value;
            DAO.RelatorioDAO rbd = new DAO.RelatorioDAO();
            DataTable dt = rbd.PesquisarVendaIndust(id_indust);

            return dt;
           
        }

        public DataTable relVendaDist()
        {
            var id_dist = HttpContext.User.Claims.ToList()[3].Value;
            DAO.RelatorioDAO rbd = new DAO.RelatorioDAO();
            DataTable dt = rbd.PesquisarVendaDist(id_dist);

            return dt;

        }

        public DataTable relEstoqueDist()
        {
            var id_dist = HttpContext.User.Claims.ToList()[3].Value;
            DAO.RelatorioDAO rbd = new DAO.RelatorioDAO();
            DataTable dt = rbd.PesquisarEstoqueDist(id_dist);

            return dt;

        }
    }
}
