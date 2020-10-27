using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace sistemarastreamento.Controllers
{
    public class CidadeController : Controller
    {
        public IActionResult ObterEstados()
        {
            CamadaNegocio.CidadeCamadaNegocio ccn =
               new CamadaNegocio.CidadeCamadaNegocio();

            return Json(ccn.ObterEstados());

        }

        public IActionResult ObterCidades(int uf)
        {
            CamadaNegocio.CidadeCamadaNegocio ccn =
                new CamadaNegocio.CidadeCamadaNegocio();

            return Json(ccn.ObterCidades(uf));
        }
    }
}