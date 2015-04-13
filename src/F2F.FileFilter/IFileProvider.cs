using System;
using System.Collections.Generic;
using System.Linq;

namespace F2F.FileFilter
{
	public interface IFileProvider
	{
		IEnumerable<FileInformation> GetFiles();
	}
}