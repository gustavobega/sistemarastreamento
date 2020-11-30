using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;

namespace sistemarastreamento.CamadaNegocio
{
    public class ProdDistCamadaNegocio
    {
        public (bool, string) Criar(Models.ProdutoDist prodemp, string lote, int id_estoque)
        {
            bool operacao;
            string msg;

            DAO.ProdutoDistDAO pbd = new DAO.ProdutoDistDAO();
            (operacao, msg) = pbd.Criar(prodemp, lote, id_estoque);

            return (operacao, msg);
        }

        public DataTable Pesquisar(string descricao, int id_dist, string tipo)
        {
            DAO.ProdutoDistDAO pbd = new DAO.ProdutoDistDAO();
            if (descricao == null)
                descricao = "";
            else
                descricao = descricao.ToLower();

            //if (nome.Length > 3)
            return pbd.Pesquisar(descricao, id_dist, tipo);
            //else return new List<Models.Empresa>();
        }

        public Models.ProdutoDist Obter(int id)
        {
            DAO.ProdutoDistDAO pbd = new DAO.ProdutoDistDAO();
            return pbd.Obter(id);
        }

        public Models.ProdutoDist ObterProd(string cod_dist_prod)
        {
            DAO.ProdutoDistDAO pbd = new DAO.ProdutoDistDAO();
            return pbd.ObterProd(cod_dist_prod);
        }

        public DataTable ObterDadosProdDist(int id_estoque)
        {
            DAO.ProdutoDistDAO pbd = new DAO.ProdutoDistDAO();
            return pbd.ObterDadosProdDist(id_estoque);
        }

        public bool Excluir(int id_prod)
        {
            DAO.ProdutoDistDAO pbd = new DAO.ProdutoDistDAO();
            return pbd.Excluir(id_prod);
        }
    }
}
