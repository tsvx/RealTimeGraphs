using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RtwfApp
{
	public interface IData
	{
		double this[int point, int graph] { get; }
	}
}
