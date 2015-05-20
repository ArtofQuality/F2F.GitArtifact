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
		private readonly string _targetDirectory;
		private readonly string _tempDirectory;
		private readonly string _repository;
		private readonly string _branch;

		public UploadArtifactsToGitRepository(
			ILogger logger,
			IFileFilter fileFilter,
			string targetDirectory,
			string tempDirectory,
			string repository,
			string branch)
		{
			if (logger == null)
				throw new ArgumentNullException("logger", "logger is null.");
			if (fileFilter == null)
				throw new ArgumentNullException("fileFilter", "fileFilter is null.");
			if (string.IsNullOrEmpty(targetDirectory))
				throw new ArgumentException("targetDirectory is null or empty.", "targetDirectory");
			if (string.IsNullOrEmpty(repository))
				throw new ArgumentException("repository is null or empty.", "repository");
			if (string.IsNullOrEmpty(branch))
				throw new ArgumentException("branch is null or empty.", "branch");

			_logger = logger;
			_fileFilter = fileFilter;
			_targetDirectory = targetDirectory;
			_tempDirectory = tempDirectory;
			_branch = branch;
			_repository = repository;
		}

		public void Upload()
		{
			using (var tmp = new ProvideTemporaryDirectory(_tempDirectory))
			{
				var git = new Git(_logger, tmp.Directory);

				new InitializeGitRepository(git)
					.Initialize();

				git.AddRemote(_repository)
					.MustBeSuccessful("could not add remote url '{0}'", _repository);
				git.CheckoutBranch(_branch)
					.MustBeSuccessful("could not checkout remote branch '{0}'", _branch);

				var hasBranch = git.PullBranch(_branch).IsSuccessful();
				if (hasBranch) git.RemoveDirectory(git.Directory)
					.MustBeSuccessful("could not remove files from directory '{0}'", git.Directory);

				var fileProvider = new ReadFilteredFilesRecursively(_targetDirectory, _fileFilter);

				new CopyAndAddFilesToGitRepository(_logger, git, fileProvider)
					.CopyFiles()
					.MustBeSuccessful("could not copy files from '{0}' to '{1}'", git.Directory, _targetDirectory);

				git.Commit(COMMIT_MSG)
					.MustBeSuccessful("could not commit to git repository");
				git.PushBranch(_branch)
					.MustBeSuccessful("could not push branch '{0}'", _branch);
			}
		}
	}
}