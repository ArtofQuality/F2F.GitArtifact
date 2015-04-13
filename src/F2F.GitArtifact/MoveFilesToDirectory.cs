using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using F2F.FileFilter;

namespace F2F.GitArtifact
{
	public class MoveFilesToDirectory
	{
		private readonly ILogger _logger;
		private readonly string _directory;

		public MoveFilesToDirectory(ILogger logger, string directory)
		{
			_logger = logger;
			_directory = directory;
		}

		public bool MoveFiles(IFileProvider fileProvider)
		{
			var isSuccessful = true;

			try
			{
				if (!Directory.Exists(_directory)) Directory.CreateDirectory(_directory);

				foreach (var file in fileProvider.GetFiles())
				{
					var sourcePath = file.AbsolutePath;
					var targetPath = Path.Combine(_directory, file.RelativePath);

					if (File.Exists(sourcePath))
					{
						var targetDirectory = Path.GetDirectoryName(targetPath);
						if (!Directory.Exists(targetDirectory)) Directory.CreateDirectory(targetDirectory);

						if (File.Exists(targetPath)) File.Delete(targetPath);

						File.Move(sourcePath, targetPath);
					}
					else
					{
						Directory.CreateDirectory(targetPath);
					}
				}

				isSuccessful = true;
			}
			catch (Exception e)
			{
				_logger.Error("Could not move files to '{0}': {1}", _directory, e.Message);

				isSuccessful = false;
			}

			return isSuccessful;
		}
	}
}