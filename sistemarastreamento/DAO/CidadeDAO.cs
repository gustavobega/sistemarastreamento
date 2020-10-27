using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace sistemarastreamento.DAO
{
    public class CidadeDAO
    {
        MySqlPersistencia _bd = new MySqlPersistencia();

        public List<Models.Cidade> ObterCidades(int uf)
        {
            List<Models.Cidade> cidades = new List<Models.Cidade>();

            string select = "select * from cidade where uf = " + uf ;

            DataTable dt = _bd.ExecutarSelect(select);

            foreach (DataRow row in dt.Rows)
            {
                cidades.Add(Map(row));
            }

            return cidades;
        }

        public List<Models.Estado> ObterEstados()
        {
            List<Models.Estado> estados = new List<Models.Estado>();
            string select = "select * from estado";

            DataTable dt = _bd.ExecutarSelect(select);

            foreach (DataRow row in dt.Rows)
            {
                estados.Add(MapEstado(row));
            }

            return estados;
        }

        public string ObterEstado(int id)
        {
            string estado = "";
            string select = "select * from estado where id = "+ id;

            DataTable dt = _bd.ExecutarSelect(select);

            if (dt.Rows.Count == 1)
            {
                //ORM - Relacional -> Objeto
                estado = dt.Rows[0]["uf"].ToString();
            }

            return estado;
        }

        public string ObterCidade(int id)
        {
            string cidade = "";
            string select = "select * from cidade where id = " + id;

            DataTable dt = _bd.ExecutarSelect(select);

            if (dt.Rows.Count == 1)
            {
                //ORM - Relacional -> Objeto
                cidade = dt.Rows[0]["nome"].ToString();
            }

            return cidade;
        }

        internal Models.Cidade Map(DataRow row)
        {
            Models.Cidade cidade = new Models.Cidade();
            cidade.Id = Convert.ToInt32(row["Id"]);
            cidade.Nome = row["Nome"].ToString();
            cidade.Uf = Convert.ToInt32(row["Uf"]);

            return cidade;
        }

        internal Models.Estado MapEstado(DataRow row)
        { 
            Models.Estado estado = new Models.Estado();
            estado.Id = Convert.ToInt32(row["Id"]);
            estado.Nome = row["Nome"].ToString();
            estado.Uf = row["Uf"].ToString();

            return estado;
        }
    }
}
