using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sistemarastreamento.CamadaNegocio
{
    public class DistribuidorCamadaNegocio
    {
        public bool Criar(Models.Distribuidor distribuidor)
        {
            bool operacao;

            DAO.DistribuidorDAO dbd = new DAO.DistribuidorDAO();
            operacao = dbd.Criar(distribuidor);

            return operacao;
        }

        public Models.Distribuidor Obter(int id)
        {
            DAO.DistribuidorDAO dbd = new DAO.DistribuidorDAO();
            return dbd.Obter(id);
        }

        public Models.Distribuidor ObterCnpj(string cnpj)
        {
            DAO.DistribuidorDAO dbd = new DAO.DistribuidorDAO();
            return dbd.ObterCnpj(cnpj);
        }

        public Models.Distribuidor Obter(string email)
        {
            DAO.DistribuidorDAO dbd = new DAO.DistribuidorDAO();
            return dbd.Obter(email);
        }

        public Models.Perfil ObterPerfil(string email)
        {
            DAO.UsuarioDAO ubd = new DAO.UsuarioDAO();
            return ubd.ObterPerfil(email);
        }

        public List<Models.Distribuidor> Pesquisar(string nome, string tipo)
        {
            DAO.DistribuidorDAO dbd = new DAO.DistribuidorDAO();
            if (nome == null)
                nome = "";
            else
                nome = nome.ToLower();

            //if (nome.Length > 3)
            return dbd.Pesquisar(nome,tipo);
            //else return new List<Models.Empresa>();
        }

        public bool Excluir(int id)
        {
            DAO.DistribuidorDAO dbd = new DAO.DistribuidorDAO();
            return dbd.Excluir(id);
        }

        public Models.Distribuidor Visualizar(int id)
        {
            DAO.DistribuidorDAO dbd = new DAO.DistribuidorDAO();
            return dbd.Obter(id);
        }
    }
}
