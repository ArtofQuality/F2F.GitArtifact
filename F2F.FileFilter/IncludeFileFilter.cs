using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace F2F.FileFilter
{
	public class IncludeFileFilter : IFileFilter
	{
		private readonly string[] _includes;

		public IncludeFileFilter()
			: this(new string[0])
		{
		}

		public IncludeFileFilter(string filter)
			: this(filter.Split(','))
		{
		}

		public IncludeFileFilter(IEnumerable<string> includes)
		{
			_includes = includes
				.Where(i => !String.IsNullOrEmpty(i))
				.Select(i => String.Format("^{0}", Regex.Escape(i).Replace("\\*", ".*")))
				.ToArray();
		}

		public bool IsAllowed(string path)
		{
			return _includes.Length > 0 ? _includes.Any(f => Regex.IsMatch(path, f)) : true;
		}
	}
}