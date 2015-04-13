using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using F2F.FileFilter;

namespace F2F.GitArtifact
{
	public class CopyFilesToDirectory
	{
		private readonly IFileProvider _fileProvider;
		private readonly string _directory;

		public CopyFilesToDirectory(IFileProvider fileProvider, string directory)
		{
			_fileProvider = fileProvider;
			_directory = directory;
		}

		public void CopyFiles()
		{
			if (!Directory.Exists(_directory)) Directory.CreateDirectory(_directory);

			foreach (var file in _fileProvider.GetFiles())
			{
				var sourcePath = file.AbsolutePath;
				var targetPath = Path.Combine(_directory, file.RelativePath);

				File.Copy(sourcePath, targetPath, overwrite: true);
			}
		}
	}
}