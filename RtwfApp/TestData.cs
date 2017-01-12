using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RtwfApp
{
	public class TestData : IData
	{
		int numPoints, numGraphs;
		long ticks2point;
		const int N = 60000; // point in the data
		const int M = 50;	  // graphics on the plot
		double[] data;

		public int NumPoints { get { return numPoints; } }
		public int NumGraphs { get { return numGraphs; } }

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="points">The number of points in each graph</param>
		/// <param name="graphs">The number of graphs</param>
		/// <param name="period">The period between points</param>
		public TestData(int points, int graphs, TimeSpan period)
		{
			numPoints = points;
			numGraphs = graphs;
			ticks2point = period.Ticks;
			InitData();
		}

		void InitData()
		{
			data = new double[numPoints];
			var rnd = new Random();
			double x = 0, v = 0, min = 0, max = 0;
			for (int i = 0; i < numPoints; i++)
			{
				if (x < min) min = x;
				if (x > max) max = x;
				data[i] = x;
				x += v / 1000;
				v += (rnd.NextDouble() - 0.5) / 1000;
			}
			for (int i = 0; i < numPoints; i++)
				data[i] = (data[i] - min) / (max - min);
		}

		static dynamic Mod(dynamic a, dynamic b)
		{
			if (b < 0) b = -b;
			a = a % b;
			if (a < 0) a += b;
			return a;
		}
		//static long Mod(long a, long b) { if (b < 0) b = -b; a = a % b; return a < 0 ? a + b : b; }
		
		public IEnumerable<KeyValuePair<long, double>> GetInterval(int graph, long beginTicks, long endTicks)
		{
			long ticks = beginTicks - Mod(beginTicks, ticks2point);
			int graphShift = graph * numPoints / numGraphs;
			int point = (int)Mod(beginTicks / ticks2point - graphShift, numPoints);

			for (; ticks <= endTicks; ticks += ticks2point, point++)
				yield return new KeyValuePair<long, double>(ticks, data[Mod(point, numPoints)]);
		}
	}
}
