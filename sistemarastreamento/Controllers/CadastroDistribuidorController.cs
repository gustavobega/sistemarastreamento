﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mail;

namespace sistemarastreamento.Controllers
{
    public class CadastroDistribuidorController : Controller
    {
        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Editar(int id)
        {
            ViewBag.IdEditar = id;
            return View("Index");
        }
        [HttpPost]
        public IActionResult Criar([FromBody] Dictionary<string, string> dados, int id)
        {
            bool operacao;

            Models.Distribuidor distribuidor = new Models.Distribuidor();

            distribuidor.Id = id;
            distribuidor.CNPJ = dados["cnpj"];
            distribuidor.Nome = dados["nome"];
            distribuidor.Ie = dados["ie"];
            distribuidor.Representante = dados["representante"];
            distribuidor.Rua = dados["rua"];
            distribuidor.Numero = Convert.ToInt32(dados["numero"]);
            distribuidor.Bairro = dados["bairro"];
            distribuidor.Telefone = dados["telefone"];
            distribuidor.Email = dados["email"];
            distribuidor.Senha = dados["senha"];
            distribuidor.Estado = Convert.ToInt32(dados["estado"]);
            distribuidor.Cidade = Convert.ToInt32(dados["cidade"]);

            CamadaNegocio.DistribuidorCamadaNegocio
                dcn = new CamadaNegocio.DistribuidorCamadaNegocio();
            operacao = dcn.Criar(distribuidor);
            string email = distribuidor.Email;
            string senha = distribuidor.Senha;

            return Json(new
            {
                operacao,
                email,
                senha
            });
        }
        [Authorize("CookieAuth")]
        public IActionResult indexListar()
        {
            return View();
        }

        public IActionResult Pesquisar(string nome, string tipo)
        {
            CamadaNegocio.DistribuidorCamadaNegocio dcn = new CamadaNegocio.DistribuidorCamadaNegocio();

            List<Models.Distribuidor> distribuidores = dcn.Pesquisar(nome, tipo);

            //objeto anônimo
            var distribuidoresLimpos = new List<object>();

            if (distribuidores != null)
            {
                foreach (var d in distribuidores)
                {
                    var distribuidorLimpo = new
                    {
                        id = d.Id,
                        nome = d.Nome,
                        cnpj = d.CNPJ,
                        telefone = d.Telefone,
                        representante = d.Representante,
                        email = d.Email
                    };

                    distribuidoresLimpos.Add(distribuidorLimpo);
                }
            }

            return Json(new {
                operacao = distribuidores != null,
                distribuidoresLimpos
            });
        }
        [HttpDelete]
        public IActionResult Excluir(int id)
        {
            CamadaNegocio.DistribuidorCamadaNegocio dcn = new CamadaNegocio.DistribuidorCamadaNegocio();
            bool operacao = dcn.Excluir(id);

            return Json(new
            {
                operacao
            });
        }

        public IActionResult IndexVisualizar(int id)
        {
            Models.Distribuidor distribuidor;
            CamadaNegocio.DistribuidorCamadaNegocio dcn = new CamadaNegocio.DistribuidorCamadaNegocio();
            CamadaNegocio.CidadeCamadaNegocio ccn = new CamadaNegocio.CidadeCamadaNegocio();
            distribuidor = dcn.Obter(id);

            ViewBag.cnpj = distribuidor.CNPJ;
            ViewBag.ie = distribuidor.Ie;
            ViewBag.nome = distribuidor.Nome;
            ViewBag.representante = distribuidor.Representante;
            ViewBag.telefone = distribuidor.Telefone;
            var estado = ccn.ObterNomeEstado(distribuidor.Estado);
            var cidade = ccn.ObterNomeCidade(distribuidor.Cidade);
            ViewBag.uf = estado;
            ViewBag.cidade = cidade;
            ViewBag.rua = distribuidor.Rua;
            ViewBag.numero = distribuidor.Numero;
            ViewBag.bairro = distribuidor.Bairro;
            DAO.UsuarioDAO ubd = new DAO.UsuarioDAO();
            var perfil = ubd.ObterPerfil(distribuidor.Email);
            ViewBag.perfil = perfil.Nome;
            ViewBag.email = distribuidor.Email;
            ViewBag.senha = distribuidor.Senha;
            return View();
        }

        public IActionResult Obter(int id)
        {
            CamadaNegocio.DistribuidorCamadaNegocio dcn = new CamadaNegocio.DistribuidorCamadaNegocio();

            return Json(dcn.Obter(id));
        }

        public IActionResult ObterEditar(int id)
        {
            CamadaNegocio.DistribuidorCamadaNegocio dcn = new CamadaNegocio.DistribuidorCamadaNegocio();
            Models.Distribuidor distribuidor = dcn.Obter(id);
            Models.Perfil perfil = dcn.ObterPerfil(distribuidor.Email);
            return Json(new
            {
                distribuidor,
                perfil
            });
        }

        public IActionResult IndexEmail(string emaildist)
        {
            ViewBag.emaildist = emaildist;
            ViewBag.emailindust = HttpContext.User.Claims.ToList()[4].Value;
            return View();
        }

        public IActionResult IndexSMS(string telldist)
        {
            ViewBag.telldist = telldist;
            return View();
        }

        public IActionResult EnviarEmail([FromBody] Dictionary<string, string> dados)
        {
            string destino = dados["destino"];
            string origem = dados["origem"];
            string assunto = dados["assunto"];
            string mensagem = dados["mensagem"];

            MailMessage mm = new MailMessage();
            mm.To.Add(destino);

            mm.From = new MailAddress(origem);

            mm.Subject = assunto;
            mm.Body = mensagem;

            SmtpClient smtp = new SmtpClient();
            smtp.Port = 587;
            smtp.UseDefaultCredentials = true;
            smtp.EnableSsl = true;
            if (origem.Contains("hotmail") || origem.Contains("outlook"))
            {
                smtp.Host = "smtp.live.com";
                smtp.Credentials = new System.Net.NetworkCredential(origem, "palmeiras159");
            }
            else if (origem.Contains("gmail"))
            {
                smtp.Host = "smtp.gmail.com";
                smtp.Credentials = new System.Net.NetworkCredential(origem, "palmeiras159");
            }

            try
            {
                smtp.Send(mm);
            }
            catch (Exception e) { }

            return Json(new
            {
                operacao = true
            });
        }

        public IActionResult EnviarSMS([FromBody] Dictionary<string, string> dados)
        {
            string telefone = dados["telefone"];
            string mensagem = dados["mensagem"];

            return Json(new
            {
                operacao = true
            });
        }

    }
}
