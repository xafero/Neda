namespace Neda.API
{
	public interface IApplication
	{
		int Execute(IContext ctx, Environment env, string[] args);
	}
}