using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sistemarastreamento.Models
{
    public class NotaFiscal
    {
        int _id;
        string _tipo;
        int _cod_indust;
        int _cod_dist;
        DateTime _data;
        int _numero;
        int _serie;
        string _chave;
        double _valor_nf;
        string _paciente;
        DateTime _data_cirurgia;
        string _medico;
        string _convenio;
        string _hospital;

        public NotaFiscal(string tipo, int cod_indust, int cod_dist, DateTime data, int numero, int serie, string chave, double valor_nf, string paciente, DateTime data_cirurgia, string medico, string convenio, string hospital)
        {
            _tipo = tipo;
            _cod_indust = cod_indust;
            _cod_dist = cod_dist;
            _data = data;
            _numero = numero;
            _serie = serie;
            _chave = chave;
            _valor_nf = valor_nf;
            _paciente = paciente;
            _data_cirurgia = data_cirurgia;
            _medico = medico;
            _convenio = convenio;
            _hospital = hospital;
        }

        public NotaFiscal()
        {
            _tipo = "";
            _cod_indust = 0;
            _cod_dist = 0;
            _data = new DateTime();
            _numero = 0;
            _serie = 0;
            _chave = "";
            _valor_nf = 0.0;
            _paciente = "";
            _data_cirurgia = new DateTime();
            _medico = "";
            _convenio = "";
            _hospital = "";
        }

        public int Id { get => _id; set => _id = value; }
        public string Tipo { get => _tipo; set => _tipo = value; }
        public int Cod_indust { get => _cod_indust; set => _cod_indust = value; }
        public int Cod_dist { get => _cod_dist; set => _cod_dist = value; }
        public DateTime Data { get => _data; set => _data = value; }
        public int Numero { get => _numero; set => _numero = value; }
        public int Serie { get => _serie; set => _serie = value; }
        public string Chave { get => _chave; set => _chave = value; }
        public double Valor_nf { get => _valor_nf; set => _valor_nf = value; }
        public string Paciente { get => _paciente; set => _paciente = value; }
        public DateTime Data_cirurgia { get => _data_cirurgia; set => _data_cirurgia = value; }
        public string Medico { get => _medico; set => _medico = value; }
        public string Convenio { get => _convenio; set => _convenio = value; }
        public string Hospital { get => _hospital; set => _hospital = value; }
    }
}
