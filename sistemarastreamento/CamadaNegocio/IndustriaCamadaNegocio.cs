using sistemarastreamento.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sistemarastreamento.CamadaNegocio
{
    public class IndustriaCamadaNegocio
    {
        public bool Criar(Models.Industria industria)
        {
            bool operacao;
            
            DAO.IndustriaDAO ibd = new DAO.IndustriaDAO();
            operacao = ibd.Criar(industria);
           
            return operacao;
        }

        public Models.Industria Obter(int id)
        {
            DAO.IndustriaDAO ibd = new DAO.IndustriaDAO();
            return ibd.Obter(id);
        }

        public Models.Industria Obter(string email)
        {
            DAO.IndustriaDAO ibd = new DAO.IndustriaDAO();
            return ibd.Obter(email);
        }

        public Models.Perfil ObterPerfil(string email)
        {
            DAO.UsuarioDAO ubd = new DAO.UsuarioDAO();
            return ubd.ObterPerfil(email);
        }
        
        public List<Models.Industria> Pesquisar(string nome, string tipo)
        {
            DAO.IndustriaDAO ibd = new DAO.IndustriaDAO();
            if (nome == null)
                nome = "";
            else
            nome = nome.ToLower();

            //if (nome.Length > 3)
                return ibd.Pesquisar(nome, tipo);
            //else return new List<Models.Empresa>();
        }

        public bool Excluir(int id)
        {
            DAO.IndustriaDAO ibd = new DAO.IndustriaDAO();
            return ibd.Excluir(id);
        }

        public List<Models.Perfil> ObterPerfis(string nome)
        {
            DAO.PerfilDAO pbd = new DAO.PerfilDAO();
            return pbd.Pesquisar(nome);
        }
    }
}
