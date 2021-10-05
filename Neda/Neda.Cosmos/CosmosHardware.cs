using Neda.API;
using Sys = Cosmos.System;

namespace Neda.Cosmos
{
	public class CosmosHardware : IHardware, IPower
	{
		public CosmosHardware()
		{
			Power = this;
		}

		public IPower Power { get; }

		public void Shutdown()
		{
			Sys.Power.Shutdown();
		}

		public void Reboot()
		{
			Sys.Power.Reboot();
		}
	}
}