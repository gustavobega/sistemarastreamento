using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sistemarastreamento.CamadaNegocio
{
    public class ItemNotaCamadaNegocio
    {
        public bool Criar(Models.ItemNota itemnota)
        {
            bool operacao;

            DAO.ItemNotaDAO inbd = new DAO.ItemNotaDAO();
            operacao = inbd.Criar(itemnota);

            return operacao;
        }

        public DataTable Obter(int id)
        {
            DAO.ItemNotaDAO inbd = new DAO.ItemNotaDAO();
            return inbd.Obter(id);
        }

        public DataTable ObterDist(int id)
        {
            DAO.ItemNotaDAO inbd = new DAO.ItemNotaDAO();
            return inbd.ObterDist(id);
        }

        public List<Models.ItemNota> Pesquisa(int id_nota)
        {
            DAO.ItemNotaDAO inbd = new DAO.ItemNotaDAO();
            return inbd.Pesquisa(id_nota);
        }

        public bool ExcluirItens(int id)
        {
            DAO.ItemNotaDAO inbd = new DAO.ItemNotaDAO();
            return inbd.ExcluirItens(id);
        }

    }
}
