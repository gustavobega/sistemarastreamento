using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace sistemarastreamento.DAO
{
    public class ProdutoDistDAO
    {
        MySqlPersistencia _bd = new MySqlPersistencia();

        public bool verificaCodigo(Models.ProdutoDist proddist)
        {
            string sql = @"select * from produto_distribuidor where cod_ref = " + proddist.Cod_ref;

            //é uma inserção
            if (proddist.Id == 0)
            {
                DataTable dt = _bd.ExecutarSelect(sql);
                return dt.Rows.Count > 0;
            }

            return false;
        }
        public bool Criar(Models.ProdutoDist proddist)
        {
            if (!verificaCodigo(proddist))
            {
                var parametros = _bd.GerarParametros();
                string sql;

                if (proddist.Id > 0)
                {
                    sql = @"update produto_distribuidor set cod_ref=@cod_ref, id_dist=@id_dist, cod_prod_dist=@cod_prod_dist where id=@id";

                    parametros.Add("@id", proddist.Id);
                }
                else
                {
                    sql = @"insert into produto_distribuidor(cod_ref,id_dist,cod_prod_dist) values(@cod_ref,@id_dist,@cod_prod_dist)";
                }

                parametros.Add("@cod_ref", proddist.Cod_ref);
                parametros.Add("@id_dist", proddist.Id_dist);
                parametros.Add("@cod_prod_dist", proddist.Cod_prod_dist);

                int linhasAfetadas = _bd.ExecutarNonQuery(sql, parametros);
                if (proddist.Id == 0)
                {
                    proddist.Id = _bd.UltimoId;
                }

                return linhasAfetadas > 0;
            }

            return false;   
        }

        public DataTable Pesquisar(string descricao, int id_dist, string tipo)
        {
            string select = "";
            if (tipo.ToUpper() == "DESCRIÇÃO")
            {
                select = @"SELECT d.id,cod_prod_dist,descricao,lote,saldo 
                                FROM produto_distribuidor as d INNER JOIN produto_industria as i 
                                ON d.cod_ref = i.cod_ref and d.id_dist = @id_dist
                                AND i.descricao like @descricao INNER JOIN estoque as e on e.id_prod = i.id
                                AND e.id_dist = @id_dist";
            }
            else
            {
                select = @"SELECT d.id,cod_prod_dist,descricao,lote,saldo 
                                FROM produto_distribuidor as d INNER JOIN produto_industria as i 
                                ON d.cod_ref = i.cod_ref and d.id_dist = @id_dist
                                AND d.cod_prod_dist like @descricao INNER JOIN estoque as e on e.id_prod = i.id
                                AND e.id_dist = @id_dist";
            }
            

            var parametros = _bd.GerarParametros();
            parametros.Add("@id_dist", id_dist);
            parametros.Add("@descricao", "%" +  descricao + "%");

            DataTable dt = _bd.ExecutarSelect(select, parametros);

            return dt;
        }

        public Models.ProdutoDist Obter(int id)
        {
            Models.ProdutoDist proddist = null;

            string select = @"select * 
                              from produto_distribuidor 
                              where id = " + id;

            DataTable dt = _bd.ExecutarSelect(select);

            if (dt.Rows.Count == 1)
            {
                //ORM - Relacional -> Objeto
                proddist = Map(dt.Rows[0]);
            }

            return proddist;

        }

        public Models.ProdutoDist ObterProd(string cod_dist_prod)
        {
            Models.ProdutoDist proddist = null;

            string select = @"select * 
                              from produto_distribuidor 
                              where cod_prod_dist = '" + cod_dist_prod + "'";

            DataTable dt = _bd.ExecutarSelect(select);

            if (dt.Rows.Count == 1)
            {
                //ORM - Relacional -> Objeto
                proddist = Map(dt.Rows[0]);
            }

            return proddist;

        }

        public bool Excluir(string cod_ref)
        {
            string select = @"delete  
                              from produto_distribuidor 
                              where cod_ref = " + cod_ref;

            return _bd.ExecutarNonQuery(select) > 0;
        }

        internal Models.ProdutoDist Map(DataRow row)
        {
            Models.ProdutoDist proddist = new Models.ProdutoDist();
            proddist.Id = Convert.ToInt32(row["id"]);
            proddist.Cod_ref = row["cod_ref"].ToString();
            proddist.Id_dist = Convert.ToInt32(row["id_dist"]);
            proddist.Cod_prod_dist = row["cod_prod_dist"].ToString();

            return proddist;
        }

        public DataTable getEstoqueProd(int id_dist)
        {

            string select = @"SELECT cod_prod_dist, descricao, saldo FROM produto_distribuidor as pd INNER JOIN 
                                produto_industria as pi ON pd.cod_ref = pi.cod_ref
                                and id_dist = " + id_dist + " INNER JOIN estoque " +
                                "as e ON pi.id = e.id_prod ORDER BY saldo LIMIT 10";

            DataTable dt = _bd.ExecutarSelect(select);

            return dt;
        }
        
    }
}
