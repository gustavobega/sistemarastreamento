using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sistemarastreamento.Models
{
    public class ProdutoDist
    {
        int _id;
        string _cod_ref;
        int _id_dist;
        string _cod_prod_dist;

        public ProdutoDist(int id, string cod_ref, int id_dist, string cod_prod_dist)
        {
            _id = id;
            _cod_ref = cod_ref;
            _id_dist = id_dist;
            _cod_prod_dist = cod_prod_dist;
        }

        public ProdutoDist() { }

        public int Id { get => _id; set => _id = value; }
        public string Cod_ref { get => _cod_ref; set => _cod_ref = value; }
        public int Id_dist { get => _id_dist; set => _id_dist = value; }
        public string Cod_prod_dist { get => _cod_prod_dist; set => _cod_prod_dist = value; }
    }
}
