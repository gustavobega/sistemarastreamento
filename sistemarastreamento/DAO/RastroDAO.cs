using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;

namespace sistemarastreamento.DAO
{
    public class RastroDAO
    {
        MySqlPersistencia _bd = new MySqlPersistencia();
        public bool Criar_rastro(Dictionary<int, string> rastro, int id_indust, string cnpjdist)
        {
            string sql;
            int linhasAfetadas;
            int id;

            cnpjdist = Convert.ToInt64(cnpjdist).ToString(@"00\.000\.000\/0000\-00");
            string sqldist = "select * from distribuidor where cnpj = @cnpj";
            var parametros = _bd.GerarParametros();
            parametros.Add("@cnpj", cnpjdist);

            DataTable dt = _bd.ExecutarSelect(sqldist, parametros);

            if (dt.Rows.Count > 0)
            {
                int id_dist = Convert.ToInt32(dt.Rows[0]["id"]);
                foreach (var item in rastro)
                {
                    sql = @"insert into rastro_industria(ri_id_indust,ri_id_prod,ri_lote) values(" + id_indust + "," + item.Key + "," + item.Value + ")";
                    linhasAfetadas = _bd.ExecutarNonQuery(sql);
                    if (linhasAfetadas > 0)
                    {
                        id = _bd.UltimoId;

                        sql = @"insert into rastro_distribuidor(rd_id_ri,rd_id_dist) values(" + id + "," + id_dist + ")";

                        linhasAfetadas = _bd.ExecutarNonQuery(sql);
                    }   
                }
            }
            
            return true;
        }

        public bool Criar_Destino(Dictionary<string,string> rastro, Models.Destino destino)
        {
            string sql;
            foreach (var item in rastro)
            {
                sql = @"select rd_id from rastro_industria as ri inner join rastro_distribuidor 
                        as rd ON ri.ri_id = rd.rd_id_ri inner join produto_distribuidor 
                        as pd ON pd.cod_prod_dist = '" + item.Key + "' and ri.ri_lote = '" + item.Value + "'";

                DataTable dt = _bd.ExecutarSelect(sql);
                if (dt.Rows.Count > 0)
                {
                    int rd_id = Convert.ToInt32(dt.Rows[0][0]);

                    sql = @"insert into rastro_destino(rdest_rd_id,rdest_nome,rdest_rua,rdest_numero,rdest_bairro,rdest_cidade,rdest_estado,rdest_cep)
                             values(" + rd_id + ",'" + destino.Nome + "','" + destino.Rua + "'," 
                             + destino.Numero + ",'" + destino.Bairro + "','" + destino.Cidade 
                             + "','" + destino.Estado + "','" + destino.Cep + "')";

                    _bd.ExecutarNonQuery(sql);
                }

            }

            return true;
        }

        public DataTable Buscar(string lote, string codigo, string hospital)
        {
            string sql = @"select descricao,ri_id_indust,rd_id_dist,rdest_nome,rdest_rua,rdest_numero,rdest_bairro,rdest_cidade,rdest_estado,rdest_cep from rastro_industria as ri inner join produto_industria as pi on ri.ri_id_prod = pi.id and pi.cod_ref = '" + codigo + "' and ri.ri_lote = '" + lote + "' inner join rastro_distribuidor as rd on ri.ri_id = rd.rd_id_ri inner join rastro_destino on rd.rd_id = rdest_rd_id and rdest_nome = '" + hospital + "';";

            DataTable dt = _bd.ExecutarSelect(sql);

            return dt;
        }
    }
}
