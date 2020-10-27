using sistemarastreamento.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sistemarastreamento.CamadaNegocio
{
    public class CidadeCamadaNegocio
    {
        public List<Models.Estado> ObterEstados()
        {

            CidadeDAO cdao = new CidadeDAO();
            List<Models.Estado> estados =  cdao.ObterEstados();

            return estados;
        }

        public List<Models.Cidade> ObterCidades(int uf)
        {
            CidadeDAO cdao = new CidadeDAO();
            List<Models.Cidade> cidades = cdao.ObterCidades(uf);

            return cidades;
        }

        public string ObterNomeEstado(int uf)
        {

            CidadeDAO cdao = new CidadeDAO();
            string estado = cdao.ObterEstado(uf);

            return estado;
        }

        public string ObterNomeCidade(int id)
        {

            CidadeDAO cdao = new CidadeDAO();
            string cidade = cdao.ObterCidade(id);

            return cidade;
        }
    }
}
