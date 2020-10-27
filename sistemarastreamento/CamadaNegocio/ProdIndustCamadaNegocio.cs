using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sistemarastreamento.CamadaNegocio
{
    public class ProdIndustCamadaNegocio
    {
        public bool Criar(Models.ProdutoIndust prodindust)
        {
            bool operacao;

            DAO.ProdutoIndustDAO pbd = new DAO.ProdutoIndustDAO();
            operacao = pbd.Criar(prodindust);

            return operacao;
        }

        public List<Models.ProdutoIndust> Pesquisar(string produto, int id_indust, string tipo)
        {
            DAO.ProdutoIndustDAO pbd = new DAO.ProdutoIndustDAO();
            if (produto == null)
                produto = "";
            else
                produto = produto.ToLower();

            //if (nome.Length > 3)
            return pbd.Pesquisar(produto, id_indust, tipo);
            //else return new List<Models.Empresa>();
        }

        public Models.ProdutoIndust Obter(int id)
        {
            DAO.ProdutoIndustDAO pbd = new DAO.ProdutoIndustDAO();
            return pbd.Obter(id);
        }

        public Models.ProdutoIndust ObterProd(string cod_ref)
        {
            DAO.ProdutoIndustDAO pbd = new DAO.ProdutoIndustDAO();
            return pbd.ObterProd(cod_ref);
        }

        public bool Excluir(int id)
        {
            DAO.ProdutoIndustDAO pbd = new DAO.ProdutoIndustDAO();
            return pbd.Excluir(id);
        }
    }
}
