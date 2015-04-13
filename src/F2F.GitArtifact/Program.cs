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

			//var directory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
			//var remoteUrl = "ssh://git@stash.aoq.local:7999/aoq/f2f.git";
			//var branch = "MOEP";

			try
			{
				var options = new Options();

				if (Parser.Default.ParseArguments(args, options) && !options.Help)
				{
					result = Run(options) ? 0 : 1;
				}
				else
				{
					Console.WriteLine(options.GetUsage());
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

				if (o.Upload) new UploadArtifactsToGitRepository(logger, o.Directory, filter, o.RepositoryUrl, o.Branch).Upload();
				else if (o.Download) new DownloadArtifactsFromGitRepository(logger, o.Directory, filter, o.RepositoryUrl, o.Branch).Download();
				else throw new ArgumentException("You have to specify -u or -d!");

				isSuccessful = true;
			}
			catch (Exception e)
			{
				Console.Error.WriteLine(e);
			}

			return isSuccessful;
		}
	}
}