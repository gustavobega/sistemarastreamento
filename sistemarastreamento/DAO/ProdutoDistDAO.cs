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

        public bool verificaCodigo(Models.ProdutoDist proddist, int id_estoque)
        {
            if (id_estoque == 0)
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

            return false;
             
        }
        public (bool,string) verificaCodigoLote(Models.ProdutoDist proddist, string lote, int id_estoque)
        {
            //é uma inserção
            if (id_estoque == 0)
            {
                string sql = @"select * from produto_distribuidor as pd inner join estoque as e on pd.id = e.id_prod and e.lote = '" + lote + "' and pd.cod_ref = '" + proddist.Cod_ref + "'";

                DataTable dt = _bd.ExecutarSelect(sql);

                return (dt.Rows.Count > 0, "Relação (Produto x Lote) já Possui Cadastro!");      
            }

            return (false, "");
        }
        public (bool,string) Criar(Models.ProdutoDist proddist, string lote, int id_estoque)
        {
            bool operacao;
            string msg;

            (operacao, msg) = verificaCodigoLote(proddist, lote, id_estoque);
            if (!operacao)
            {
                if (!verificaCodigo(proddist, id_estoque))
                {
                    var parametros = _bd.GerarParametros();
                    string sql;

                    if (id_estoque > 0)
                    {
                        EstoqueDAO ebd = new EstoqueDAO();
                        Models.Estoque estoque = ebd.ObterEstoque(id_estoque); 

                        ebd.UpdateEstoque(estoque.Id_prod, lote);

                        sql = @"update produto_distribuidor set cod_ref=@cod_ref, id_dist=@id_dist, cod_prod_dist=@cod_prod_dist where id=@id";

                        parametros.Add("@id", estoque.Id_prod);
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
                    if (id_estoque == 0)
                        return (linhasAfetadas > 0, msg);
                    else
                        return (true, "");
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
                select = @"SELECT * 
                                FROM estoque as e INNER JOIN produto_distribuidor as pd 
                                ON e.id_prod = pd.id INNER JOIN produto_industria as pi
                                on pi.cod_ref = pd.cod_ref and pd.id_dist = @id_dist
                                AND e.lote like @descricao AND e.id_dist = @id_dist";
            }
            else
            {
                select = @"SELECT * 
                                FROM estoque as e INNER JOIN produto_distribuidor as pd 
                                ON e.id_prod = pd.id INNER JOIN produto_industria as pi
                                on pi.cod_ref = pd.cod_ref and pd.id_dist = @id_dist
                                AND pd.cod_prod_dist like @descricao AND e.id_dist = @id_dist";
            }
            

            var parametros = _bd.GerarParametros();
            parametros.Add("@id_dist", id_dist);
            parametros.Add("@descricao", "%" +  descricao + "%");

            DataTable dt = _bd.ExecutarSelect(select, parametros);

            return dt;
        }

        public DataTable getProdutos(int id_dist, string lote)
        {

            string select = @"SELECT pd.id,pd.cod_ref,cod_prod_dist,descricao,lote,saldo 
                                FROM produto_distribuidor as pd INNER JOIN produto_industria as pi 
                                ON pd.cod_ref = pi.cod_ref and pd.id_dist = @id_dist
                                INNER JOIN estoque as e on e.id_prod = pd.id
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

        public DataTable ObterDadosProdDist(int id_estoque)
        {
            string select = @"SELECT * 
                                FROM estoque as e inner join produto_distribuidor as pd on e.id_prod = pd.id
                                and e.id = " + id_estoque + " inner join produto_industria as pi on pd.cod_ref = pi.cod_ref";

            DataTable dt = _bd.ExecutarSelect(select);

            return dt;
        }

        public bool Excluir(int id_prod)
        {
            string select = @"delete  
                              from produto_distribuidor 
                              where id = " + id_prod;

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
                                "as e ON pd.id = e.id_prod ORDER BY saldo LIMIT 5";

            DataTable dt = _bd.ExecutarSelect(select);

            return dt;
        }
        
    }
}
