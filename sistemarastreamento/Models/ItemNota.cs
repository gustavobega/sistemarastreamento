using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sistemarastreamento.Models
{
    public class ItemNota
    {
        int _id;
        int _id_nota;
        int _id_prod;
        string _lote;
        int _qtde;
        double _valor_unit;

        public ItemNota(int id, int id_nota, int id_prod, string lote, int qtde, double valor_unit)
        {
            _id = id;
            _id_nota = id_nota;
            _id_prod = id_prod;
            _lote = lote;
            _qtde = qtde;
            _valor_unit = valor_unit;
        }

        public ItemNota()
        {
            Id = 0;
            Id_nota = 0;
            Id_prod = 0;
            Lote = "";
            Qtde = 0;
            Valor_unit = 0.0;
        }

        public int Id { get => _id; set => _id = value; }
        public int Id_nota { get => _id_nota; set => _id_nota = value; }
        public int Id_prod { get => _id_prod; set => _id_prod = value; }
        public string Lote { get => _lote; set => _lote = value; }
        public int Qtde { get => _qtde; set => _qtde = value; }
        public double Valor_unit { get => _valor_unit; set => _valor_unit = value; }
    }
}
