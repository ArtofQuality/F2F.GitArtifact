using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using F2F.FileFilter;

namespace F2F.GitArtifact
{
	public class GetAllFilesFromGitRepository
	{
		private readonly IFileFilter _fileFilter;

		public GetAllFilesFromGitRepository(IFileFilter fileFilter)
		{
			_fileFilter = fileFilter;
		}

		public IFileProvider GetFileProvider(string directory)
		{
			return new ReadFilteredFilesRecursively(directory, new CompositeFilter(new ExcludeFileFilter(".git"), _fileFilter));
		}
	}
}