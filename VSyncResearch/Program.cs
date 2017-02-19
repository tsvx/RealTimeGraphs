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
			bool exit = false;
			Console.CancelKeyPress += (s, e) =>
			{
				if (e.SpecialKey == ConsoleSpecialKey.ControlC)
					e.Cancel = true;
				exit = true;
			};
			long inVBlank = 0;
			var acc = new SortedDictionary<int, long>();
			using (var viewport = new ViewportDX(new ViewportParams
			{
				WindowHeight = 1,
				WindowWidth = 1,
				FullScreen = false
			}))
			{
				while (!exit)
				{
					int scanline = viewport.GetScanline();
					long n;
					if (scanline < 0)
					{
						inVBlank++;
					}
					else
					{
						acc.TryGetValue(scanline, out n);
						acc[scanline] = n + 1;
					}
				}
			}
			Console.WriteLine("{0} InVBlank", inVBlank);
			Console.WriteLine("{0} not InVBlank", acc.Values.Sum());
			foreach (var p in acc)
				Console.WriteLine("{0,3}: {1,8}", p.Key, p.Value);
		}
	}
}
