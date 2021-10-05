using Cosmos.System;
using Neda.API;
using Neda.Core;

namespace Neda.Cosmos
{
	public class CosmosKernel : Kernel
	{
		private readonly IHardware _hw;
		private readonly MainKernel _main;

		public CosmosKernel()
		{
			_hw = new CosmosHardware();
			_main = new MainKernel(_hw);
		}

		protected override void BeforeRun()
		{
			_main.BeforeRun();
		}

		protected override void Run()
		{
			_main.Run();
		}

		protected override void AfterRun()
		{
			_main.AfterRun();
		}
	}
}