namespace Neda.API
{
	public interface IConsole
	{
		void WriteLine(string text);

		void Clear();

		void Write(string text);

		string ReadLine();
	}
}