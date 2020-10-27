using System;
using System.Collections.Generic;
using System.Data;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using sistemarastreamento.Models;

namespace sistemarastreamento.DAO
{
    public class UsuarioDAO
    {
        MySqlPersistencia _bd = new MySqlPersistencia();

        public bool Criar(Models.Usuario usuario)
        {
            string sql = "insert into usuario(email,senha,tipo,perfil) values(@email,@senha,@tipo,@perfil)";

            var parametros = _bd.GerarParametros();
            parametros.Add("@email", usuario.Email);
            parametros.Add("@senha", usuario.Senha);
            parametros.Add("@tipo", usuario.Tipo);
            parametros.Add("@perfil", usuario.Perfil);

            int linhasAfetadas = _bd.ExecutarNonQuery(sql, parametros);

            if (usuario.Id == 0)
            {
                usuario.Id = _bd.UltimoId;
            }

            return linhasAfetadas > 0;
        }

        public Models.Usuario Obter(int id)
        {
            Models.Usuario usuario = null;
            string select = @"select * from usuario where id = "+id;


            DataTable dt = _bd.ExecutarSelect(select);

            if (dt.Rows.Count == 1)
            {
                //ORM - Relacional -> Objeto
                usuario = Map(dt.Rows[0]);
            }

            return usuario;
        }

        public Models.Perfil ObterPerfil(string email)
        {
            Models.Usuario usuario = null;
            string select = @"select * from usuario where email = '" + email + "'";
            Models.Perfil perfil = new Models.Perfil();

            DataTable dt = _bd.ExecutarSelect(select);

            if (dt.Rows.Count == 1)
            {
                //ORM - Relacional -> Objeto
                usuario = Map(dt.Rows[0]);
                DAO.PerfilDAO pbd = new PerfilDAO();
                perfil = pbd.Obter(usuario.Perfil);
            }

            return perfil;
        }

        internal Models.Usuario Map(DataRow row)
        {
            Models.Usuario usuario = new Models.Usuario();
            usuario.Id = Convert.ToInt32(row["id"]);
            usuario.Email = row["email"].ToString();
            usuario.Senha = row["senha"].ToString();
            usuario.Tipo = row["tipo"].ToString();
            usuario.Perfil = Convert.ToInt32(row["perfil"].ToString());

            return usuario;
        }

        public Models.Usuario Validar(Models.Usuario usuario)
        {
            Models.Usuario retorno = null;
            string select = @"select id from usuario where email = @email and senha = @senha and tipo = @tipo";

            var parametros = _bd.GerarParametros();
            parametros.Add("@email", usuario.Email);
            parametros.Add("@senha", usuario.Senha);
            parametros.Add("@tipo", usuario.Tipo);

            DataTable dt = _bd.ExecutarSelect(select, parametros);
            int conta = dt.Rows.Count;

            if (conta > 0)
            {
                int id = Convert.ToInt32(dt.Rows[0]["id"]);
                return Obter(id);
            }

            return retorno;
        }

        public bool AlterarPerfil(string email, int idPerfil)
        {
            string sql = "update usuario set perfil = " + idPerfil + " where email = '" + email + "'";

            int linhasAfetadas = _bd.ExecutarNonQuery(sql);

            return linhasAfetadas > 0;
        }
    }
}
