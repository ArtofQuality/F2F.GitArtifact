using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace F2F.FileFilter
{
	public class CompositeFilter : IFileFilter
	{
		private readonly IFileFilter[] _filters;

		public CompositeFilter(params IFileFilter[] filters)
		{
			_filters = filters;
		}

		public bool IsAllowed(string path)
		{
			return _filters.All(f => f.IsAllowed(path));
		}
	}
}