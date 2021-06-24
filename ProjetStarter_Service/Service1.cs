using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Management;
using System.ServiceProcess;
using System.Timers;

namespace ProjetStarter_Service
{
    public partial class Service1 : ServiceBase
    {



        public Service1()
        {
            InitializeComponent();
        }

        #region Fonctions
        public static float GetAVGLIST(List<float> list, string type)
        {
           // CoucheDonnee_MySQL.JournalEvent.AddEvent("J'arrive dans la méthode qui fait la moyenne");
            int compteur = 0; float somme = 0;



            foreach (float v in list)
            {
               // CoucheDonnee_MySQL.JournalEvent.AddEvent("Valeur : " + v);
                compteur++;
                //moyennes des  valeurs
                somme += v;

            }

            //CoucheDonnee_MySQL.JournalEvent.AddEvent("Somme : " + somme);
            //CoucheDonnee_MySQL.JournalEvent.AddEvent("Compteur : " + compteur);

            return somme / compteur;
        }

        public void GetValueCPU()
        {
            //CoucheDonnee_MySQL.JournalEvent.AddEvent("J'arrive dans la méthode GetValueCPU");
            int i = 0;
            //Peut être à modifier
            PerformanceCounter cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
            CounterSample cs1 = cpuCounter.NextSample();
            CounterSample cs2 = cpuCounter.NextSample();
            float finalCpuCounter = 0;
            if (cs1 != null && cs2 != null)
            {
                finalCpuCounter = CounterSample.Calculate(cs1, cs2);
            }
            //if(cs1 != null && cs2==null)
            //{
            //    finalCpuCounter = cs1.RawValue;
            //}
            //if (cs2 != null && cs1 == null)
            //{
            //    finalCpuCounter = cs2.RawValue;
            //}

            if (CPUList is null)
            {
                CPUList = new List<float>();
            }

            if (finalCpuCounter != 0)
            {
                CPUList.Add(finalCpuCounter);
            }
        }

        public void GetValueRAM()
        {
            //CoucheDonnee_MySQL.JournalEvent.AddEvent("J'arrive dans la méthode GetValueRAM");

            //Peut être à modifier
            PerformanceCounter ramCounter;
            ramCounter = new PerformanceCounter("Memory", "Available MBytes");


            ObjectQuery wql = new ObjectQuery("SELECT * FROM Win32_OperatingSystem");
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(wql);
            ManagementObjectCollection results = searcher.Get();
            float totalram = 0;
            float finalRAMCounter;

            foreach (ManagementObject result in results)
            {
                totalram = Convert.ToInt32(result["TotalVisibleMemorySize"]);// Ram total en MB

            }
            totalram = totalram / 1024;
            finalRAMCounter = ((totalram - ramCounter.NextValue()) / totalram) * 100;

            if (finalRAMCounter != 0)
            {
                RAMList.Add(finalRAMCounter);
            }

        }


        public float GetDisk()
        {
            //CoucheDonnee_MySQL.JournalEvent.AddEvent("J'arrive dans la méthode GetDisk");
            DriveInfo[] allDrives = DriveInfo.GetDrives();
            float spacedispo = 0, totalspace = 0;

            spacedispo = allDrives[0].AvailableFreeSpace;

            totalspace = allDrives[0].TotalSize;


            return (spacedispo / totalspace) * 100;
        }

        public void gestionInsert(Tasks mytask, float value)
        {
            CoucheDonnee_MySQL.JournalEvent.AddEvent("J'arrive dans la méthode gestioninsert "+mytask.Nomtache +"/"+value);
            if (value >= 90)
            {
                WS.InsertNewDashboard("KO", mytask.idT, value);
            }
            if ((value <= 90) && (value >= 70))
            {
                WS.InsertNewDashboard("NOK", mytask.idT, value);
            }
            if (value < 70)
            {
                WS.InsertNewDashboard("OK", mytask.idT, value);
            }
        }


        private void Insert(Tasks mytask)
        {
           // CoucheDonnee_MySQL.JournalEvent.AddEvent("J'arrive dans la méthode insert");
            switch (mytask.Nomtache.ToLower())
            {
                /*
                 case "Ping":
                     if (PingHost(mytask.Host))
                     {
                         WS.InsertNewDashboard("OK", mytask.idT, valeurping);
                     }
                     else
                     {
                         WS.InsertNewDashboard("KO", mytask.idT, valeurping);
                     }
                     break;
                */
                case "cpu":
                    CoucheDonnee_MySQL.JournalEvent.AddEvent("insert cpu" );
                    gestionInsert(mytask, GetAVGLIST(CPUList, "cpu"));
                    CPUList.Clear();
                    break;

                case "ram":
                    CoucheDonnee_MySQL.JournalEvent.AddEvent("insert ram");
                    gestionInsert(mytask, GetAVGLIST(RAMList, "ram"));
                    RAMList.Clear();
                    break;

                case "disque":
                    CoucheDonnee_MySQL.JournalEvent.AddEvent("insert disque" + GetDisk());
                    gestionInsert(mytask, GetDisk());
                    break;

                case "":
                    break;
            }

        }
        #endregion

