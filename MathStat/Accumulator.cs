using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.IO;

namespace NIIT.Utils
{
	// TODO: Achtung!! _Очень_ медленно. Если ключей немного, лучше использовать массив.
	public class Accumulator<TKey, TValue> : SortedDictionary<TKey, TValue>
	{
		//public Accumulator() { }
		new public TValue this[TKey key]
		{
			get
			{
				TValue val;
				return base.TryGetValue(key, out val) ? val : default(TValue);
			}
			set { base[key] = value; }
		}

		public TKey MinKey { get { return this.Keys.Min(); } }
		public TKey MaxKey { get { return this.Keys.Max(); } }
		public TKey MostlyKey { get { return this.OrderByDescending(p => p.Value).First().Key; } }
	}

	public class Accumulator<TKey> : Accumulator<TKey, long>
	{
		public override string ToString()
		{
			int d = this.Values.Min() < 0 ? 1 : 0;
			long max = this.Values.Max(x => Math.Abs(x));
			d += max == 0 ? 1 : (int)Math.Ceiling(Math.Log10(max + 0.5));
			string fmt = String.Format("{{0,{0}}} of {{1}}", d);
			var sw = new StringWriter();
			foreach (var p in this)
				sw.WriteLine(fmt, p.Value, p.Key);
			return sw.ToString();
		}
	}

	public class Accumulator : Accumulator<long>
	{
		public double AverageKey { get { return SumKeysValues / (double)SumValues; } }
		public long SumKeysValues { get { return this.Select(p => p.Key * p.Value).Sum(); } }
		public long SumValues { get { return this.Values.Sum(); } }
	}
}
