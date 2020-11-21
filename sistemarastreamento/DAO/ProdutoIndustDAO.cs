using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace sistemarastreamento.DAO
{
    public class ProdutoIndustDAO
    {
        MySqlPersistencia _bd = new MySqlPersistencia();

        public bool verificaCodigo(Models.ProdutoIndust prodindust)
        {
            string sql = @"select * from produto_industria where cod_ref = " + prodindust.Cod_ref;

            //é uma inserção
            if (prodindust.Id == 0)
            {
                DataTable dt = _bd.ExecutarSelect(sql);
                return dt.Rows.Count > 0;
            }

            return false;
        }
        public bool Criar(Models.ProdutoIndust prodindust)
        {
            //verifica se o código ja foi inserido
            if (!verificaCodigo(prodindust))
            {
                var parametros = _bd.GerarParametros();
                string sql;

                if (prodindust.Id > 0)
                {
                    sql = @"update produto_industria set cod_ref=@cod_ref, descricao=@descricao, id_indust=@id_indust where id=@id";

                    parametros.Add("@id", prodindust.Id);
                }
                else
                {
                    sql = @"insert into produto_industria(cod_ref,descricao,id_indust) values(@cod_ref,@descricao,@id_indust)";
                }

                parametros.Add("@cod_ref", prodindust.Cod_ref);
                parametros.Add("@descricao", prodindust.Descricao);
                parametros.Add("@id_indust", prodindust.Id_indust);

                int linhasAfetadas = _bd.ExecutarNonQuery(sql, parametros);
                if (prodindust.Id == 0)
                {
                    prodindust.Id = _bd.UltimoId;
                }

                return linhasAfetadas > 0;
            }
            return false;
        }

        public List<Models.ProdutoIndust> Pesquisar(string produto, int id_indust, string tipo)
        {

            List<Models.ProdutoIndust> produtos = new List<Models.ProdutoIndust>();
            string select = "";
            if (tipo.ToUpper() == "CÓDIGO")
            {
                select = @"select * 
                              from produto_industria 
                              where id_indust=@id_indust and cod_ref like @produto";
            }
            else
            {
                select = @"select * 
                              from produto_industria 
                              where id_indust=@id_indust and descricao like @produto";
            }
           

            var parametros = _bd.GerarParametros();
            parametros.Add("@produto", "%" + produto + "%");
            parametros.Add("@id_indust", id_indust);

            DataTable dt = _bd.ExecutarSelect(select, parametros);

            foreach (DataRow row in dt.Rows)
            {
                produtos.Add(Map(row));
            }

            return produtos;

        }

        public List<Models.ProdutoIndust> getProdutos(int id_indust)
        {

            List<Models.ProdutoIndust> produtos = new List<Models.ProdutoIndust>();
            string select =  @"select * from produto_industria where id_indust = " + id_indust;

            var parametros = _bd.GerarParametros();
            DataTable dt = _bd.ExecutarSelect(select);

            foreach (DataRow row in dt.Rows)
            {
                produtos.Add(Map(row));
            }

            return produtos;

        }

        public Models.ProdutoIndust Obter(int id)
        {
            Models.ProdutoIndust prodindust = null;

            string select = @"select * 
                              from produto_industria 
                              where id = " + id;

            DataTable dt = _bd.ExecutarSelect(select);

            if (dt.Rows.Count == 1)
            {
                //ORM - Relacional -> Objeto
                prodindust = Map(dt.Rows[0]);
            }

            return prodindust;

        }

        public Models.ProdutoIndust ObterProd(string cod_ref)
        {
            Models.ProdutoIndust prodindust = null;

            string select = @"select * 
                              from produto_industria 
                              where cod_ref = '" + cod_ref + "'";

            DataTable dt = _bd.ExecutarSelect(select);

            if (dt.Rows.Count == 1)
            {
                //ORM - Relacional -> Objeto
                prodindust = Map(dt.Rows[0]);
            }

            return prodindust;

        }

        public bool Excluir(int id)
        {
            string select = @"delete  
                              from produto_industria 
                              where id = " + id;

            return _bd.ExecutarNonQuery(select) > 0;
        }

        internal Models.ProdutoIndust Map(DataRow row)
        {
            Models.ProdutoIndust prodindust = new Models.ProdutoIndust();
            prodindust.Id = Convert.ToInt32(row["id"]);
            prodindust.Cod_ref = row["cod_ref"].ToString();
            prodindust.Descricao = row["descricao"].ToString();

            return prodindust;
        }


    }
}
