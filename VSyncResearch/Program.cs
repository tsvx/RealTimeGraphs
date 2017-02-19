using DxProxy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VSyncResearch
{
	class Program
	{
		static void Main(string[] args)
		{
			var viewport = new ViewportDX(new ViewportParams
			{
				WindowHeight = 1,
				WindowWidth = 1,
				FullScreen = false
			});
			bool exit = false;
			Console.CancelKeyPress += (s, e) =>
			{
				if (e.SpecialKey == ConsoleSpecialKey.ControlC)
					e.Cancel = true;
				exit = true;
			};
			var acc = new SortedDictionary<int, long>();
			var acc2 = new SortedDictionary<int, long>();
			while (!exit)
			{
				int scanline;
				bool invblank = viewport.GetRasterStatus(out scanline);
				long n;
				if (invblank)
				{
					acc.TryGetValue(scanline, out n);
					acc[scanline] = n + 1;
				}
				else
				{
					acc2.TryGetValue(scanline, out n);
					acc2[scanline] = n + 1;
				}
			}
			Console.WriteLine("{0} InVBlank", acc.Values.Sum());
			foreach (var p in acc)
				Console.WriteLine("{0,3}: {1,8}", p.Key, p.Value);
			Console.WriteLine("{0} not InVBlank", acc2.Values.Sum());
			foreach (var p in acc2)
				Console.WriteLine("{0,3}: {1,8}", p.Key, p.Value);
		}
	}
}
