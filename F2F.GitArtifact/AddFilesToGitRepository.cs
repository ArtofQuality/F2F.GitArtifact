using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using F2F.FileFilter;

namespace F2F.GitArtifact
{
	public class AddFilesToGitRepository
	{
		private readonly Git _git;
		private readonly IFileProvider _fileProvider;

		public AddFilesToGitRepository(Git git, IFileProvider fileProvider)
		{
			_git = git;
			_fileProvider = fileProvider;
		}

		public void AddFiles()
		{
			foreach (var file in _fileProvider.GetFiles())
			{
				var sourcePath = file.AbsolutePath;
				var targetPath = Path.Combine(_git.Directory, file.RelativePath);

				File.Copy(sourcePath, targetPath, overwrite: true);

				_git.AddFile(targetPath);
			}
		}
	}
}