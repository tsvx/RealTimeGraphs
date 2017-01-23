using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;

namespace MultimediaTimer
{
    public class TestAccTimer
    {
        static AccurateTimer mTimer1,mTimer2;

        static void Main()
        {
			int min, max, cur;
			AccurateTimer.QueryTimerResolution(out min, out max, out cur);
			Console.WriteLine("NT Timer resolution (100ns units): min={0}, max={1}, current={2}", min, max, cur);
			AccurateTimer.TimeGetDevCaps(out min, out max);
			Console.WriteLine("Multimedia Timer period (1ms units): min={0}, max={1}", min, max);

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
