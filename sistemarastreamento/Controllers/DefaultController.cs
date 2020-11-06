using System;
using System.Collections.Generic;
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
    }
}
