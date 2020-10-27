using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sistemarastreamento
{
    public class AppSettings
    {
        public string StringConexaoMySQL { get; set; }
        public string EmailPadrao { get; set; }
        public string DirTemp { get; set; }
        public int CookieTempoVida { get; set; }
        public AppSettings()
        {

        }
    }
}
