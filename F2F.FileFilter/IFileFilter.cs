using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace F2F.FileFilter
{
	public interface IFileFilter
	{
		bool IsAllowed(string path);
	}
}