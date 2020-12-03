using System;
using Newtonsoft.Json;
using System.Text;
using System.Data;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace sistemarastreamento.Controllers
{
    [Authorize("CookieAuth")]
    public class LancamentoIndustController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult IndexListar()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ImportXML()
        {
            IFormFile xmlFile = Request.Form.Files[0];
            bool operacao = false;
            string msg;
            int id_nota = 0;
            string cnpjdist = "";
            string extension = Path.GetExtension(xmlFile.FileName);
            CamadaNegocio.NotaCamadaNegocio ncn = new CamadaNegocio.NotaCamadaNegocio();

            if (extension != ".xml")
                msg = "Selecione um arquivo .xml";
            else
            {
                XmlDocument xmlDoc = new XmlDocument();
                
                try
                {
                    using (MemoryStream str = new MemoryStream())
                    {
                        await xmlFile.CopyToAsync(str);
                        str.Position = 0;
                        xmlDoc.Load(str);
                    }

                    XmlNodeList xnList = xmlDoc.GetElementsByTagName("ide");
                    int numero = Convert.ToInt32(xnList[0]["nNF"].InnerText);

                    if (!ncn.ObterNotaExistente(numero))
                    {
                        (operacao, msg, id_nota, cnpjdist) = ProcessImport(xmlDoc);
                    }
                    else
                    {
                        msg = "Nota Fiscal Já Importada!";
                    }
                }
                catch (Exception e)
                {
                    msg = e.Message;
                }
            }
          
            return Json(new
            {
                operacao,
                msg,
                id_nota,
                cnpjdist
            });
        }

        public IActionResult AlterarEstoque([FromBody] Dictionary<string, string> dados, int id_nota)
        {
            bool operacao = true;
            Models.Distribuidor dist;
            CamadaNegocio.DistribuidorCamadaNegocio dcn = new CamadaNegocio.DistribuidorCamadaNegocio();
            string cnpjdist = Convert.ToInt64(dados["cnpjdist"]).ToString(@"00\.000\.000\/0000\-00");
            dist = dcn.ObterCnpj(cnpjdist);

            List<Models.ItemNota> itens;
            CamadaNegocio.ItemNotaCamadaNegocio icn = new CamadaNegocio.ItemNotaCamadaNegocio();
            itens = icn.Pesquisa(id_nota);

            CamadaNegocio.EstoqueCamadaNegocio ecn = new CamadaNegocio.EstoqueCamadaNegocio();
            Models.Estoque estoque = new Models.Estoque();
            for (int i = 0;i < itens.Count() && operacao; i++)
            {
                estoque.Id_dist = dist.Id;
                estoque.Id_prod = Convert.ToInt32(itens[i].Id_prod);
                estoque.Lote = itens[i].Lote;
                estoque.Saldo = itens[i].Qtde;

                operacao = ecn.AlterarEstoque(estoque);
            }

            return Json(new
            {
                operacao
            });
        }

        public static double GetDouble(string value, double defaultValue)
        {
            double result;
            string output;

            // Check if last seperator==groupSeperator
            string groupSep = System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberGroupSeparator;
            if (value.LastIndexOf(groupSep) + 4 == value.Count())
            {
                bool tryParse = double.TryParse(value, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.CurrentCulture, out result);
                result = tryParse ? result : defaultValue;
            }
            else
            {
                // Unify string (no spaces, only . )
                output = value.Trim().Replace(" ", string.Empty).Replace(",", ".");

                // Split it on points
                string[] split = output.Split('.');

                if (split.Count() > 1)
                {
                    // Take all parts except last
                    output = string.Join(string.Empty, split.Take(split.Count() - 1).ToArray());

                    // Combine token parts with last part
                    output = string.Format("{0}.{1}", output, split.Last());
                }
                // Parse double invariant
                result = double.Parse(output, System.Globalization.CultureInfo.InvariantCulture);
            }
            return result;
        }
        private (bool,string,int,string) ProcessImport(XmlDocument xmlDoc)
        {
            CamadaNegocio.ProdIndustCamadaNegocio picn = new CamadaNegocio.ProdIndustCamadaNegocio();
            List<Models.ProdutoIndust> pi = new List<Models.ProdutoIndust>();

            CamadaNegocio.NotaCamadaNegocio ncn = new CamadaNegocio.NotaCamadaNegocio();
            Models.NotaFiscal notamodelo = new Models.NotaFiscal();

            CamadaNegocio.ItemNotaCamadaNegocio incn = new CamadaNegocio.ItemNotaCamadaNegocio();
            Models.ItemNota itemnota;

            //dados para rastreamento
            List<int> rastroCod = new List<int>();
            List<string> rastroLote = new List<string>();

            XmlNodeList xnList, xnList2;

            bool operacao,entrou;
            entrou = operacao = false;
            string msg = "Produtos sem Cadastro - Códigos: ";
            string cnpjdist = "";

            xnList = xmlDoc.GetElementsByTagName("prod");
            notamodelo.Tipo = HttpContext.User.Claims.ToList()[1].Value;
            for (int i = 0; i < xnList.Count; i++)
            {
                string id_prod = xnList[i]["cProd"].InnerText;

                pi.Add(picn.ObterProd(id_prod));
                if (pi[i] == null)
                {
                    entrou = true;
                    msg += id_prod + ", ";
                }  

            }
            msg = msg.Remove(msg.Length-2);
            msg += ".";

            //todos produtos cadastrados
            if (!entrou)
            {
                //dados nota fiscal
                notamodelo.Cod_indust = Convert.ToInt32(HttpContext.User.Claims.ToList()[3].Value);

                xnList = xmlDoc.GetElementsByTagName("infProt");
                notamodelo.Chave = xnList[0]["chNFe"].InnerText;

                xnList = xmlDoc.GetElementsByTagName("ide");
                notamodelo.Serie = Convert.ToInt32(xnList[0]["serie"].InnerText);
                notamodelo.Numero = Convert.ToInt32(xnList[0]["nNF"].InnerText);
                notamodelo.Data = Convert.ToDateTime(xnList[0]["dhEmi"].InnerText);

                xnList = xmlDoc.GetElementsByTagName("fat");
                notamodelo.Valor_nf = GetDouble(xnList[0]["vLiq"].InnerText, 0d);

                operacao = ncn.Criar(notamodelo);

                if (operacao)
                {
                    xnList = xmlDoc.GetElementsByTagName("prod");
                    xnList2 = xmlDoc.GetElementsByTagName("rastro");

                    for (int i = 0; i < xnList.Count; i++)
                    {
                        itemnota = new Models.ItemNota();
                        itemnota.Id_nota = notamodelo.Id;
                        itemnota.Id_prod = pi[i].Id;
                        itemnota.Lote = xnList2[i]["nLote"].InnerText;
                        string qtdeaux = xnList[i]["qTrib"].InnerText;
                        string qtde = "";
                        for (int j = 0; qtdeaux[j] != '.'; j++)
                            qtde += qtdeaux[j];

                        itemnota.Qtde = Convert.ToInt32(qtde);

                        itemnota.Valor_unit = GetDouble(xnList[i]["vUnTrib"].InnerText, 0d);

                        rastroCod.Add(itemnota.Id_prod);
                        rastroLote.Add(itemnota.Lote);
                        operacao = incn.Criar(itemnota);
                    }
                }
                else
                {
                    return (operacao, "Problemas com os dados da Nota Fiscal!", notamodelo.Id, cnpjdist);
                }

                
            }
            if (!entrou)
            {
                xnList = xmlDoc.GetElementsByTagName("dest");
                cnpjdist = xnList[0]["CNPJ"].InnerText;

                //salvar o rastreio
                CamadaNegocio.RastroCamadaNegocio rcn = new CamadaNegocio.RastroCamadaNegocio();
                rcn.Criar(rastroCod, rastroLote, cnpjdist, notamodelo.Cod_indust);

                return (operacao, "Dados Importados!", notamodelo.Id, cnpjdist);
            }    
            else
                return (false, msg, 0, cnpjdist);
        }
        
        public IActionResult PesquisarIndust(string id_indust, string numero, string tipo)
        {
            CamadaNegocio.NotaCamadaNegocio ncn = new CamadaNegocio.NotaCamadaNegocio();

            List<Models.NotaFiscal> notas = ncn.PesquisarIndust(numero, id_indust, tipo);

            //objeto anônimo
            var notasLimpos = new List<object>();

            if (notas != null)
            {
                foreach (var n in notas)
                {
                    string dataNota = n.Data.ToString();

                    var notasLimpo = new
                    {
                        id = n.Id,
                        data = dataNota,
                        numero = n.Numero,
                        serie = n.Serie,
                        valor_nf = n.Valor_nf
                    };

                    notasLimpos.Add(notasLimpo);
                }
            }

            return Json(new {
                operacao = notas != null,
                notasLimpos
            });
        }

        public IActionResult IndexVisualizar(int id)
        {
            Models.NotaFiscal nota;
            CamadaNegocio.NotaCamadaNegocio ncn = new CamadaNegocio.NotaCamadaNegocio();
            nota = ncn.Obter(id);

            ViewBag.IdNota = nota.Id;
            ViewBag.data = nota.Data;
            ViewBag.numero = nota.Numero;
            ViewBag.serie = nota.Serie;
            ViewBag.chave = nota.Chave;
            ViewBag.valor_nf = "R$" + nota.Valor_nf;

            return View();
        }

        public IActionResult carregaItensNota(int id)
        {
            DataTable dt;
            CamadaNegocio.ItemNotaCamadaNegocio incn = new CamadaNegocio.ItemNotaCamadaNegocio();
            dt = incn.Obter(id);

            return Json(
                JsonConvert.SerializeObject(dt)
            );
        }

        public IActionResult Excluir(int id)
        {
            CamadaNegocio.ItemNotaCamadaNegocio incn = new CamadaNegocio.ItemNotaCamadaNegocio();
            bool operacao = incn.ExcluirItens(id);

            if (operacao)
            {
                CamadaNegocio.NotaCamadaNegocio ncn = new CamadaNegocio.NotaCamadaNegocio();
                operacao = ncn.Excluir(id);
            }
            
            return Json(new
            {
                operacao
            });
        }
    }
}
