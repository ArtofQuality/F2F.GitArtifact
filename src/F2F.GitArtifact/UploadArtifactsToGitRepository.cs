using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using F2F.FileFilter;

namespace F2F.GitArtifact
{
	public class UploadArtifactsToGitRepository
	{
		private readonly IFileFilter _fileFilter;
		private readonly string _directory;
		private readonly string _remoteUrl;
		private readonly string _branch;

		public UploadArtifactsToGitRepository(string directory, IFileFilter fileFilter, string remoteUrl, string branch)
		{
			_fileFilter = fileFilter;
			_directory = directory;
			_branch = branch;
			_remoteUrl = remoteUrl;
		}

		public void Upload()
		{
			using (var tmp = new ProvideTemporaryDirectory())
			{
				var git = new Git(tmp.Directory);

				new InitializeGitRepository(git).Initialize();

				git.AddRemote(_remoteUrl);
				git.CheckoutBranch(_branch);
				var hasBranch = git.PullBranch(_branch) == 0;
				if (hasBranch) git.RemoveDirectory(git.Directory);

				var fileProvider = new ReadFilteredFilesRecursively(_directory, _fileFilter);
				new AddFilesToGitRepository(git, fileProvider).AddFiles();

				git.Commit("Initial Commit");
				git.PushBranch(_branch);
			}
		}
	}
}