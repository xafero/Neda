namespace Neda.API
{
	public interface IConsole
	{
		void WriteLine(string text = null);

		void Clear();

		void Write(string text);

		string ReadLine();
	}
}