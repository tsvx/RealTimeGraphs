using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NIIT.Utils
{
	public class StatsAccum
	{
		internal double bias { get; private set; }
		double min, max;
		internal double sum { get; private set; }
		internal double sum2 { get; private set; }
		long n;

		public double Min { get { return min; } }
		public double Max { get { return max; } }
		public double Sum { get { return sum + n * bias; } }
		public long Count { get { return n; } }
		public double Avg { get { return Sum / Count; } }
		public double Sum2 { get { return sum2 + 2 * bias * sum + n * bias * bias; } }
		public double D { get { return (sum2 - sum * sum / n) / n; } }			// состоятельная оценка выборочной дисперсии (смещённая)
		public double Var { get { return (sum2 - sum * sum / n) / (n - 1); } }	// состоятельная оценка выборочной дисперсии (несмещённая)
		public double Dev { get { return Math.Sqrt(Var); } }					// стандартное отклонение (состоятельная оценка среднеквадратического отклонения)
		public double DevAvg { get { return Math.Sqrt(Var / n); } }				// оценка с.к.о. оценки мат. ожидания

		public StatsAccum() : this(false) { }
		public StatsAccum(bool useFirstValueAsBias) : this(useFirstValueAsBias ? Double.NaN : 0) { }

		private StatsAccum(double bias) { Reset(); this.bias = bias; }

		public void Reset()
		{
			n = 0;
			min = double.MaxValue;
			max = double.MinValue;
			sum = sum2 = 0;
		}

		public void Add(double x)
		{
			n++;
			if (Double.IsNaN(bias))
				bias = x;
			if (x < Min) min = x;
			if (x > Max) max = x;
			x -= bias;
			sum += x;
			sum2 += x * x;
		}

		public void AddRange(IEnumerable<double> seq)
		{
			foreach (var x in seq) Add(x);
		}

		public override string ToString()
		{
			return String.Format("{0} ± {1} ∈ [{2}; {3}], s(M) = {4}", Avg, Dev, Min, Max, DevAvg);
		}

		public string ToShortString()
		{
			return String.Format("{0:G6} ± {1:G5} ∈ [{2:G7}; {3:G7}]", Avg, Dev, Min, Max);
		}
	}
}
