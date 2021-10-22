using Neda.API;
using Neda.Base;

namespace Neda.Core
{
	public class MainKernel
	{
		private readonly IHardware _hw;

		public MainKernel(IHardware hw)
		{
			_hw = hw;
		}

		public void BeforeRun()
		{
			_hw.Console.WriteLine($"Starting {Constants.OsName}...");
			_hw.Console.WriteLine();
			_hw.Console.WriteLine();
			_hw.Console.WriteLine("Booted successfully.");
			_hw.Console.WriteLine();
			_hw.Console.WriteLine();
		}

		public void Run()
		{
			var shell = new Shell();
			var ctx = new DeviceContext(_hw);
			var env = new Environment();
			shell.Execute(ctx, env, new[] { "init" });
		}

		public void AfterRun()
		{
			_hw.Console.WriteLine("Kernel stopped.");
		}
	}
}