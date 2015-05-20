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
		private readonly string _targetDirectory;
		private readonly string _tempDirectory;
		private readonly string _repository;
		private readonly string _branch;

		public DownloadArtifactsFromGitRepository(
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
			_repository = repository;
			_branch = branch;
		}

		public void Download()
		{
			using (var tmp = new ProvideTemporaryDirectory(_tempDirectory))
			{
				var git = new Git(_logger, tmp.Directory);

				new InitializeGitRepository(git)
					.Initialize();

				git.AddRemote(_repository)
					.MustBeSuccessful("could not add remote url '{0}'", _repository);
				git.PullBranch(_branch)
					.MustBeSuccessful("could not pull remote branch '{0}'", _branch);

				var fileProvider = new GetAllFilesFromGitRepository(_fileFilter)
					.GetFileProvider(git.Directory);

				new MoveFilesToDirectory(_logger, _targetDirectory)
					.MoveFiles(fileProvider)
					.MustBeSuccessful("could not move files from '{0}' to '{1}'", git.Directory, _targetDirectory);
			}
		}
	}
}