using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace F2F.FileFilter
{
	public class FilterParser
	{
		public IFileFilter Parse(string filter)
		{
			if (!String.IsNullOrEmpty(filter))
			{
				var parts = filter.Split(',');
				var include = parts.Where(f => f.StartsWith("+")).Select(f => f.Substring(1));
				include = include.Concat(parts.Where(f => !f.StartsWith("+") && !f.StartsWith("-")));
				var exclude = parts.Where(f => f.StartsWith("-")).Select(f => f.Substring(1));

				return new CompositeFilter(new IncludeFileFilter(include), new ExcludeFileFilter(exclude));
			}

			return new IncludeFileFilter();
		}
	}
}