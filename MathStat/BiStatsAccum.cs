using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace NIIT.Utils
{
	public class BiStatsAccum
	{
		public StatsAccum X, Y;
		double A0;
		public long Count { get { return X.Count; } }
		double sumxy;

		double cov { get { return (sumxy - X.sum * Y.sum / Count) / Count; } }	// Ковариация
		public double Cov { get { return cov + A0 * X.D; } }		// Ковариация
		public double A { get { return A0 + cov / X.D; } }			// наклон линейной регрессии
		double b { get { return (X.sum2 * Y.sum - X.sum * sumxy) / (Count * Count * X.D); } }
		public double B { get { return b + Y.bias - A * X.bias; } }	// константа линейной регрессии

		double yD { get { return Y.D + A0 * A0 * X.D + 2 * A0 * cov; } }
		
		public double Corr { get { return Cov / Math.Sqrt(X.D * yD); } }		// Коэффициент корреляции
		public double Err0 { get { return (1 - Corr) * (1 + Corr); } }
		//public double Err { get { return Err0 * Y.D * Count; } }
		public double Err { get { return (Y.D - cov * cov / X.D) * Count; } }	// Ошибка лин. регрессии: Sum (A*x+B-y)^2
		public double Err1 { get { return Math.Sqrt(Y.D - cov * cov / X.D); } }	// Приведённая ошибка лин. регрессии (с.к.о. на одну точку)
		public double DevErr { get { return Math.Sqrt(Err / (Count - 2)); } }
		public double DevErr1 { get { return Math.Sqrt(Err1 / (Count - 2)); } }
		public double DevA { get { return DevErr / Math.Sqrt(X.D * Count); } }
		//public double DevB { get { return DevErr * Math.Sqrt(X.Sum2 / X.D) / Count; } }
		public double DevB { get { return DevA * Math.Sqrt(X.Sum2 / Count); } }

		public BiStatsAccum() : this(0, false) { }
		
		public BiStatsAccum(double slopeBias, bool useValueBias)
		{
			X = new StatsAccum(useValueBias);
			Y = new StatsAccum(useValueBias);
			A0 = slopeBias;
			Reset();
		}

		public void Reset()
		{
			X.Reset();
			Y.Reset();
			sumxy = 0;
		}

		public void Add(double x, double y)
		{
			X.Add(x);
			x -= X.bias;
			y -= x * A0;
			Y.Add(y);
			y -= Y.bias;
			sumxy += x * y;
		}

		public void AddRange(IEnumerable<KeyValuePair<double, double>> seq)
		{
			foreach (var p in seq)
				Add(p.Key, p.Value);
		}

		public double GetY(double x) { return A * x + B; }

		public override string ToString()
		{
			return String.Format("Y = {0} * X + {1}, 1 - corr = {2}, s(err) = {3}", A, B, 1 - Corr, DevErr);
		}

		public string ToShortString()
		{
			return String.Format("Y = {0:G5} * X + {1:G5}, 1 - corr = {2:G4}, s(err) = {3:G4}", A, B, 1 - Corr, DevErr);
		}

		public string XToString() { return X.ToString(); }
		public string YToString() { return Y.ToString(); }
	}
}
