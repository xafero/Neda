using Neda.API;

namespace Neda.Core
{
	internal class DeviceContext : IContext, IInputOutput, IInStream, IOutStream
	{
		private readonly IHardware _hw;

		public DeviceContext(IHardware hw)
		{
			_hw = hw;
		}

		public IInputOutput Streams => this;

		public IInStream StandardInput => this;

		public IOutStream StandardOutput => this;

		public IOutStream StandardError => this;

		public string ReadLine() => _hw.Console.ReadLine();

		public void Write(string text) => _hw.Console.Write(text);

		public void WriteLine(string text = null) => _hw.Console.WriteLine(text);
	}
}