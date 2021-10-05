namespace Neda.API
{
	public interface IHardware
	{
		IPower Power { get; }

		IConsole Console { get; }
	}
}