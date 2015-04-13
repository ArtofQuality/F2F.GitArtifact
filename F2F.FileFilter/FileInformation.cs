using System;
using System.Collections.Generic;
using System.Linq;

namespace F2F.FileFilter
{
	public class FileInformation
	{
		public string AbsolutePath { get; set; }

		public string RelativePath { get; set; }

		public override string ToString()
		{
			return AbsolutePath;
		}
	}
}