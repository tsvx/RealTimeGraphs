using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RtwfApp
{
	public interface IData
	{
		/// <summary>
		/// the value of the selected graph (function) at the selected point..
		/// </summary>
		/// <param name="point">the number of the point, 1 ms/point</param>
		/// <param name="graph">the number of the graph</param>
		/// <returns></returns>
		double this[int point, int graph] { get; }
	}
}
