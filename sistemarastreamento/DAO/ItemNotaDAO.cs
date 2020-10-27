using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sistemarastreamento.DAO
{
    public class ItemNotaDAO
    {

        MySqlPersistencia _bd = new MySqlPersistencia();
        public bool Criar(Models.ItemNota itemnota)
        {
            var parametros = _bd.GerarParametros();
            string sql;
            if (itemnota.Id > 0)
            {
                sql = @"update item_nota set id_nota=@id_nota,id_prod=@id_prod,lote=@lote,qtde=@qtde,valor_unit=@valor_unit," +
                        "where id=@id";

                parametros.Add("@id", itemnota.Id);

            }
            else
            {
                sql = @"insert into item_nota(id_nota,id_prod,lote,qtde,valor_unit)" +
                        "values(@id_nota,@id_prod,@lote,@qtde,@valor_unit)";

            }

            parametros.Add("@id_nota", itemnota.Id_nota);
            parametros.Add("@id_prod", itemnota.Id_prod);
            parametros.Add("@lote", itemnota.Lote);
            parametros.Add("@qtde", itemnota.Qtde);
            parametros.Add("@valor_unit", itemnota.Valor_unit);

            int linhasAfetadas = _bd.ExecutarNonQuery(sql, parametros);
            if (itemnota.Id == 0)
            {
                itemnota.Id = _bd.UltimoId;
            }

            return linhasAfetadas > 0;
        }

        public DataTable Obter(int id)
        {
            string select = @"SELECT cod_ref,descricao,lote,qtde,valor_unit 
                              FROM produto_industria INNER JOIN item_nota 
                              ON item_nota.id_nota = " + id + " AND produto_industria.id = item_nota.id_prod";

            DataTable dt = _bd.ExecutarSelect(select);

            if (dt.Rows.Count > 0)
            {
                return dt;
            }
            else
                return null;
        }

        public DataTable ObterDist(int id)
        {
            string select = @"SELECT cod_prod_dist,descricao,lote,qtde,valor_unit FROM produto_distribuidor INNER JOIN item_nota " + 
                              "ON item_nota.id_nota = " + id + " AND produto_distribuidor.id = item_nota.id_prod " +
                              "INNER JOIN produto_industria ON produto_industria.cod_ref = produto_distribuidor.cod_ref";
                              

            DataTable dt = _bd.ExecutarSelect(select);

            if (dt.Rows.Count > 0)
            {
                return dt;
            }
            else
                return null;
        }

        public Models.ItemNota ObterItemId_Prod(int id_prod)
        {
            Models.ItemNota item = null;

            string select = @"select * 
                              from item_nota 
                              where id_prod = " + id_prod;

            DataTable dt = _bd.ExecutarSelect(select);

            if (dt.Rows.Count == 1)
            {
                //ORM - Relacional -> Objeto
                item = Map(dt.Rows[0]);
            }

            return item;
        }

        public List<Models.ItemNota> Pesquisa(int id_nota)
        {

            List<Models.ItemNota> itens = new List<Models.ItemNota>();

            string select = @"select * 
                              from item_nota 
                              where id_nota = " + id_nota;

            DataTable dt = _bd.ExecutarSelect(select);

            foreach (DataRow row in dt.Rows)
            {
                itens.Add(Map(row));
            }

            return itens;

        }

        public bool ExcluirItens(int id)
        {
            string select = @"delete  
                              from item_nota 
                              where id_nota = " + id;

            return _bd.ExecutarNonQuery(select) > 0;
        }

        internal Models.ItemNota Map(DataRow row)
        {
            Models.ItemNota itemnota = new Models.ItemNota();
            itemnota.Id = Convert.ToInt32(row["id"]);
            itemnota.Id_prod = Convert.ToInt32(row["Id_prod"]);
            itemnota.Lote = row["Lote"].ToString();
            itemnota.Qtde = Convert.ToInt32(row["Qtde"]);
            itemnota.Valor_unit = Convert.ToDouble(row["Valor_unit"]);

            return itemnota;
        }
    }   
}
