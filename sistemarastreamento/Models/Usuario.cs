using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sistemarastreamento.Models
{
    public class Usuario
    {
        int _id;
        string _email;
        string _senha;
        string _tipo;
        int _perfil;

        public int Id { get => _id; set => _id = value; }
        public string Email { get => _email; set => _email = value; }
        public string Senha { get => _senha; set => _senha = value; }
        public string Tipo { get => _tipo; set => _tipo = value; }
        public int Perfil { get => _perfil; set => _perfil = value; }

        public Usuario(string email, string senha, string tipo, int perfil)
        {
            _email = email;
            _senha = senha;
            _tipo = tipo;
            _perfil = perfil;
        }

        public Usuario() { }
    }
}
