using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MySqlX.XDevAPI;
using sistemarastreamento.DAO;

namespace sistemarastreamento.Controllers
{
    public class LoginController : Controller
    {
        [AllowAnonymous]
        public IActionResult Index()
        {
            if (HttpContext.User.Claims.Count() > 0)
                return Redirect("/Default");
            else
                return View();
        }

        public IActionResult Validar([FromBody] Dictionary<string, string> dados)
        {
            Models.Usuario usuario = new Models.Usuario();
            usuario.Email = dados["email"];
            usuario.Senha = dados["senha"];
            usuario.Tipo = dados["tipo"];
            UsuarioDAO udao = new UsuarioDAO();
            int id;
            Models.Usuario usuarioOk = udao.Validar(usuario);

            if (usuarioOk != null)
            {
                if (usuario.Tipo == "Indústria")
                {
                    CamadaNegocio.IndustriaCamadaNegocio icn = new CamadaNegocio.IndustriaCamadaNegocio();
                    Models.Industria industria = icn.Obter(usuarioOk.Email);
                    id = industria.Id;
                }
                else
                {
                    CamadaNegocio.DistribuidorCamadaNegocio dcn = new CamadaNegocio.DistribuidorCamadaNegocio();
                    Models.Distribuidor distribuidor = dcn.Obter(usuarioOk.Email);
                    id = distribuidor.Id;
                }
                    
                #region Criando as cookie de autenticação

                var usuarioClaims = new List<Claim>() {

                    new Claim("usuarioId", usuarioOk.Id.ToString()),
                    new Claim("usuarioTipo", usuarioOk.Tipo.ToString()),
                    new Claim("usuarioPerfil", usuarioOk.Perfil.ToString()),
                    new Claim("usuarioTipoId", id.ToString())
                };

                var identificacao = new ClaimsIdentity(usuarioClaims, "Identificação do Usuario");
                var principal = new ClaimsPrincipal(identificacao);

                //gerar a cookie
                Microsoft.AspNetCore.Authentication.AuthenticationHttpContextExtensions.SignInAsync(HttpContext, principal);

                #endregion

                return Json(new
                {
                    operacao = true,
                });
            }

            return Json(new
            {
                operacao = false,
            });

        }

        public IActionResult Criar([FromBody] Dictionary<string, string> dados)
        {
            bool operacao;

            Models.Usuario usuario = new Models.Usuario();
            usuario.Email = dados["email"];
            usuario.Senha = dados["senha"];
            usuario.Tipo = dados["tipo"];
            usuario.Perfil = Convert.ToInt32(dados["perfilId"]);
            UsuarioDAO udao = new UsuarioDAO();

            operacao = udao.Criar(usuario);

            return Json(new
            {
                operacao
            });
        }

        public IActionResult AlterarPerfil([FromBody] Dictionary<string, string> dados)
        {
            bool operacao;

            string email = dados["email2"];
            int idPerfil = Convert.ToInt32(dados["perfilId"]);
            UsuarioDAO udao = new UsuarioDAO();
            operacao = udao.AlterarPerfil(email, idPerfil);

            return Json(new
            {
                operacao
            });
        }

        public IActionResult Sair()
        {
            //excluindo a cookie
            Microsoft.AspNetCore.Authentication.AuthenticationHttpContextExtensions.SignOutAsync(HttpContext);

            return Redirect("/Login");
        }

    }
}