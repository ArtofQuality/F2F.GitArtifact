using System;
using System.Text;
using CommandLine;

namespace F2F.GitArtifact
{
	public class Options
	{
		[Option('u', "upload", DefaultValue = false, Required = false)]
		public bool Upload { get; set; }

		[Option('d', "download", DefaultValue = false, Required = false)]
		public bool Download { get; set; }

		[Option('p', "directory", Required = false)]
		public string Directory { get; set; }

		[Option('f', "filter", Required = false)]
		public string Filter { get; set; }

		[Option('r', "repository", Required = true)]
		public string RepositoryUrl { get; set; }

		[Option('b', "branch", Required = true)]
		public string Branch { get; set; }

		[Option('h', "help", DefaultValue = false, Required = false)]
		public bool Help { get; set; }

		[ParserState]
		public IParserState LastParserState { get; set; }

		[HelpOption]
		public string GetUsage()
		{
			var sb = new StringBuilder();
			sb.AppendLine(GetType().Assembly.FullName);
			sb.AppendLine(String.Format("Version: {0}", GetType().Assembly.GetName().Version));
			sb.AppendLine();
			sb.AppendLine("Parameter:");
			sb.AppendLine(" -u, --upload\tUpload artifacts");
			sb.AppendLine(" -d, --download\tDownload artifacts");
			sb.AppendLine(" -h, --help\tShow this help.");

			return sb.ToString();
		}
	}
}