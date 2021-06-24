using System;
using System.Collections.Generic;
using System.ServiceProcess;
using System.Timers;
using System.Net.NetworkInformation;


namespace Service_Network
{
    public partial class Service_Network : ServiceBase
    {
        public Service_Network()
        {
            InitializeComponent();
        }

        #region Fonctions

        public static bool PingHost(string nameOrAddress)
        {
            bool pingable = false;
            Ping pinger = null;

            try
            {
                pinger = new Ping();
                PingReply reply = pinger.Send(nameOrAddress);
                if (reply.RoundtripTime < 1 && nameOrAddress!= null)
                    valeurping = 1;
                else
                valeurping = reply.RoundtripTime; // Assigne les ms du ping
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

        public void gestionInsert(Tasks mytask, float value, bool statut)
        {
            if ((value <= 100)&&(statut))
            {
                WS.InsertNewDashboard("OK", mytask.idT, value, mytask.Nomservice);
            }
            if ((value >= 100)&&(statut))
            {
                WS.InsertNewDashboard("NOK", mytask.idT, value, mytask.Nomservice);
            }
            if (!statut)
            {
                WS.InsertNewDashboard("KO", mytask.idT, value,mytask.Nomservice);
            }
        }


        private void Insert(Tasks mytask)
        {
            switch (mytask.Nomtache)
            {
                 case "Ping-ip":
                     if (PingHost(mytask.Host))
                     {
                         gestionInsert(mytask,valeurping,true);
                     }
                     else
                     {
                         gestionInsert(mytask, valeurping, false);
                     }
                     break;

                case "Ping-dns":
                    if (PingHost(mytask.Host))
                    {
                        gestionInsert(mytask, valeurping, true);
                    }
                    else
                    {
                        gestionInsert(mytask, valeurping, false);
                    }
                    break;


                case "":
                    break;
            }

        }

        #endregion

        List<TimerService> Timerlist = new List<TimerService>();
        WebService WS = new WebService();
        static float valeurping;

        protected override void OnStart(string[] args)
        {
            try
            {

                foreach (Tasks value in WS.GetTaches())
                {
                    TimerService TEnglobe = new TimerService();
                    Timer T_Task = new Timer();
                    T_Task.Interval = value.Frequence * 60000; //Millisecondes*1000 = sec
                    TEnglobe.Tag = value.Nomservice + "$" + value.Nomtache + "$" + value.Frequence + "$" + value.idT + "$" + value.Host + "$" + value.Dns;
                    //T_Task.Tag = value;
                    T_Task.Elapsed += new ElapsedEventHandler(timer1_Tick);
                    T_Task.Start();
                    TEnglobe.T = T_Task;
                    Timerlist.Add(TEnglobe);
                }


                Timer T_Vide = new Timer();
                T_Vide.Interval = 1800000;
                T_Vide.Elapsed += new ElapsedEventHandler(timer2_Tick);

            }

            catch (Exception ex)
            {
                WS.AddErreur(ex);
            }

        }

        protected override void OnStop()
        {
            try
            {
                // Arrête tous les timers 
                foreach (TimerService TimerS in Timerlist)
                {
                    TimerS.T.Stop();
                }
                Timerlist.Clear();
            }
            catch (Exception ex)
            {
                WS.AddErreur(ex);
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                Timer T_Task = (Timer)sender;
                TimerService TEnglobe;

                TEnglobe = Timerlist.Find(f => f.T == T_Task);

                T_Task.Stop();
                string NomService = TEnglobe.Tag.ToString().Split('$')[0];
                string NomTache = TEnglobe.Tag.ToString().Split('$')[1];
                int frequence = Int32.Parse(TEnglobe.Tag.ToString().Split('$')[2]);
                int idT = Int32.Parse(TEnglobe.Tag.ToString().Split('$')[3]);
                string host = TEnglobe.Tag.ToString().Split('$')[4];
                string Dns = TEnglobe.Tag.ToString().Split('$')[5];

                Tasks myTask = new Tasks(NomTache, NomService, frequence, idT, host,Dns);

                Insert(myTask);
                T_Task.Start();
            }

            catch (Exception ex)
            {
                WS.AddErreur(ex);
            }
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            Timer T_Vide = (Timer)sender;
            T_Vide.Stop();
            WS.DeleteOldlogs();
            T_Vide.Start();
        }
    }
}
