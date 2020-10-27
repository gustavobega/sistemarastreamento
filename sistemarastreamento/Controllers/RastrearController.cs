using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace sistemarastreamento.Controllers
{
    [Authorize("CookieAuth")]
    public class RastrearController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

    }
}