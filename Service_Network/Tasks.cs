using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service_Network
{
    public class Tasks
    {
        public Tasks(string nomtache, string nomservice, int frequence,int idt,string host,string dns)
        {
            Nomtache = nomtache;
            Nomservice = nomservice;
            Frequence = frequence;
            idT = idt;
            Host = host;
            Dns = dns;

        }

        public string Nomtache { get; set; }
        public string Nomservice { get; set; }

        public string Host { get; set; }

        public string Dns { get; set; }
        public int Frequence { get; set; }

        public int idT { get; set; }


    }
}
