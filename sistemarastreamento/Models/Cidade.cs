using System;

namespace sistemarastreamento.Models
{
    public class Cidade
    {
        int _id;
        string _nome;
        int _uf;

        public int Id { get => _id; set => _id = value; }
        public string Nome { get => _nome; set => _nome = value; }
        public int Uf { get => _uf; set => _uf = value; }

        public Cidade()
        {
            _id = 0;
            _nome = "";
            _uf = 0;
        }
    }
}
