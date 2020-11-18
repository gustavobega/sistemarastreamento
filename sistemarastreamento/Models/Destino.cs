using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sistemarastreamento.Models
{
    public class Destino
    {
        private string _nome;
        private string _rua;
        private int _numero;
        private string _bairro;
        private string _cidade;
        private string _estado;
        private string _cep;

        public Destino(string nome, string rua, int numero, string bairro, string cidade, string estado, string cep)
        {
            _nome = nome;
            _rua = rua;
            _numero = numero;
            _bairro = bairro;
            _cidade = cidade;
            _estado = estado;
            _cep = cep;
        }

        public Destino() { }

        public string Nome { get => _nome; set => _nome = value; }
        public string Rua { get => _rua; set => _rua = value; }
        public int Numero { get => _numero; set => _numero = value; }
        public string Bairro { get => _bairro; set => _bairro = value; }
        public string Cidade { get => _cidade; set => _cidade = value; }
        public string Estado { get => _estado; set => _estado = value; }
        public string Cep { get => _cep; set => _cep = value; }

        public bool salvar(Dictionary<string,string> rastro)
        {
            DAO.RastroDAO rbd = new DAO.RastroDAO();
            return rbd.Criar_Destino(rastro,this);
        }
    }
}
