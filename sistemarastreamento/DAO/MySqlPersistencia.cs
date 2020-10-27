using System;
using System.Collections.Generic;
using System.Data; //importar o DataTable (tabela em memória)
using System.Linq;
using System.Threading.Tasks;

//Provider para o MYSQL
using MySql.Data.MySqlClient;

namespace sistemarastreamento.DAO
{
    public class MySqlPersistencia
    {
        MySqlConnection _con;
        MySqlCommand _cmd; //executa as instruçoes SQL

        int _ultimoId = 0;

        public int UltimoId { get => _ultimoId; set => _ultimoId = value; }

        public MySqlPersistencia()
        {
            string strCon = System.Environment.GetEnvironmentVariable("MYSQLSTRCOM");
            _con = new MySqlConnection(strCon);
            _cmd = _con.CreateCommand();
        }

        public void Abrir()
        {
            if (_con.State != System.Data.ConnectionState.Open)
                _con.Open();
        }

        public void Fechar()
        {
            _con.Close();
        }

        public Dictionary<string, object> GerarParametros()
        {
            return new Dictionary<string, object>();
        }
        /// <summary>
        /// Executa um comando SELECT
        /// </summary>
        /// <param name="select"></param>
        /// <returns></returns>
        public DataTable ExecutarSelect(string select, Dictionary<string, object> parametros = null)
        {
            Abrir();
            _cmd.CommandText = select;
            DataTable dt = new DataTable();

            if (parametros != null)
            {
                foreach (var p in parametros)
                {
                    _cmd.Parameters.AddWithValue(p.Key, p.Value);
                }
            }
            dt.Load(_cmd.ExecuteReader());

            Fechar();

            return dt;
        }

        /// <summary>
        /// Executa INSERT, DELETE, UPDATE E STORED PROCEDURE
        /// </summary>
        /// <param name="sql"></param>
        public int ExecutarNonQuery(string sql, Dictionary<string, object> parametros = null)
        {
            Abrir();
            _cmd.CommandText = sql;

            if (parametros != null)
            {
                foreach (var p in parametros)
                {
                    _cmd.Parameters.AddWithValue(p.Key, p.Value);
                }
            }

            int linhasAfetadas = _cmd.ExecuteNonQuery();
            _ultimoId = (int)_cmd.LastInsertedId;

            Fechar();

            return linhasAfetadas;
        }

    }
}
