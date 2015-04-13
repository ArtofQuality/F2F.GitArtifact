using System;
using System.Collections.Generic;
using System.Linq;
using F2F.FileFilter;

namespace F2F.GitArtifact
{
	public class DownloadArtifactsFromGitRepository
	{
		private readonly ILogger _logger;
		private readonly IFileFilter _fileFilter;
		private readonly string _directory;
		private readonly string _remoteUrl;
		private readonly string _branch;

		public DownloadArtifactsFromGitRepository(ILogger logger, string directory, IFileFilter fileFilter, string remoteUrl, string branch)
		{
			_logger = logger;
			_fileFilter = fileFilter;
			_directory = directory;
			_branch = branch;
			_remoteUrl = remoteUrl;
		}

		public void Download()
		{
			using (var tmp = new ProvideTemporaryDirectory())
			{
				var git = new Git(_logger, tmp.Directory);

				new InitializeGitRepository(git)
					.Initialize();

				git.AddRemote(_remoteUrl)
					.MustBeSuccessful("could not add remote url '{0}'", _remoteUrl);
				git.PullBranch(_branch)
					.MustBeSuccessful("could not pull remote branch '{0}'", _branch);

				var fileProvider = new GetAllFilesFromGitRepository(_fileFilter)
					.GetFileProvider(git.Directory);

				new MoveFilesToDirectory(_logger, _directory)
					.MoveFiles(fileProvider)
					.MustBeSuccessful("could not move files from '{0}' to '{1}'", git.Directory, _directory);
			}
		}
	}
}