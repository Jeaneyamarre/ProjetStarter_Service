using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetStarter_Service
{
    public class Tasks
    {
        public Tasks(string nomtache, string nomservice, int frequence,int idt,string host)
        {
            Nomtache = nomtache;
            Nomservice = nomservice;
            Frequence = frequence;
            idT = idt;
            Host = host;

        }

        public string Nomtache { get; set; }
        public string Nomservice { get; set; }

        public string Host { get; set; }
        public int Frequence { get; set; }

        public int idT { get; set; }


    }
}
