using System;

namespace sistemarastreamento.Models
{
    public class Estado
    {
        int _id;
        string _nome;
        string _uf;

        public int Id { get => _id; set => _id = value; }
        public string Nome { get => _nome; set => _nome = value; }
        public string Uf { get => _uf; set => _uf = value; }

        public Estado()
        {
            _id = 0;
            _nome = "";
            _uf = "";
        }
    }
}
