using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Threading.Tasks;

namespace sistemarastreamento.DAO
{
    public class EstoqueDAO
    {
        MySqlPersistencia _bd = new MySqlPersistencia();


        public bool Criar(Models.Estoque estoque)
        {
            var parametros = _bd.GerarParametros();
            string sql,sql2;

            sql2 = @"select * from estoque " +
                       "where id_prod = " + estoque.Id_prod + " and lote = '" + estoque.Lote + "'";

            DataTable dt = _bd.ExecutarSelect(sql2);

            if (dt.Rows.Count > 0)
            {
                sql = @"update estoque set id_prod = @id_prod,lote = @lote where id_prod = @id_prod";
            }
            else
            {
                sql = @"insert into estoque(id_dist,id_prod,lote,saldo)" +
                       "values(@id_dist,@id_prod,@lote,@saldo)";

                parametros.Add("@id_dist", estoque.Id_dist);
                parametros.Add("@saldo", estoque.Saldo);
            }

            parametros.Add("@id_prod", estoque.Id_prod);
            parametros.Add("@lote", estoque.Lote);

            int linhasAfetadas = _bd.ExecutarNonQuery(sql, parametros);

            estoque.Id = _bd.UltimoId;
            
            return linhasAfetadas > 0;

        }

        public bool AlteraEstoque(int qtde, int id_prod)
        {
            var parametros = _bd.GerarParametros();
            string sql,sql2;

            sql2 = @"select * from estoque " +
                       "where id_prod = " + id_prod;

            DataTable dt = _bd.ExecutarSelect(sql2);


            sql = @"update estoque set saldo = @saldo where id_prod = @id_prod";

            DataRow dr = dt.Rows[0];
            int saldoatual = Convert.ToInt32(dr["saldo"]);
                
            parametros.Add("@id_prod", id_prod);
            parametros.Add("@saldo", saldoatual - qtde);

            int linhasAfetadas = _bd.ExecutarNonQuery(sql, parametros);

            return linhasAfetadas > 0;

        }

        public Models.Estoque Obter(int id_prod)
        {
            Models.Estoque estoque = null;

            string select = @"select * 
                              from estoque 
                              where id_prod = " + id_prod;

            DataTable dt = _bd.ExecutarSelect(select);

            if (dt.Rows.Count == 1)
            {
                //ORM - Relacional -> Objeto
                estoque = Map(dt.Rows[0]);
            }

            return estoque;

        }

        public bool VerificaEstoque(string id_prod, int qtde)
        {
            string select = @"SELECT saldo FROM produto_distribuidor as pd inner join produto_industria as pi 
                                on pd.cod_ref = pi.cod_ref and pd.cod_prod_dist = '" + id_prod + "' inner join estoque as e" +
                                " on pi.id = e.id_prod;";

            DataTable dt = _bd.ExecutarSelect(select);
            int saldo = 0;

            if (dt.Rows.Count > 0)
            {
                DataRow row = dt.Rows[0];
                saldo = Convert.ToInt32(row["saldo"]);
            }   

            return saldo >= qtde;
        }

        public bool Excluir(int id_prod)
        {
            string select = @"delete  
                              from estoque 
                              where id_prod = " + id_prod;

            return _bd.ExecutarNonQuery(select) > 0;
        }

        internal Models.Estoque Map(DataRow row)
        {
            Models.Estoque estoque = new Models.Estoque();
            estoque.Id = Convert.ToInt32(row["id"]);
            estoque.Id_dist = Convert.ToInt32(row["id_dist"]);
            estoque.Id_prod = Convert.ToInt32(row["id_prod"]);
            estoque.Lote = row["lote"].ToString();
            estoque.Saldo = Convert.ToInt32(row["saldo"]);

            return estoque;
        }
    }
}
