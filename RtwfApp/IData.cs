using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RtwfApp
{
	public interface IData
	{
		/// <summary>
		/// The total number of the graphs
		/// </summary>
		int NumGraphs { get; }

		/// <summary>
		/// Get all the points for the given graph in the given (semi-open) interval.
		/// </summary>
		/// <param name="graph">The number of the graph</param>
		/// <param name="beginTicks">the (open) left end of the time interval</param>
		/// <param name="endTicks">the right end of the time interval</param>
		/// <returns></returns>
		IEnumerable<KeyValuePair<long, double>> GetInterval(int graph, long beginTicks, long endTicks);
	}
}
