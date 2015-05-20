using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CommandLine;
using F2F.FileFilter;

namespace F2F.GitArtifact
{
	internal class Program
	{
		private static int Main(string[] args)
		{
			var result = -1;

			try
			{
				var options = new Options();

				if (Parser.Default.ParseArguments(args, options))
				{
					if (!options.Help)
					{
						result = Run(options) ? 0 : 1;
					}
					else
					{
						Console.WriteLine(options.GetUsage());
					}
				}
			}
			catch (Exception e)
			{
				Console.Error.WriteLine(e);
			}

			return result;
		}

		private static bool Run(Options o)
		{
			var isSuccessful = false;

			try
			{
				var logger = new ConsoleLogger();
				var filter = new FilterParser().Parse(o.Filter);

				if (o.Upload) new UploadArtifactsToGitRepository(logger, filter, o.TargetDirectory, o.TempDirectory, o.Repository, o.Branch).Upload();
				else if (o.Download) new DownloadArtifactsFromGitRepository(logger, filter, o.TargetDirectory, o.TempDirectory, o.Repository, o.Branch).Download();
				else throw new ArgumentException("You have to specify -u or -d!");

				isSuccessful = true;
			}
			catch (Exception e)
			{
				Console.Error.WriteLine(e.Message);
			}

			return isSuccessful;
		}
	}
}