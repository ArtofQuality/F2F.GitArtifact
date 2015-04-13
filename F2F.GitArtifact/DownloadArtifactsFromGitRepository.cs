using System;
using System.Collections.Generic;
using System.Linq;
using F2F.FileFilter;

namespace F2F.GitArtifact
{
	public class DownloadArtifactsFromGitRepository
	{
		private readonly IFileFilter _fileFilter;
		private readonly string _directory;
		private readonly string _remoteUrl;
		private readonly string _branch;

		public DownloadArtifactsFromGitRepository(string directory, IFileFilter fileFilter, string remoteUrl, string branch)
		{
			_fileFilter = fileFilter;
			_directory = directory;
			_branch = branch;
			_remoteUrl = remoteUrl;
		}

		public void Download()
		{
			using (var tmp = new ProvideTemporaryDirectory())
			{
				var git = new Git(tmp.Directory);

				new InitializeGitRepository(git).Initialize();

				git.AddRemote(_remoteUrl);
				git.PullBranch(_branch);

				var fileProvider = new ReadFilteredFilesRecursively(git.Directory, new CompositeFilter(new ExcludeFileFilter(".git"), _fileFilter));
				new MoveFilesToDirectory(fileProvider, _directory).MoveFiles();
			}
		}
	}
}