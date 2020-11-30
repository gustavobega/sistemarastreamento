using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sistemarastreamento.CamadaNegocio
{
    public class EstoqueCamadaNegocio
    {
        public bool AlterarEstoque(Models.Estoque estoque)
        {
            bool operacao;

            DAO.EstoqueDAO ebd = new DAO.EstoqueDAO();
            operacao = ebd.AtualizaEstoque(estoque);

            return operacao;
        }

        public bool AtualizaEstoque(int id_prod, string lote, int qtde)
        {
            bool operacao;

            DAO.EstoqueDAO ebd = new DAO.EstoqueDAO();
            operacao = ebd.AtualizaEstoque(id_prod, lote, qtde);

            return operacao;
        }

        public Models.Estoque Obter(int id_prod)
        {
            DAO.EstoqueDAO ebd = new DAO.EstoqueDAO();
            return ebd.Obter(id_prod);
        }

        public int ObterTodosEstoque(int id_prod)
        {
            DAO.EstoqueDAO ebd = new DAO.EstoqueDAO();
            return ebd.ObterTodosEstoque(id_prod);
        }

        public bool Excluir(int id)
        {
            DAO.EstoqueDAO ebd = new DAO.EstoqueDAO();
            return ebd.Excluir(id);
        }

        public bool VerificaEstoque(string id_prod, int qtde)
        {
            DAO.EstoqueDAO ebd = new DAO.EstoqueDAO();
            return ebd.VerificaEstoque(id_prod, qtde);
        }
    }
}
