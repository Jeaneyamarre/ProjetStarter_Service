using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.NetworkInformation;
using System.Net.Http;

namespace Projet_Starter_WinformsCSharp
{
    public partial class FormStarter : Form
    {
        WebService WS = new WebService();
        //static readonly HttpClient client = new HttpClient();
        List<Timer> Timerlist = new List<Timer>();
        static String valeur;
        public FormStarter()
        {
            InitializeComponent();
            //timer1.Start();

        }

        private void Form1_Load_1(object sender, EventArgs e)
        {

        }


        public static bool PingHost(string nameOrAddress)
        {
            bool pingable = false;
            Ping pinger = null;


            try
            {
                pinger = new Ping();
                PingReply reply = pinger.Send(nameOrAddress);
                valeur = reply.RoundtripTime.ToString();// Assigne les ms du ping
                pingable = reply.Status == IPStatus.Success;

            }
            catch (PingException)
            {
                // Discard PingExceptions and return false;
                pingable = false;
            }
            finally
            {
                if (pinger != null)
                {
                    pinger.Dispose();
                }
            }

            return pingable;
        }

        private void Insert(Tasks mytask)
        {
            bool ping = false;

            switch (mytask.Nomtache.ToLower())
            {
                case "ping": ping = PingHost(mytask.Host); break;

                case "cpu": /*test = Mafonction();*/ break;

                case "disque": /*test = Mafonction();*/break;

                case "": break;
            }


            if (ping)
            {
                WS.InsertNewDashboard("OK", mytask.idT, valeur);
            }
            else
            {
                WS.InsertNewDashboard("KO", mytask.idT,valeur);
            }

        }


        private void button3_Click(object sender, EventArgs e)
        {
            foreach(Timer T in Timerlist) 
            {
                T.Stop();
            }
            Timerlist.Clear();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            string service = "LabVantage";
            // GetService dans un tableau pour futur traitement dynamique de tous les services


            foreach (Tasks value in WS.GetTaches(service))
            {
                Timer T_Task = new Timer();
                T_Task.Interval = value.Frequence * 1000; //Millisecondes*1000
                T_Task.Tag = value.Nomservice + "$" + value.Nomtache + "$" +value.Frequence+ "$" + value.idT+ "$" + value.Host;
                //T_Task.Tag = value;
                T_Task.Tick += new EventHandler(timer1_Tick);
                T_Task.Start();
                Timerlist.Add(T_Task);
            }

            // Timer suppression données >7jours
            timer2.Interval = 18000000; //30 min
            timer2.Tick += new EventHandler(timer2_Tick);
            timer2.Start();
            Timerlist.Add(timer2);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            Timer T_Task = (Timer)sender;
            
            T_Task.Stop();
            string NomService = T_Task.Tag.ToString().Split('$')[0];
            string NomTache = T_Task.Tag.ToString().Split('$')[1];
            int frequence = Int32.Parse(T_Task.Tag.ToString().Split('$')[2]);
            int idT = Int32.Parse(T_Task.Tag.ToString().Split('$')[3]);
            string host = T_Task.Tag.ToString().Split('$')[4];

            Tasks myTask = new Tasks(NomTache, NomService,frequence,idT,host);

            switch (NomTache.ToLower())
            {
                case "ping":
                    Insert(myTask);
                    break;

                case "cpu": /*timer1.Interval = value.Frequence*1000;
                                 timer1.Start();
                                 task = value.Nomtache.ToLower(); 
                                 test = Mafonction();*/
                    break;

                case "disque": /*timer1.Interval = value.Frequence*1000;
                                 timer1.Start();
                                 task = value.Nomtache.ToLower(); 
                                 test = Mafonction();*/
                    break;

                case "": break;

            }

            //Insert(task);
            T_Task.Start();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            timer2.Stop();
            WS.DeleteOldlogs();
            timer2.Start();
        }
    }



}
