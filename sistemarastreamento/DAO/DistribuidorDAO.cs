using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace sistemarastreamento.DAO
{
    public class DistribuidorDAO
    {
        MySqlPersistencia _bd = new MySqlPersistencia();

        public bool Criar(Models.Distribuidor distribuidor)
        {
            var parametros = _bd.GerarParametros();
            string sql;
            if (distribuidor.Id > 0)
            {
                sql = @"update distribuidor set cnpj=@cnpj,nome=@nome,ie=@ie,representante=@representante,rua=@rua,numero=@numero," +
                        "bairro=@bairro,telefone=@telefone,email=@email,senha=@senha,estado=@estado, cidade=@cidade where id=@id";

                parametros.Add("@id", distribuidor.Id);

            }
            else
            {
                sql = @"insert into distribuidor(cnpj,nome,ie,representante,rua,numero,bairro,telefone,email,senha,estado, cidade)" +
                        "values(@cnpj,@nome,@ie,@representante,@rua,@numero,@bairro,@telefone,@email,@senha,@estado,@cidade)";

            }

            parametros.Add("@cnpj", distribuidor.CNPJ);
            parametros.Add("@nome", distribuidor.Nome);
            parametros.Add("@ie", distribuidor.Ie);
            parametros.Add("@representante", distribuidor.Representante);
            parametros.Add("@rua", distribuidor.Rua);
            parametros.Add("@numero", distribuidor.Numero);
            parametros.Add("@bairro", distribuidor.Bairro);
            parametros.Add("@telefone", distribuidor.Telefone);
            parametros.Add("@email", distribuidor.Email);
            parametros.Add("@senha", distribuidor.Senha);
            parametros.Add("@estado", distribuidor.Estado);
            parametros.Add("@cidade", distribuidor.Cidade);

            int linhasAfetadas = _bd.ExecutarNonQuery(sql, parametros);
            if (distribuidor.Id == 0)
            {
                distribuidor.Id = _bd.UltimoId;
            }

            return linhasAfetadas > 0;
        }
        public bool validar(string cnpj, string nome, string ie, string representante, string rua, int numero, string bairro, string telefone, string email, string senha)
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

        public Models.Distribuidor Obter(int id)
        {
            Models.Distribuidor distribuidor = null;

            string select = @"select * 
                              from distribuidor 
                              where id = " + id;

            DataTable dt = _bd.ExecutarSelect(select);

            if (dt.Rows.Count == 1)
            {
                //ORM - Relacional -> Objeto
                distribuidor = Map(dt.Rows[0]);
            }

            return distribuidor;

        }

        public Models.Distribuidor ObterCnpj(string cnpj)
        {
            Models.Distribuidor distribuidor = null;

            string select = @"select * 
                              from distribuidor 
                              where cnpj = '" + cnpj + "'";

            DataTable dt = _bd.ExecutarSelect(select);

            if (dt.Rows.Count == 1)
            {
                //ORM - Relacional -> Objeto
                distribuidor = Map(dt.Rows[0]);
            }

            return distribuidor;

        }

        public Models.Distribuidor Obter(string email)
        {
            Models.Distribuidor distribuidor = null;

            string select = @"select * 
                              from distribuidor 
                              where email = '" + email + "'";

            DataTable dt = _bd.ExecutarSelect(select);

            if (dt.Rows.Count == 1)
            {
                //ORM - Relacional -> Objeto
                distribuidor = Map(dt.Rows[0]);
            }

            return distribuidor;

        }

        public List<Models.Distribuidor> Pesquisar(string nome, string tipo)
        {

            List<Models.Distribuidor> distribuidores = new List<Models.Distribuidor>();

            string select = "";

            if (tipo.ToUpper() == "NOME")
            {
                select = @"select * 
                              from distribuidor 
                              where nome like @nome";
            }
            else
            {
                select = @"select * 
                              from distribuidor 
                              where cnpj like @nome";
            }
            
            var parametros = _bd.GerarParametros();
            parametros.Add("@nome", "%" + nome + "%");

            DataTable dt = _bd.ExecutarSelect(select, parametros);

            foreach (DataRow row in dt.Rows)
            {
                distribuidores.Add(Map(row));
            }

            return distribuidores;

        }

        //para o relatório filtro por cidade
        public List<Models.Distribuidor> getDistribuidores(string cidade)
        {

            List<Models.Distribuidor> distribuidores = new List<Models.Distribuidor>();

            string select = @"select * from distribuidor inner join cidade as c on cidade = c.id and c.nome like @cidade";

            var parametros = _bd.GerarParametros();
            parametros.Add("@cidade", "%" + cidade + "%");

            DataTable dt = _bd.ExecutarSelect(select, parametros);

            foreach (DataRow row in dt.Rows)
            {
                distribuidores.Add(Map(row));
            }

            return distribuidores;

        }

        public bool Excluir(int id)
        {
            string select = @"delete  
                              from distribuidor 
                              where id = " + id;

            return _bd.ExecutarNonQuery(select) > 0;
        }

        internal Models.Distribuidor Map(DataRow row)
        {
            Models.Distribuidor distribuidor = new Models.Distribuidor();
            distribuidor.Id = Convert.ToInt32(row["id"]);
            distribuidor.CNPJ = row["cnpj"].ToString();
            distribuidor.Nome = row["nome"].ToString();
            distribuidor.Ie = row["ie"].ToString();
            distribuidor.Representante = row["representante"].ToString();
            distribuidor.Rua = row["rua"].ToString();
            distribuidor.Numero = Convert.ToInt32(row["numero"]);
            distribuidor.Bairro = row["bairro"].ToString();
            distribuidor.Telefone = row["telefone"].ToString();
            distribuidor.Email = row["email"].ToString();
            distribuidor.Senha = row["senha"].ToString();
            distribuidor.Estado = Convert.ToInt32(row["estado"]);
            distribuidor.Cidade = Convert.ToInt32(row["cidade"]);

            return distribuidor;
        }
    }
}
