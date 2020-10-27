using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace sistemarastreamento.Controllers
{
    public class RelatorioController : Controller
    {
        [Authorize("CookieAuth")]
        public IActionResult RelVendaIndust()
        {
            return View();
        }
    }
}
