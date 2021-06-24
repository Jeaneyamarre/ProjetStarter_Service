using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projet_Starter_WinformsCSharp
{
   public class ServiceStatut
    {
        public ServiceStatut(string nom, string statut,string datet, string idTache)
        {
            Nom = nom;
            Statut = statut;
            Datet = datet;
            IdTache = idTache;
        }

        public string Nom { get; set; }
        public string Statut { get; set; }

        public string Datet { get; set; }

        public string IdTache { get; set; }
    }
}
