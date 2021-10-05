using Neda.API;

namespace Neda.Core
{
	public class MainKernel
	{
		private readonly IHardware _hw;
		private bool _firstRun;

		public MainKernel(IHardware hw)
		{
			_hw = hw;
			_firstRun = true;
		}

		public void BeforeRun()
		{
			_hw.Console.WriteLine("Booted successfully.");
		}

		public void Run()
		{
			if (_firstRun)
			{
				_firstRun = false;
				_hw.Console.Clear();
				return;
			}

			_hw.Console.Write("Input: ? ");
			var input = _hw.Console.ReadLine();
			_hw.Console.Write("Text typed: ");
			_hw.Console.WriteLine(input);

			if (input == "shutdown")
			{
				_hw.Power.Shutdown();
			}
			else if (input == "reboot")
			{
				_hw.Power.Reboot();
			}
		}

		public void AfterRun()
		{
			_hw.Console.WriteLine("Kernel stopped.");
		}
	}
}