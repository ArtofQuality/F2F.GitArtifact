using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace F2F.GitArtifact
{
	public class ProvideTemporaryDirectory : IDisposable
	{
		private readonly string _directory;

		public ProvideTemporaryDirectory()
			: this(Path.GetTempPath())
		{
		}

		public ProvideTemporaryDirectory(string rootDirectory)
		{
			_directory = Path.Combine(rootDirectory, Guid.NewGuid().ToString());

			System.IO.Directory.CreateDirectory(_directory);
		}

		public string Directory
		{
			get { return _directory; }
		}

		public void Dispose()
		{
			if (System.IO.Directory.Exists(_directory))
			{
				System.IO.Directory.Delete(_directory, recursive: true);
			}
		}
	}
}