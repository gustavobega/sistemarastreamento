using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sistemarastreamento.CamadaNegocio
{
    public class EstoqueCamadaNegocio
    {
        public bool CriarEstoque(Models.Estoque estoque)
        {
            bool operacao;

            DAO.EstoqueDAO ebd = new DAO.EstoqueDAO();
            operacao = ebd.Criar(estoque);

            return operacao;
        }

        public bool AlteraEstoque(int qtde,int id_prod)
        {
            bool operacao;

            DAO.EstoqueDAO ebd = new DAO.EstoqueDAO();
            operacao = ebd.AlteraEstoque(qtde, id_prod);

            return operacao;
        }

        public Models.Estoque Obter(int id_prod)
        {
            DAO.EstoqueDAO ebd = new DAO.EstoqueDAO();
            return ebd.Obter(id_prod);
        }

        public bool Excluir(int id_prod)
        {
            DAO.EstoqueDAO ebd = new DAO.EstoqueDAO();
            return ebd.Excluir(id_prod);
        }

        public bool VerificaEstoque(string id_prod, int qtde)
        {
            DAO.EstoqueDAO ebd = new DAO.EstoqueDAO();
            return ebd.VerificaEstoque(id_prod, qtde);
        }
    }
}
