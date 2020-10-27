using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sistemarastreamento.CamadaNegocio
{
    public class NotaCamadaNegocio
    {
        public bool Criar(Models.NotaFiscal nota)
        {
            bool operacao;

            DAO.NotaDAO nbd = new DAO.NotaDAO();
            operacao = nbd.Criar(nota);

            return operacao;
        }

        public bool Alterar(Models.NotaFiscal nota)
        {
            bool operacao;

            DAO.NotaDAO nbd = new DAO.NotaDAO();
            operacao = nbd.Alterar(nota);

            return operacao;
        }

        public Models.NotaFiscal Obter(int id)
        {
            DAO.NotaDAO nbd = new DAO.NotaDAO();
            return nbd.Obter(id);
        }
        
        public bool ObterNotaExistente(int numero)
        {
            DAO.NotaDAO nbd = new DAO.NotaDAO();
            return nbd.ObterNotaExistente(numero);
        }

        public List<Models.NotaFiscal> PesquisarIndust(string numero, string id_indust, string tipo)
        {
            DAO.NotaDAO nbd = new DAO.NotaDAO();
            if (numero == null)
                numero = "";

            //if (nome.Length > 3)
            return nbd.PesquisarIndust(numero, id_indust, tipo);
            //else return new List<Models.Empresa>();
        }

        public List<Models.NotaFiscal> PesquisarDist(string numero, string id_dist, string tipo)
        {
            DAO.NotaDAO nbd = new DAO.NotaDAO();
            if (numero == null)
                numero = "";

            //if (nome.Length > 3)
            return nbd.PesquisarDist(numero, id_dist, tipo);
            //else return new List<Models.Empresa>();
        }

        public bool Excluir(int id)
        {
            DAO.NotaDAO nbd = new DAO.NotaDAO();
            return nbd.Excluir(id);
        }
    }
}
