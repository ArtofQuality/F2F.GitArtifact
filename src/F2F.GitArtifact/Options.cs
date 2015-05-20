using System;
using System.Text;
using CommandLine;

namespace F2F.GitArtifact
{
	public class Options
	{
		[Option("upload", DefaultValue = false)]
		public bool Upload { get; set; }

		[Option("download", DefaultValue = false)]
		public bool Download { get; set; }

		[Option('d', "directory")]
		public string TargetDirectory { get; set; }

		[Option('t', "temp")]
		public string TempDirectory { get; set; }

		[Option('f', "filter")]
		public string Filter { get; set; }

		[Option('r', "repository")]
		public string Repository { get; set; }

		[Option('b', "branch")]
		public string Branch { get; set; }

		[Option('h', "help", DefaultValue = false)]
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
			sb.AppendLine(" --upload\t\tUpload artifacts");
			sb.AppendLine(" --download\t\tDownload artifacts");
			sb.AppendLine(" -d, --directory\tPath to target directory");
			sb.AppendLine(" -t, --temp\t\tPath to temp directory");
			sb.AppendLine(" -f, --filter\t\tFilter for files which have to be uploaded");
			sb.AppendLine(" -r, --repository\tGit repository url");
			sb.AppendLine(" -b, --branch\t\tGit branch name");
			sb.AppendLine(" -h, --help\t\tShow this help.");

			return sb.ToString();
		}
	}
}