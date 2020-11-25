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
            string sql;

            sql = @"select * from produto_distribuidor where cod_prod_dist = '" + proddist.Cod_prod_dist + "'";
            DataTable dt = _bd.ExecutarSelect(sql);
            
            if (dt.Rows.Count > 0)
            {
                proddist.Id = Convert.ToInt32(dt.Rows[0]["Id"]);
            }
            return (dt.Rows.Count > 0);  
        }
        public (bool,string) verificaCodigoLote(Models.ProdutoDist proddist, string lote)
        {
            //é uma inserção
            if (proddist.Id == 0)
            {
                string sql = @"select * from produto_industria as pi inner join produto_distribuidor as pd on pi.cod_ref = pd.cod_ref and pi.cod_ref = '" + proddist.Cod_ref + "' inner join estoque as e on pi.id = e.id_prod and e.lote = '" + lote + "'";

                DataTable dt = _bd.ExecutarSelect(sql);

                return (dt.Rows.Count > 0, "Relação (Produto x Lote) já Possui Cadastro!");      
            }

            return (false, "");
        }
        public (bool,string) Criar(Models.ProdutoDist proddist, string lote)
        {
            bool operacao;
            string msg;

            (operacao, msg) = verificaCodigoLote(proddist, lote);
            if (!operacao)
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

                    return (linhasAfetadas > 0, msg);
                }
                else
                    return (true, msg);
                
            }

            return (false, msg);   
        }

        public DataTable Pesquisar(string descricao, int id_dist, string tipo)
        {
            string select;
            if (tipo.ToUpper() == "LOTE")
            {
                select = @"SELECT d.id,d.cod_ref,cod_prod_dist,descricao,lote,saldo 
                                FROM produto_distribuidor as d INNER JOIN produto_industria as i 
                                ON d.cod_ref = i.cod_ref and d.id_dist = @id_dist
                                INNER JOIN estoque as e on e.id_prod = i.id
                                AND e.lote like @descricao AND e.id_dist = @id_dist";
            }
            else
            {
                select = @"SELECT d.id,d.cod_ref,cod_prod_dist,descricao,lote,saldo 
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

        public DataTable getProdutos(int id_dist, string lote)
        {

            string select = @"SELECT d.id,d.cod_ref,cod_prod_dist,descricao,lote,saldo 
                                FROM produto_distribuidor as d INNER JOIN produto_industria as i 
                                ON d.cod_ref = i.cod_ref and d.id_dist = @id_dist
                                INNER JOIN estoque as e on e.id_prod = i.id
                                AND e.lote like @lote AND e.id_dist = @id_dist";


            var parametros = _bd.GerarParametros();
            parametros.Add("@id_dist", id_dist);
            parametros.Add("@lote", "%" + lote + "%");

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
                              where cod_ref = '" + cod_ref + "'";

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
                                "as e ON pi.id = e.id_prod ORDER BY saldo LIMIT 5";

            DataTable dt = _bd.ExecutarSelect(select);

            return dt;
        }
        
    }
}
