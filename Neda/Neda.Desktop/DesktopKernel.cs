using Neda.API;
using Neda.Core;

namespace Neda.Desktop
{
	public class DesktopKernel
	{
		private readonly IHardware _hw;
		private readonly MainKernel _main;

		public DesktopKernel(IConsole con)
		{
			_hw = new DesktopHardware(con);
			_main = new MainKernel(_hw);
		}

		public bool IsStopped { get; private set; }

		public void BeforeRun()
		{
			_main.BeforeRun();
		}

		public void Run()
		{
			IsStopped = false;
			while (!IsStopped)
				_main.Run();
		}

		public void AfterRun()
		{
			_main.AfterRun();
		}
	}
}