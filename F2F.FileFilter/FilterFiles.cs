using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace F2F.FileFilter
{
	public class FilterFiles : IFileProvider
	{
		private readonly IFileProvider _fileProvider;

		private readonly IFileFilter _filter;

		public FilterFiles(IFileProvider fileProvider, IFileFilter filter)
		{
			_fileProvider = fileProvider;
			_filter = filter;
		}

		public IEnumerable<FileInformation> GetFiles()
		{
			return _fileProvider.GetFiles().Where(f => _filter.IsAllowed(f.RelativePath));
		}
	}
}