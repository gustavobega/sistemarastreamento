using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sistemarastreamento.Models
{
    public class Estoque
    {
        int _id;
        int _id_dist;
        int _id_prod;
        string _lote;
        int _saldo;

        public Estoque(int id, int id_dist, int id_prod, string lote, int saldo)
        {
            Id = id;
            Id_dist = id_dist;
            Id_prod = id_prod;
            Lote = lote;
            Saldo = saldo;
        }

        public Estoque() { }

        public int Id { get => _id; set => _id = value; }
        public int Id_dist { get => _id_dist; set => _id_dist = value; }
        public int Id_prod { get => _id_prod; set => _id_prod = value; }
        public string Lote { get => _lote; set => _lote = value; }
        public int Saldo { get => _saldo; set => _saldo = value; }

    }
}
