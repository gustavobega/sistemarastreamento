using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sistemarastreamento.DAO
{
    public class IndustriaDAO
    {
        MySqlPersistencia _bd = new MySqlPersistencia();

        public bool Criar(Models.Industria industria)
        {
            var parametros = _bd.GerarParametros();
            string sql;
            if (industria.Id > 0)
            {
                sql = @"update industria set cnpj=@cnpj,nome=@nome,ie=@ie,representante=@representante,rua=@rua,numero=@numero," +
                        "bairro=@bairro,telefone=@telefone,email=@email,senha=@senha,estado=@estado, cidade=@cidade where id=@id";

                parametros.Add("@id", industria.Id);

            }
            else
            {
                sql = @"insert into industria(cnpj,nome,ie,representante,rua,numero,bairro,telefone,email,senha,estado, cidade)" +
                        "values(@cnpj,@nome,@ie,@representante,@rua,@numero,@bairro,@telefone,@email,@senha,@estado,@cidade)";

            }
 
            parametros.Add("@cnpj", industria.CNPJ);
            parametros.Add("@nome", industria.Nome);
            parametros.Add("@ie", industria.Ie);
            parametros.Add("@representante", industria.Representante);
            parametros.Add("@rua", industria.Rua);
            parametros.Add("@numero", industria.Numero);
            parametros.Add("@bairro", industria.Bairro);
            parametros.Add("@telefone", industria.Telefone);
            parametros.Add("@email", industria.Email);
            parametros.Add("@senha", industria.Senha);
            parametros.Add("@estado", industria.Estado);
            parametros.Add("@cidade", industria.Cidade);
            
            int linhasAfetadas = _bd.ExecutarNonQuery(sql, parametros);
            if (industria.Id == 0)
            {
                industria.Id = _bd.UltimoId;
            }
        
            return linhasAfetadas > 0;     
 
        }
        public bool validar(string cnpj,string nome,string ie,string representante,string rua,int numero,string bairro,string telefone,string email, string senha)
        {
            Dictionary<string, object> parametros = new Dictionary<string, object>();
            parametros.Add("cnpj", cnpj);
            parametros.Add("nome", nome);
            parametros.Add("ie", ie);
            parametros.Add("representante", representante);
            parametros.Add("rua", rua);
            parametros.Add("numero", numero);
            parametros.Add("bairro", bairro);
            parametros.Add("telefone", telefone);
            parametros.Add("email", email);
            parametros.Add("senha", senha);
            string select = @"select ";

            _bd.ExecutarSelect(select, parametros);
            return true;
        }

        public Models.Industria Obter(int id)
        {
            Models.Industria industria = null;

            string select = @"select * 
                              from industria 
                              where id = " + id;

            DataTable dt = _bd.ExecutarSelect(select);

            if (dt.Rows.Count == 1)
            {
                //ORM - Relacional -> Objeto
                industria = Map(dt.Rows[0]);
            }

            return industria;

        }

        public Models.Industria Obter(string email)
        {
            Models.Industria industria = null;

            string select = @"select * 
                              from industria 
                              where email = '" + email + "'";

            DataTable dt = _bd.ExecutarSelect(select);

            if (dt.Rows.Count == 1)
            {
                //ORM - Relacional -> Objeto
                industria = Map(dt.Rows[0]);
            }

            return industria;

        }

        public List<Models.Industria> Pesquisar(string nome, string tipo)
        {

            List<Models.Industria> industrias = new List<Models.Industria>();

            string select = "";
            if (tipo.ToUpper() == "NOME")
            {
                select = @"select * 
                              from industria 
                              where nome like @nome";
            }
            else
            {
                select = @"select * 
                              from industria 
                              where cnpj like @nome";
            }
            
            var parametros = _bd.GerarParametros();
            parametros.Add("@nome", "%" + nome + "%");

            DataTable dt = _bd.ExecutarSelect(select, parametros);

            foreach (DataRow row in dt.Rows)
            {
                industrias.Add(Map(row));
            }

            return industrias;

        }


        public bool Excluir(int id)
        {
            string select = @"delete  
                              from industria 
                              where id = " + id;

            return _bd.ExecutarNonQuery(select) > 0;
        }

        internal Models.Industria Map(DataRow row)
        {
            Models.Industria industria = new Models.Industria();
            industria.Id = Convert.ToInt32(row["id"]);
            industria.CNPJ = row["cnpj"].ToString();
            industria.Nome = row["nome"].ToString();
            industria.Ie = row["ie"].ToString();
            industria.Representante = row["representante"].ToString();
            industria.Rua = row["rua"].ToString();
            industria.Numero = Convert.ToInt32(row["numero"]);
            industria.Bairro = row["bairro"].ToString();
            industria.Telefone = row["telefone"].ToString();
            industria.Email = row["email"].ToString();
            industria.Senha = row["senha"].ToString();
            industria.Estado = Convert.ToInt32(row["estado"]);
            industria.Cidade = Convert.ToInt32(row["cidade"]);

            return industria;
        }
    }
}
