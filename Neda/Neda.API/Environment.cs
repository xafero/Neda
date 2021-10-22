using System.Collections.Generic;

namespace Neda.API
{
	public class Environment : SortedDictionary<string, string>
	{
		public Environment() { }

		public Environment(IDictionary<string, string> dict) : base(dict) { }

		public Environment Clone()
		{
			var copy = new Environment(this);
			return copy;
		}
	}
}