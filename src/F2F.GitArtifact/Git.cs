using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reactive.Linq;

namespace F2F.GitArtifact
{
	public class Git
	{
		private const string GIT_EXE = "git.exe";

		private readonly ILogger _logger;
		private readonly string _directory;
		private readonly string _gitPath;

		public Git(ILogger logger, string directory)
			: this(logger, directory, GIT_EXE)
		{
		}

		public Git(ILogger logger, string directory, string gitPath)
		{
			_logger = logger;
			_directory = directory;
			_gitPath = gitPath;
		}

		public string Directory
		{
			get { return _directory; }
		}

		private int Execute(string arguments)
		{
			var ps = new ProcessStartInfo()
			{
				FileName = _gitPath,
				WorkingDirectory = _directory,
				Arguments = arguments,
				UseShellExecute = false,
				RedirectStandardOutput = true,
				RedirectStandardError = true
			};

			using (var p = Process.Start(ps))
			using (MonitorStandardOutput(p))
			using (MonitorStandardError(p))
			{
				p.BeginOutputReadLine();
				p.BeginErrorReadLine();

				p.WaitForExit();

				return p.ExitCode;
			}
		}

		private IDisposable MonitorStandardOutput(Process p)
		{
			return Observable
				.FromEventPattern<DataReceivedEventArgs>(p, "OutputDataReceived")
				.Select(e => e.EventArgs.Data)
				.DistinctUntilChanged()
				.Where(m => m != null)
				.Do(m => _logger.Info(m))
				.Subscribe();
		}

		private IDisposable MonitorStandardError(Process p)
		{
			return Observable
				.FromEventPattern<DataReceivedEventArgs>(p, "ErrorDataReceived")
				.Select(e => e.EventArgs.Data)
				.DistinctUntilChanged()
				.Where(m => m != null)
				.Do(m => _logger.Error(m))
				.Subscribe();
		}

		public int Init()
		{
			return Execute("init");
		}

		public int Config(string arguments)
		{
			return Execute(String.Format("config {0}", arguments));
		}

		public int AddFile(string file)
		{
			return Execute(String.Format("add -f \"{0}\"", file));
		}

		public int AddDirectory(string file)
		{
			return Execute(String.Format("add --all -f \"{0}\"", file));
		}

		public int RemoveFile(string file)
		{
			return Execute(String.Format("rm -f \"{0}\"", file));
		}

		public int RemoveDirectory(string directory)
		{
			return Execute(String.Format("rm -f -r \"{0}\"", directory));
		}

		public int Commit(string comment)
		{
			return Execute(String.Format("commit -m \"{0}\"", comment));
		}

		public int AddRemote(string remoteUrl)
		{
			return Execute(String.Format("remote add origin \"{0}\"", remoteUrl));
		}

		public int CheckoutBranch(string branch)
		{
			return Execute(String.Format("checkout -b \"{0}\"", branch));
		}

		public int Push()
		{
			return Execute("push origin");
		}

		public int PushBranch(string branch)
		{
			return Execute(String.Format("push origin --tags \"{0}\"", branch));
		}

		public int PullBranch(string branch)
		{
			return Execute(String.Format("pull --rebase origin \"{0}\"", branch));
		}
	}
}