using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
    }
}
