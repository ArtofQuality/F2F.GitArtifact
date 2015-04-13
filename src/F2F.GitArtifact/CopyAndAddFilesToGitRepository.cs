using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using F2F.FileFilter;

namespace F2F.GitArtifact
{
	public class CopyAndAddFilesToGitRepository
	{
		private readonly ILogger _logger;
		private readonly Git _git;
		private readonly IFileProvider _fileProvider;

		public CopyAndAddFilesToGitRepository(ILogger logger, Git git, IFileProvider fileProvider)
		{
			_logger = logger;
			_git = git;
			_fileProvider = fileProvider;
		}

		public bool CopyFiles()
		{
			var isSuccessful = true;

			try
			{
				foreach (var file in _fileProvider.GetFiles())
				{
					var sourcePath = file.AbsolutePath;
					var targetPath = Path.Combine(_git.Directory, file.RelativePath);

					if (File.Exists(sourcePath))
					{
						var targetDirectory = Path.GetDirectoryName(targetPath);
						if (!Directory.Exists(targetDirectory)) Directory.CreateDirectory(targetDirectory);

						File.Copy(sourcePath, targetPath, overwrite: true);

						_git.AddFile(targetPath);
					}
					else
					{
						Directory.CreateDirectory(targetPath);

						// TODO add directory to git as well?
					}
				}
			}
			catch (Exception e)
			{
				_logger.Error("Could not copy files to '{0}': {1}", _git.Directory, e.Message);

				isSuccessful = false;
			}

			return isSuccessful;
		}
	}
}