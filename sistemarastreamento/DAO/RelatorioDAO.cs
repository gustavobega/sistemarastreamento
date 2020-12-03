using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace sistemarastreamento.DAO
{
    public class RelatorioDAO
    {
        MySqlPersistencia _bd = new MySqlPersistencia();
        public DataTable PesquisarVendaIndust(string id_indust, DateTime datainicio, DateTime datafim)
        {
            string select = @"SELECT * FROM nota_fiscal as nf INNER JOIN 
                                item_nota as itn ON nf.id = itn.id_nota 
                                and id_indust = " + id_indust + " INNER JOIN produto_industria as pi ON itn.id_prod = pi.id and nf.data >= @datainicio and nf.data <= @datafim  ORDER BY numero";


            var parametros = _bd.GerarParametros();
            parametros.Add("@datainicio", datainicio);
            parametros.Add("@datafim", datafim);

            DataTable dt = _bd.ExecutarSelect(select, parametros);

            return dt;
        }

        public DataTable PesquisarVendaDist(string id_dist, DateTime datainicio, DateTime datafim)
        {
            string select = @"SELECT * FROM nota_fiscal as nf INNER JOIN 
                                item_nota as itn ON nf.id = itn.id_nota 
                                and id_dist = " + id_dist + " and nf.data >= @datainicio and nf.data <= @datafim INNER JOIN produto_distribuidor as pd ON itn.id_prod = pd.id INNER JOIN produto_industria as pi ON pi.cod_ref = pd.cod_ref ORDER BY numero";


            var parametros = _bd.GerarParametros();
            parametros.Add("@datainicio", datainicio);
            parametros.Add("@datafim", datafim);

            DataTable dt = _bd.ExecutarSelect(select, parametros);

            return dt;
        }

        public DataTable PesquisarEstoqueDist(string id_dist)
        {
            string select = @"SELECT * FROM estoque as e INNER JOIN produto_distribuidor 
                                as pd ON e.id_prod = pd.id and pd.id_dist = " + id_dist + " INNER JOIN produto_industria as pi ON pd.cod_ref = pi.cod_ref;";


            DataTable dt = _bd.ExecutarSelect(select);

            return dt;
        }
  
    }
}
