using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace ProjetStarter_Service
{
    class TimerService
    {
        public TimerService(string tag, Timer t)
        {
            Tag = tag;
            T = t;
        }

        public TimerService()
        {
            Tag = "";
            T = new Timer();
        }

        public string Tag { get; set; }
        public Timer T { get; set; }
    }
}
