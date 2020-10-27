using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sistemarastreamento.Models
{
    public class Industria
    {
        int _id;
        string _cnpj;
        string _nome;
        string _ie;
        string _representante;
        string _rua;
        int _numero;
        string _bairro;
        string _telefone;
        string _email;
        string _senha;
        int _estado;
        int _cidade;

        public Industria(string cnpj, string nome, string ie, string representante, string rua, int numero, string bairro, string telefone, string email, string senha, int estado, int cidade)
        {
            _cnpj = cnpj;
            _nome = nome;
            _ie = ie;
            _representante = representante;
            _rua = rua;
            _numero = numero;
            _bairro = bairro;
            _telefone = telefone;
            _email = email;
            _senha = senha;
            _estado = estado;
            _cidade = estado;
        }

        public Industria() { }

        public string CNPJ { get => _cnpj; set => _cnpj = value; }
        public string Nome { get => _nome; set => _nome = value; }
        public string Ie { get => _ie; set => _ie = value; }
        public string Representante { get => _representante; set => _representante = value; }
        public string Rua { get => _rua; set => _rua = value; }
        public int Numero { get => _numero; set => _numero = value; }
        public string Bairro { get => _bairro; set => _bairro = value; }
        public string Telefone { get => _telefone; set => _telefone = value; }
        public string Email { get => _email; set => _email = value; }
        public string Senha { get => _senha; set => _senha = value; }
        public int Id { get => _id; set => _id = value; }
        public int Estado { get => _estado; set => _estado = value; }
        public int Cidade { get => _cidade; set => _cidade = value; }
    }
}
