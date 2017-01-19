using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;

namespace YourProjectsNamespace
{
    public class TestAccTimer
    {
        static AccurateTimer mTimer1,mTimer2;

        static void Main()
        {
            int delay = 10;   // In milliseconds. 10 = 1/100th second.
            mTimer1 = new AccurateTimer(new Action(TimerTick1),delay);
            delay = 100;      // 100 = 1/10th second.
            mTimer2 = new AccurateTimer(new Action(TimerTick2), delay);

            Console.WriteLine("Press Enter.");
            Console.ReadLine();
            
            mTimer1.Stop();
            mTimer2.Stop();
        }

        static private void TimerTick1()
        {
            // Put your first timer code here!
        }

        static private void TimerTick2()
        {
            // Put your second timer code here!
        }
    }
}
