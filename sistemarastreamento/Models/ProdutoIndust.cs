using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sistemarastreamento.Models
{
    public class ProdutoIndust
    {
        int _id;
        string _cod_ref;
        string _descricao;
        int _id_indust;

        public ProdutoIndust(int id, string cod_ref, string descricao, int id_indust)
        {
            _id = id;
            _cod_ref = cod_ref;
            _descricao = descricao;
            _id_indust = id_indust;
        }

        public ProdutoIndust() { }

        public int Id { get => _id; set => _id = value; }
        public string Cod_ref { get => _cod_ref; set => _cod_ref = value; }
        public string Descricao { get => _descricao; set => _descricao = value; }
        public int Id_indust { get => _id_indust; set => _id_indust = value; }
    }
}
