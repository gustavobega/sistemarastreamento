using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace sistemarastreamento.Controllers
{
    public class DefaultController : Controller
    {
        public DefaultController(AppSettings appconfig)
        {
            //obtendo injeção de dependencia
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult geraGraficoAno(string ano)
        {
            List<int> notas = QtdNotasMeses(ano);

            return Json(notas);
        }

        public IActionResult geraGraficoEstoque()
        {
            List<string> produto;
            List<int> saldo;
            (produto, saldo) = QtdEstoqueProd();

            return Json(new
            {
                produto,
                saldo
            });      
        }

        public IActionResult geraGraficoGanho(string ano)
        {
            List<decimal> ganhos = GanhosMeses(ano);

            return Json(ganhos);
        }

        private List<int> QtdNotasMeses(string ano)
        {
            var tipo = HttpContext.User.Claims.ToList()[1].Value;
            var id = Convert.ToInt32(HttpContext.User.Claims.ToList()[3].Value);
            DAO.NotaDAO nbd = new DAO.NotaDAO();
            
            if (tipo == "Indústria")
                return nbd.getQtdNotaMesIndustria(id, ano);
            else
                return nbd.getQtdNotaMesDistribuidor(id, ano);
        }

        private (List<string>, List<int>) QtdEstoqueProd()
        {

            var id = Convert.ToInt32(HttpContext.User.Claims.ToList()[3].Value);
            DAO.ProdutoDistDAO pdbd = new DAO.ProdutoDistDAO();

            DataTable dt = pdbd.getEstoqueProd(id);
            var saldo = new List<int>(); 
            var produto = new List<string>();

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow dr = dt.Rows[i];
                produto.Add(dr["descricao"].ToString());
                saldo.Add(Convert.ToInt32(dr["saldo"]));
            }

            return (produto, saldo);
        }

        private List<decimal> GanhosMeses(string ano)
        {
            var tipo = HttpContext.User.Claims.ToList()[1].Value;
            var id = Convert.ToInt32(HttpContext.User.Claims.ToList()[3].Value);
            DAO.NotaDAO nbd = new DAO.NotaDAO();

            if (tipo == "Indústria")
                return nbd.getGanhoMesIndustria(id, ano);
            else
                return nbd.getGanhoMesDistribuidor(id, ano);
        }
    }
}
