using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sistemarastreamento.DAO
{
    public class NotaDAO
    {
        MySqlPersistencia _bd = new MySqlPersistencia();
        public bool Criar(Models.NotaFiscal nota)
        {
            var parametros = _bd.GerarParametros();
            string sql;

            sql = @"insert into nota_fiscal(tipo,id_indust,id_dist,data,numero,serie,chave,valor_nf,paciente,data_cirurgia,medico,convenio,hospital)" +
                    "values(@tipo,@id_indust,@id_dist,@data,@numero,@serie,@chave,@valor_nf,@paciente,@data_cirurgia,@medico,@convenio,@hospital)";

            parametros.Add("@tipo", nota.Tipo);
            parametros.Add("@id_indust", nota.Cod_indust);
            parametros.Add("@id_dist", nota.Cod_dist);
            parametros.Add("@data", nota.Data);
            parametros.Add("@numero", nota.Numero);
            parametros.Add("@serie", nota.Serie);
            parametros.Add("@chave", nota.Chave);
            parametros.Add("@valor_nf", nota.Valor_nf);
            parametros.Add("@paciente", nota.Paciente);
            parametros.Add("@data_cirurgia", nota.Data_cirurgia);
            parametros.Add("@medico", nota.Medico);
            parametros.Add("@convenio", nota.Convenio);
            parametros.Add("@hospital", nota.Hospital);

            int linhasAfetadas = _bd.ExecutarNonQuery(sql, parametros);
            if (nota.Id == 0)
            {
                nota.Id = _bd.UltimoId;
            }

            return linhasAfetadas > 0;
        }
        

        public bool Alterar(Models.NotaFiscal nota)
        {
            var parametros = _bd.GerarParametros();
            string sql;

            sql = @"update nota_fiscal set paciente=@paciente, data_cirurgia=@data_cirurgia, medico=@medico, convenio=@convenio,hospital=@hospital  where id=@id";

            parametros.Add("@id", nota.Id);
            parametros.Add("@paciente", nota.Paciente);
            parametros.Add("@data_cirurgia", nota.Data_cirurgia);
            parametros.Add("@medico", nota.Medico);
            parametros.Add("@convenio", nota.Convenio);
            parametros.Add("@hospital", nota.Hospital);

            int linhasAfetadas = _bd.ExecutarNonQuery(sql, parametros);

            return linhasAfetadas > 0;
        }

        public Models.NotaFiscal Obter(int id)
        {
            Models.NotaFiscal nota = null;

            string select = @"select * 
                              from nota_fiscal 
                              where id = " + id;

            DataTable dt = _bd.ExecutarSelect(select);

            if (dt.Rows.Count == 1)
            {
                //ORM - Relacional -> Objeto
                nota = Map(dt.Rows[0]);
            }

            return nota;

        }

        public bool ObterNotaExistente(int numero)
        {
            string select = @"select * 
                              from nota_fiscal 
                              where numero = " + numero;

            DataTable dt = _bd.ExecutarSelect(select);

            return dt.Rows.Count > 0;
        }

        public List<Models.NotaFiscal> PesquisarIndust(string numero, string id_indust, string tipo)
        {

            List<Models.NotaFiscal> notas = new List<Models.NotaFiscal>();

            string select = "";
            numero = numero.Trim();
            var parametros = _bd.GerarParametros();

            if (tipo.ToUpper() == "NÚMERO")
            {
                select = @"select * from nota_fiscal where id_indust = @id_indust and numero like @numero";
                parametros.Add("@numero", "%" + numero + "%");
            }
            else
            {
                select = @"select * 
                              from nota_fiscal 
                              where id_indust = @id_indust and data like @data";

                var data = numero.Split("/");
                var dataFormat = "";
                if (data.Length == 3)
                {
                    dataFormat += data[2] + "-" + data[1] + "-" + data[0];
                }
                parametros.Add("@data", "%" + dataFormat + "%");
            }

            parametros.Add("@id_indust", id_indust);

            DataTable dt = _bd.ExecutarSelect(select, parametros);

            foreach (DataRow row in dt.Rows)
            {
                notas.Add(Map(row));
            }

            return notas;
        }

        public List<Models.NotaFiscal> PesquisarDist(string numero, string id_dist, string tipo)
        {

            List<Models.NotaFiscal> notas = new List<Models.NotaFiscal>();
            string select = "";
            numero = numero.Trim();
            var parametros = _bd.GerarParametros();
            if (tipo.ToUpper() == "NÚMERO")
            {
                select = @"select * 
                              from nota_fiscal 
                              where id_dist = @id_dist and numero like @numero";

                parametros.Add("@numero", "%" + numero + "%");
            }
            else
            {

                select = @"select * 
                              from nota_fiscal 
                              where id_dist = @id_dist and data like @data";

                var data = numero.Split("/");
                var dataFormat = "";
                if (data.Length == 3)
                {
                    dataFormat += data[2] + "-" + data[1] + "-" + data[0];
                }
                parametros.Add("@data", "%" + dataFormat + "%");
            }

           
            parametros.Add("@id_dist", id_dist);

            DataTable dt = _bd.ExecutarSelect(select, parametros);

            foreach (DataRow row in dt.Rows)
            {
                notas.Add(Map(row));
            }

            return notas;
        }

        public bool Excluir(int id)
        {
            string select = @"delete  
                              from nota_fiscal 
                              where id = " + id;

            return _bd.ExecutarNonQuery(select) > 0;
        }

        internal Models.NotaFiscal Map(DataRow row)
        {
            Models.NotaFiscal nota = new Models.NotaFiscal();
            nota.Id = Convert.ToInt32(row["id"]);
            nota.Data = Convert.ToDateTime(row["Data"]);
            nota.Numero = Convert.ToInt32(row["Numero"]);
            nota.Serie = Convert.ToInt32(row["Serie"]);
            nota.Chave = row["Chave"].ToString();
            nota.Valor_nf = Convert.ToDouble(row["Valor_nf"]);

            nota.Data_cirurgia = Convert.ToDateTime(row["Data_cirurgia"]);
            nota.Paciente = row["Paciente"].ToString();
            nota.Medico = row["Medico"].ToString();
            nota.Convenio = row["Convenio"].ToString();
            nota.Hospital = row["Hospital"].ToString();

            return nota;
        }
    }
}
