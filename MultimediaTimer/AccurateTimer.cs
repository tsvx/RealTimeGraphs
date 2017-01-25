// AccurateTimer.cs
using System;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace MultimediaTimer
{
	// Needs improvement, see http://www.pinvoke.net/default.aspx/winmm.timesetevent
    public class AccurateTimer : IDisposable
    {
		private delegate void TimerEventDel(uint id, uint msg, UIntPtr user, UIntPtr dw1, UIntPtr dw2);
        private const int TIME_PERIODIC = 1;
        private const int EVENT_TYPE = TIME_PERIODIC;// + 0x100;  // TIME_KILL_SYNCHRONOUS causes a hang ?!

		[StructLayout(LayoutKind.Sequential)]
		struct TimeCaps
		{
			static readonly int size = Marshal.SizeOf(typeof(TimeCaps));
			static public int Size { get { return size; } }

			public UInt32 wPeriodMin;
			public UInt32 wPeriodMax;
		}

		[DllImport("winmm.dll")]
        private static extern int timeBeginPeriod(int msec);
        [DllImport("winmm.dll")]
        private static extern int timeEndPeriod(int msec);
        [DllImport("winmm.dll")]
        private static extern int timeSetEvent(int delay, int resolution, TimerEventDel handler, IntPtr user, int eventType);
        [DllImport("winmm.dll")]
        private static extern int timeKillEvent(int id);
		[DllImport("winmm.dll", SetLastError = true)]
		private static extern UInt32 timeGetDevCaps(ref TimeCaps timeCaps, UInt32 sizeTimeCaps);

		public static void TimeGetDevCaps(out int minPeriod, out int maxPeriod)
		{
			TimeCaps tc = new TimeCaps();
			uint rslt = timeGetDevCaps(ref tc, (uint)TimeCaps.Size);
			if (rslt != 0)
				throw new Win32Exception();
			minPeriod = (int)tc.wPeriodMin;
			maxPeriod = (int)tc.wPeriodMax;
		}

		[DllImport("ntdll.dll", SetLastError = true)]
		static extern int NtQueryTimerResolution(out int minimumResolution, out int maximumResolution, out int currentResolution);

		public static void QueryTimerResolution(out int minimumResolution, out int maximumResolution, out int currentResolution)
		{
			int rslt = NtQueryTimerResolution(out minimumResolution, out maximumResolution, out currentResolution);
			if (rslt != 0)
				throw new Win32Exception();
		}

        Action mAction;
        private int mTimerId;
        private TimerEventDel mHandler;  // NOTE: declare at class scope so garbage collector doesn't release it!!!

        public AccurateTimer(Action action,int delay)
        {
            mAction = action;
            timeBeginPeriod(1);
            mHandler = new TimerEventDel(TimerCallback);
            mTimerId = timeSetEvent(delay, 0, mHandler, IntPtr.Zero, EVENT_TYPE);
        }

        public void Stop()
        {
            int err = timeKillEvent(mTimerId);
            timeEndPeriod(1);
            System.Threading.Thread.Sleep(100);// Ensure callbacks are drained
        }

		private void TimerCallback(uint id, uint msg, UIntPtr user, UIntPtr dw1, UIntPtr dw2)
        {
			mAction();
        }

		public void Dispose()
		{
			Stop();
		}
	}
}
