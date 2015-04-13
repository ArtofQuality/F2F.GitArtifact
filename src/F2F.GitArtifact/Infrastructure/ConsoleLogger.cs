using System;
using System.Collections.Generic;
using System.Linq;

namespace F2F.GitArtifact
{
	public class ConsoleLogger : ILogger
	{
		public void Info(string format, object[] args)
		{
			Console.WriteLine(format, args);
		}

		public void Error(string format, object[] args)
		{
			Console.Error.WriteLine(format, args);
		}
	}
}