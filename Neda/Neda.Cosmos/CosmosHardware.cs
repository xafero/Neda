using Neda.API;
using Sys = Cosmos.System;
using Con = System.Console;

namespace Neda.Cosmos
{
	public class CosmosHardware : IHardware, IPower, IConsole
	{
		public CosmosHardware()
		{
			Power = this;
			Console = this;
		}

		public IPower Power { get; }
		public IConsole Console { get; }

		public void Shutdown() => Sys.Power.Shutdown();

		public void Reboot() => Sys.Power.Reboot();

		public void WriteLine(string text) => Con.WriteLine(text);

		public void Clear() => Con.Clear();

		public void Write(string text) => Con.Write(text);

		public string ReadLine() => Con.ReadLine();
	}
}