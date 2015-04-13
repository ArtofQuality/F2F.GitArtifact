using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using F2F.FileFilter;

namespace F2F.GitArtifact
{
	public class UploadArtifactsToGitRepository
	{
		private const string COMMIT_MSG = "Initial Commit";

		private readonly ILogger _logger;
		private readonly IFileFilter _fileFilter;
		private readonly string _directory;
		private readonly string _remoteUrl;
		private readonly string _branch;

		public UploadArtifactsToGitRepository(ILogger logger, string directory, IFileFilter fileFilter, string remoteUrl, string branch)
		{
			_logger = logger;
			_fileFilter = fileFilter;
			_directory = directory;
			_branch = branch;
			_remoteUrl = remoteUrl;
		}

		public void Upload()
		{
			using (var tmp = new ProvideTemporaryDirectory())
			{
				var git = new Git(_logger, tmp.Directory);

				new InitializeGitRepository(git)
					.Initialize();

				git.AddRemote(_remoteUrl)
					.MustBeSuccessful("could not add remote url '{0}'", _remoteUrl);
				git.CheckoutBranch(_branch)
					.MustBeSuccessful("could not checkout remote branch '{0}'", _branch);

				var hasBranch = git.PullBranch(_branch).IsSuccessful();
				if (hasBranch) git.RemoveDirectory(git.Directory)
					.MustBeSuccessful("could not remove files from directory '{0}'", git.Directory);

				var fileProvider = new ReadFilteredFilesRecursively(_directory, _fileFilter);

				new CopyAndAddFilesToGitRepository(_logger, git, fileProvider)
					.CopyFiles()
					.MustBeSuccessful("could not copy files from '{0}' to '{1}'", git.Directory, _directory);

				git.Commit(COMMIT_MSG)
					.MustBeSuccessful("could not commit to git repository");
				git.PushBranch(_branch)
					.MustBeSuccessful("could not push branch '{0}'", _branch);
			}
		}
	}
}