﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Threading.Tasks;

namespace sistemarastreamento.DAO
{
    public class EstoqueDAO
    {
        MySqlPersistencia _bd = new MySqlPersistencia();
        
        //adiciona no estoque do distribuidor
        public bool AtualizaEstoque(Models.Estoque estoque)
        {
            var parametros = _bd.GerarParametros();
            string sql,sql2;
            int saldoAtual = 0;
            int id_prod = 0;
            sql2 = @"select pd.id, saldo from produto_industria as pi inner join produto_distribuidor as pd on pi.id = " + estoque.Id_prod + " and pd.cod_ref = pi.cod_ref inner join estoque as e on e.id_prod = pd.id and e.lote = '" + estoque.Lote + "'";

            DataTable dt = _bd.ExecutarSelect(sql2);

            if (dt.Rows.Count > 0)
            {
                sql = @"update estoque set saldo = @saldo where id_prod = @id_prod and lote = @lote";
                DataRow dr = dt.Rows[0];
                saldoAtual = Convert.ToInt32(dr["saldo"]);
                id_prod = Convert.ToInt32(dr["id"]);
            }
            else
            {
                sql = @"insert into estoque(id_dist,id_prod,lote,saldo)" +
                    "values(@id_dist,@id_prod,@lote,@saldo)";

                parametros.Add("@id_dist", estoque.Id_dist) ;
                id_prod = estoque.Id_prod;
            }
            
   
            parametros.Add("@id_prod", id_prod);
            parametros.Add("@lote", estoque.Lote);
            parametros.Add("@saldo", saldoAtual + estoque.Saldo);

            int linhasAfetadas = _bd.ExecutarNonQuery(sql, parametros);

            return linhasAfetadas > 0;
        }

        //retira do estoque do distribuidor
        public bool AtualizaEstoque(int id_prod, string lote, int qtde)
        {
            var parametros = _bd.GerarParametros();
            string sql, sql2;
            sql = "";
            int saldoAtual = 0;
            sql2 = @"select * from estoque where id_prod = " + id_prod + " and lote = '" + lote + "'";

            DataTable dt = _bd.ExecutarSelect(sql2);

            if (dt.Rows.Count > 0)
            {
                sql = @"update estoque set saldo = @saldo where id_prod = @id_prod and lote = @lote";
                DataRow dr = dt.Rows[0];
                saldoAtual = Convert.ToInt32(dr["saldo"]);
            }

            parametros.Add("@id_prod", id_prod);
            parametros.Add("@lote", lote);
            parametros.Add("@saldo", saldoAtual - qtde);

            int linhasAfetadas = _bd.ExecutarNonQuery(sql, parametros);

            return linhasAfetadas > 0;
        }

        public bool UpdateEstoque(int id, string lote)
        {
            string sql = @"update estoque set id_prod = @id_prod, lote = @lote where id_prod = @id_prod";
            var parametros = _bd.GerarParametros();


            parametros.Add("@id_prod", id);
            parametros.Add("@lote", lote);

            int linhasAfetadas = _bd.ExecutarNonQuery(sql, parametros);

            return linhasAfetadas > 0;
        }

        public Models.Estoque Obter(int id)
        {
            Models.Estoque estoque = null;

            string select = @"select * 
                              from estoque 
                              where id = " + id;

            DataTable dt = _bd.ExecutarSelect(select);

            if (dt.Rows.Count == 1)
            {
                //ORM - Relacional -> Objeto
                estoque = Map(dt.Rows[0]);
            }

            return estoque;

        }

        public int ObterTodosEstoque(int id_prod)
        {
            string select = @"select * 
                              from estoque 
                              where id_prod = " + id_prod;

            DataTable dt = _bd.ExecutarSelect(select);

            return dt.Rows.Count;
        }

        public bool VerificaEstoque(string id_prod, int qtde)
        {
            string select = @"SELECT saldo FROM produto_distribuidor as pd inner join estoque as e on pd.cod_prod_dist = '" + id_prod + "' and pd.id = e.id_prod";

            DataTable dt = _bd.ExecutarSelect(select);
            int saldo = 0;

            if (dt.Rows.Count > 0)
            {
                DataRow row = dt.Rows[0];
                saldo = Convert.ToInt32(row["saldo"]);
            }   

            return saldo >= qtde;
        }

        public Models.Estoque ObterEstoque(int id_estoque)
        {
            string select = @"SELECT * FROM estoque where id = " + id_estoque;

            DataTable dt = _bd.ExecutarSelect(select);
            Models.Estoque estoque = new Models.Estoque();
            if (dt.Rows.Count > 0)
            {
                DataRow row = dt.Rows[0];
                estoque = Map(row);
            }

            return estoque;
        }

        public bool Excluir(int id)
        {
            string select = @"delete  
                              from estoque 
                              where id = " + id;

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
