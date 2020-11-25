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
    public class LancamentoDistController : Controller
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
        public IActionResult Alterar([FromBody] Dictionary<string, string> dados, int id)
        {
            bool operacao = false;
            Models.NotaFiscal nota = new Models.NotaFiscal();
            nota.Id = id;
            nota.Paciente = dados["paciente"];
            nota.Data_cirurgia = Convert.ToDateTime(dados["data_cirurgia"]);
            nota.Medico = dados["medico"];
            nota.Convenio = dados["convenio"];
            nota.Hospital = dados["hospital"];

            CamadaNegocio.NotaCamadaNegocio ncn = new CamadaNegocio.NotaCamadaNegocio();
            ncn.Alterar(nota);

            return Json(
                operacao
            );
        }

        [HttpPost]
        public async Task<IActionResult> ImportXML()
        {
            IFormFile xmlFile = Request.Form.Files[0];
            bool operacao = false;
            string msg;
            int id_nota = 0;
            string infoadicionais = "";
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
                        (operacao, msg, id_nota, infoadicionais) = ProcessImport(xmlDoc);
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
                infoadicionais
            });
        }

        public IActionResult AlteraEstoque(int id_nota)
        {
            bool operacao = true;
            //int id_dist = Convert.ToInt32(HttpContext.User.Claims.ToList()[3].Value);

            List<Models.ItemNota> itens = new List<Models.ItemNota>();
            CamadaNegocio.ItemNotaCamadaNegocio icn = new CamadaNegocio.ItemNotaCamadaNegocio();
            itens = icn.Pesquisa(id_nota);

            CamadaNegocio.ProdDistCamadaNegocio pdcn = new CamadaNegocio.ProdDistCamadaNegocio();
            Models.ProdutoDist pd = new Models.ProdutoDist();

            CamadaNegocio.ProdIndustCamadaNegocio picn = new CamadaNegocio.ProdIndustCamadaNegocio();
            Models.ProdutoIndust pi = new Models.ProdutoIndust();

            CamadaNegocio.EstoqueCamadaNegocio ecn = new CamadaNegocio.EstoqueCamadaNegocio();
            int qtde,id_prod;
            for (int i = 0; i < itens.Count() && operacao; i++)
            {
                pd = pdcn.Obter(itens[i].Id_prod);

                pi = picn.ObterProd(pd.Cod_ref);
                id_prod = pi.Id;
                qtde = itens[i].Qtde;

                operacao = ecn.AlteraEstoque(qtde, id_prod);
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
        private (bool, string, int, string) ProcessImport(XmlDocument xmlDoc)
        {
            CamadaNegocio.ProdDistCamadaNegocio pdcn = new CamadaNegocio.ProdDistCamadaNegocio();
            List<Models.ProdutoDist> pd = new List<Models.ProdutoDist>();

            CamadaNegocio.NotaCamadaNegocio ncn = new CamadaNegocio.NotaCamadaNegocio();
            Models.NotaFiscal notamodelo = new Models.NotaFiscal();

            CamadaNegocio.ItemNotaCamadaNegocio incn = new CamadaNegocio.ItemNotaCamadaNegocio();
            Models.ItemNota itemnota;

            CamadaNegocio.EstoqueCamadaNegocio ecn = new CamadaNegocio.EstoqueCamadaNegocio();

            //dados para rastreamento
            List<string> rastroCod = new List<string>();
            List<string> rastroLote = new List<string>();

            List<string> codProd = new List<string>();

            XmlNodeList xnList, xnList2;

            bool operacao, entrou, nosaldo;
            entrou = operacao = nosaldo = false;
            string msg = "Produtos sem Cadastro - Códigos: ";
            string msg2 = "Tais Itens Não possui Saldo Suficiente - Códigos: ";
            string qtdeaux, qtde;

            xnList = xmlDoc.GetElementsByTagName("prod");
            notamodelo.Tipo = HttpContext.User.Claims.ToList()[1].Value;
            for (int i = 0; i < xnList.Count; i++)
            {
                string id_prod = xnList[i]["cProd"].InnerText;
                codProd.Add(id_prod);
                pd.Add(pdcn.ObterProd(id_prod));
                if (pd[i] == null)
                {
                    entrou = true;
                    msg += id_prod + ", ";
                }
                //verifica estoque junto
                //verifica se há quantidade
                qtdeaux = xnList[i]["qTrib"].InnerText;
                qtde = "";
                for (int j = 0; qtdeaux[j] != '.'; j++)
                    qtde += qtdeaux[j];

                operacao = ecn.VerificaEstoque(id_prod, Convert.ToInt32(qtde));

                if (!operacao)
                {
                    nosaldo = true;
                    msg2 += id_prod + ", ";
                }

            }

            msg = msg.Remove(msg.Length - 2);
            msg += ".";

            msg2 = msg2.Remove(msg2.Length - 2);
            msg2 += ".";

            string dadosadd = "";
            //todos produtos cadastrados e com estoques disponiveis
            if (!entrou && !nosaldo)
            {
                //dados nota fiscal
                notamodelo.Cod_dist = Convert.ToInt32(HttpContext.User.Claims.ToList()[3].Value);

                xnList = xmlDoc.GetElementsByTagName("infProt");
                notamodelo.Chave = xnList[0]["chNFe"].InnerText;

                xnList = xmlDoc.GetElementsByTagName("ide");
                notamodelo.Serie = Convert.ToInt32(xnList[0]["serie"].InnerText);
                notamodelo.Numero = Convert.ToInt32(xnList[0]["nNF"].InnerText);
                notamodelo.Data = Convert.ToDateTime(xnList[0]["dhEmi"].InnerText);

                xnList = xmlDoc.GetElementsByTagName("fat");
                notamodelo.Valor_nf = GetDouble(xnList[0]["vLiq"].InnerText, 0d);

                //dados adicionais
                xnList = xmlDoc.GetElementsByTagName("infCpl");
                dadosadd = xnList[0].InnerText;
                string[] infoadd = dadosadd.Split(",");
                
                if (infoadd.Length > 0)
                { 
                    string[] paciente = infoadd[0].Split(":");
                    if (paciente.Length > 0)
                        notamodelo.Paciente = paciente[1].Trim();

                    if (infoadd.Length >= 2)
                    {
                        string[] medico = infoadd[1].Split(":");
                        if (medico.Length > 0)
                            notamodelo.Medico = medico[1].Trim();

                        if (infoadd.Length >= 3)
                        {
                            string[] convenio = infoadd[2].Split(":");
                            if (convenio.Length > 0)
                                notamodelo.Convenio = convenio[1].Trim();
                                
                            if (infoadd.Length >= 4)
                            {
                                string[] data_cirurgia = infoadd[4].Split(":");

                                if (data_cirurgia.Length > 0)
                                    notamodelo.Data_cirurgia = DateTime.ParseExact(data_cirurgia[1].Trim(), "dd/MM/yyyy", null);

                                if (infoadd.Length >= 5)
                                {
                                    string[] hospital = infoadd[5].Split(":");

                                    if (hospital.Length == 0)
                                        notamodelo.Hospital = "";
                                    else
                                        notamodelo.Hospital = hospital[1].Trim();
                                }
                            }
                        }
                    }
                }

                operacao = ncn.Criar(notamodelo);

                if (operacao)
                {
                    xnList = xmlDoc.GetElementsByTagName("prod");
                    xnList2 = xmlDoc.GetElementsByTagName("infAdProd");
                    for (int i = 0; i < xnList.Count; i++)
                    {
                        itemnota = new Models.ItemNota();
                        itemnota.Id_nota = notamodelo.Id;
                        itemnota.Id_prod = pd[i].Id;

                        //lote
                        string dadosinfo = xnList2[i].InnerText;
                        string[] dados = dadosinfo.Split(";");
                        string[] dados2 = dados[1].Split(":");
                        string lote = dados2[1].Split(" ")[1];

                        itemnota.Lote = lote;

                        qtdeaux = xnList[i]["qTrib"].InnerText;
                        qtde = "";
                        for (int j = 0; qtdeaux[j] != '.'; j++)
                            qtde += qtdeaux[j];

                        itemnota.Qtde = Convert.ToInt32(qtde);

                        itemnota.Valor_unit = GetDouble(xnList[i]["vUnTrib"].InnerText, 0d);

                        rastroCod.Add(codProd[i]);
                        rastroLote.Add(itemnota.Lote);
                        operacao = incn.Criar(itemnota);
                    }
                }
                else
                {
                    return (operacao, "Problemas com os dados da Nota Fiscal!", notamodelo.Id, "Erro");
                }

            }

            if (entrou)
                return (false, msg, 0, "Erro");
            else if (nosaldo)
                return (false, msg2, notamodelo.Id, dadosadd);
            else
            {
                xnList = xmlDoc.GetElementsByTagName("dest");
                Models.Destino destino = new Models.Destino();
                destino.Nome = xnList[0]["xNome"].InnerText;

                xnList = xmlDoc.GetElementsByTagName("enderDest");
                destino.Rua = xnList[0]["xLgr"].InnerText;
                destino.Numero = Convert.ToInt32(xnList[0]["nro"].InnerText);
                destino.Bairro = xnList[0]["xBairro"].InnerText;
                destino.Cidade = xnList[0]["xMun"].InnerText;
                destino.Estado = xnList[0]["UF"].InnerText;
                destino.Cep = xnList[0]["CEP"].InnerText;

                destino.salvar(rastroCod,rastroLote);

                return (operacao, "Dados Importados!", notamodelo.Id, dadosadd);
            }
                 
        }

        public IActionResult IndexVisualizar(int id)
        {
            Models.NotaFiscal nota;
            CamadaNegocio.NotaCamadaNegocio ncn = new CamadaNegocio.NotaCamadaNegocio();
            nota = ncn.Obter(id);

            ViewBag.paciente = nota.Paciente;
            ViewBag.data_cirurgia = nota.Data_cirurgia.ToString("dd/MM/yyyy");
            ViewBag.medico = nota.Medico;
            ViewBag.convenio = nota.Convenio;
            ViewBag.hospital = nota.Hospital;

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
            dt = incn.ObterDist(id);

            return Json(
                JsonConvert.SerializeObject(dt)
            );
        }

        public IActionResult PesquisarDist(string id_dist, string numero, string tipo)
        {
            CamadaNegocio.NotaCamadaNegocio ncn = new CamadaNegocio.NotaCamadaNegocio();

            List<Models.NotaFiscal> notas = ncn.PesquisarDist(numero, id_dist, tipo);

            //objeto anônimo
            var notasLimpos = new List<object>();

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

            return Json(notasLimpos);
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
