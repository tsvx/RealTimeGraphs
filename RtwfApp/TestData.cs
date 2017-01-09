using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RtwfApp
{
	public class TestData : IData
	{
		int numPoints, numGraphs;
		const int N = 60000; // point in the data
		const int M = 50;	  // graphics on the plot
		double[] data;

		public int NumPoints { get { return numPoints; } }
		public int NumGraphs { get { return numGraphs; } }

		public TestData(int points, int graphs)
		{
			numPoints = points;
			numGraphs = graphs;
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

		public double this[int point, int graph] { get { return data[(point + graph * numPoints / numGraphs) % numPoints]; } }
	}
}