        List<TimerService> Timerlist = new List<TimerService>();
        WebService WS = new WebService();
        List<float> CPUList = new List<float>();
        List<float> RAMList = new List<float>();

        protected override void OnStart(string[] args)
        {
            try
            {
                CoucheDonnee_MySQL.JournalEvent.AddEvent("Démarrage ! ");
                string service = ConfigurationManager.AppSettings["Nom_du_service"];
               

                foreach (Tasks value in WS.GetTaches(service))
                {
                    if (value.Nomtache == "CPU" || value.Nomtache == "Disque" || value.Nomtache == "RAM")
                    {
                        TimerService TEnglobe = new TimerService();
                        Timer T_Task = new Timer();
                        T_Task.Interval = value.Frequence * 6000; //Millisecondes*1000 = sec
                        TEnglobe.Tag = value.Nomservice + "$" + value.Nomtache + "$" + value.Frequence + "$" + value.idT + "$" + value.Host;
                        T_Task.Elapsed += new ElapsedEventHandler(timer1_Tick);
                        T_Task.Start();
                        TEnglobe.T = T_Task;
                        Timerlist.Add(TEnglobe);
                        //CoucheDonnee_MySQL.JournalEvent.AddEvent(value.Nomtache + " added "+ T_Task.Interval.ToString());
                    
                  
                        if (value.Nomtache == "CPU")
                        {
                            TimerService TE_CPU = new TimerService();
                            Timer T_CPU = new Timer();
                            T_CPU.Interval = value.Frequence * 1000;
                            TE_CPU.Tag = value.Nomservice + "$" + value.Nomtache + "$" + value.Frequence + "$" + value.idT + "$" + value.Host;
                            T_CPU.Elapsed += new ElapsedEventHandler(timer2_Tick);
                            T_CPU.Start();
                            TE_CPU.T = T_CPU;
                            Timerlist.Add(TE_CPU);
                  
                        }

                        if (value.Nomtache == "RAM")
                        {
                            TimerService TE_RAM = new TimerService();
                            Timer T_RAM = new Timer();
                            T_RAM.Interval = value.Frequence *1000;
                            TE_RAM.Tag = value.Nomservice + "$" + value.Nomtache + "$" + value.Frequence + "$" + value.idT + "$" + value.Host;
                            T_RAM.Elapsed += new ElapsedEventHandler(timer3_Tick);
                            T_RAM.Start();
                            TE_RAM.T = T_RAM;
                            Timerlist.Add(TE_RAM);

                        }

                    }

                }
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

                Tasks myTask = new Tasks(NomTache, NomService, frequence, idT, host);
                CoucheDonnee_MySQL.JournalEvent.AddEvent("J'arrive dans la méthode timer1 "+ NomTache.ToString()+" "+ T_Task.Interval.ToString());
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
            //CoucheDonnee_MySQL.JournalEvent.AddEvent("J'arrive dans la méthode timer2");
            try
            {
                Timer T_CPU = (Timer)sender;
                TimerService TE_CPU;
                TE_CPU = Timerlist.Find(f => f.T == T_CPU);
                T_CPU.Stop();
             
                    
                GetValueCPU();
               //CoucheDonnee_MySQL.JournalEvent.AddEvent("timer 2 cpu " + CPUList.Count);
                

                T_CPU.Start();

            }
            catch (Exception ex)
            {
                WS.AddErreur(ex);
            }
        }

        private void timer3_Tick(object sender, EventArgs e)
        {
            //CoucheDonnee_MySQL.JournalEvent.AddEvent("J'arrive dans la méthode timer2");
            try
            {
                Timer T_RAM = (Timer)sender;
                TimerService TE_RAM;
                TE_RAM = Timerlist.Find(f => f.T == T_RAM);
                T_RAM.Stop();
               
                    
                GetValueRAM();
               // CoucheDonnee_MySQL.JournalEvent.AddEvent("timer 2 ram " + RAMList.Count);

                T_RAM.Start();

            }
            catch (Exception ex)
            {
                WS.AddErreur(ex);
            }
        }


    }
}
