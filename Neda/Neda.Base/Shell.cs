using Neda.API;

namespace Neda.Base
{
	public class Shell : IApplication
	{
		private string User { get; set; }
		private string Host { get; set; }
		private string WorkDir { get; set; }
		private bool IsRunning { get; set; }

		public Shell()
		{
			User = "root";
			Host = "localhost";
			WorkDir = "~";
		}

		private string GetPrompt()
		{
			return $"{User}@{Host}:{WorkDir}$ ";
		}

		public int Execute(IContext ctx, Environment env, string[] args)
		{
			IsRunning = true;
			env["PWD"] = WorkDir;
			var stdIn = ctx.Streams.StandardInput;
			var stdOut = ctx.Streams.StandardOutput;
			while (IsRunning)
				Execute(stdIn, stdOut, env, args);
			return 0;
		}

		private void Execute(IInStream stdIn, IOutStream stdOut,
			Environment env, string[] args)
		{
			var path = GetPrompt();
			stdOut.Write(path);
			var input = stdIn.ReadLine();
			var line = input.Trim();
			if (string.IsNullOrWhiteSpace(line))
				return;
			switch (line)
			{
				case "pwd":
					stdOut.WriteLine(env["PWD"]);
					break;
				case "ver":
					stdOut.WriteLine();
					stdOut.WriteLine($"{Constants.OsName} [Version {Constants.OsVer}]");
					stdOut.WriteLine();
					break;
				case "exit":
					IsRunning = false;
					break;


				/*case "shutdown":
					_hw.Power.Shutdown();
					break;
				case "reboot":
					_hw.Power.Reboot();
					break;*/
				default:
					stdOut.WriteLine();
					stdOut.WriteLine($"Command '{line}' not found");
					stdOut.WriteLine();
					break;
			}
		}
	}
}