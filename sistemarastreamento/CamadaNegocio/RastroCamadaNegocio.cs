using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;

namespace sistemarastreamento.CamadaNegocio
{
    public class RastroCamadaNegocio
    {
        public void Criar(Dictionary<int,string> rastro, string cnpjdist, int id_indust)
        {
            DAO.RastroDAO rbd = new DAO.RastroDAO();
            rbd.Criar_rastro(rastro, id_indust, cnpjdist);
        }

        public DataTable Buscar(string lote, string codigo, string hospital)
        {
            DAO.RastroDAO rbd = new DAO.RastroDAO();
            return rbd.Buscar(lote, codigo, hospital);
        }
    }
}
