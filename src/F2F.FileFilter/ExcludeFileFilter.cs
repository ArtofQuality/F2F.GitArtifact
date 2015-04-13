using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace F2F.FileFilter
{
	public class ExcludeFileFilter : IFileFilter
	{
		private readonly string[] _excludes;

		public ExcludeFileFilter(string filter)
			: this(filter.Split(','))
		{
		}

		public ExcludeFileFilter(IEnumerable<string> excludes)
		{
			_excludes = excludes
				.Where(i => !String.IsNullOrEmpty(i))
				.Select(i => String.Format("^{0}", Regex.Escape(i).Replace("\\*", ".*")))
				.ToArray();
		}

		public bool IsAllowed(string file)
		{
			return _excludes.Length > 0 ? !_excludes.Any(f => Regex.IsMatch(file, f)) : true;
		}
	}
}