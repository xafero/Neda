using System;
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
			Console.WriteLine("Booted successfully.");
		}

		public void Run()
		{
			if (_firstRun)
			{
				_firstRun = false;
				Console.Clear();
				return;
			}

			Console.Write("Input: ? ");
			var input = Console.ReadLine();
			Console.Write("Text typed: ");
			Console.WriteLine(input);

			if (input == "shutdown")
			{
				_hw.Power.Shutdown();
			}
			else if (input == "reboot")
			{
				_hw.Power.Reboot();
			}
		}
	}
}