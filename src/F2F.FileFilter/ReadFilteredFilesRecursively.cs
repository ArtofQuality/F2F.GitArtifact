using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace F2F.FileFilter
{
	public class ReadFilteredFilesRecursively : IFileProvider
	{
		private const string SEARCH_ALL = "*";

		private readonly string _path;
		private readonly IFileFilter _filter;

		public ReadFilteredFilesRecursively(string path, IFileFilter filter)
		{
			_path = path;
			_filter = filter;
		}

		public IEnumerable<FileInformation> GetFiles()
		{
			return GetFiles(_path);
		}

		private IEnumerable<FileInformation> GetFiles(string absoluteRootPath)
		{
			var directories = Directory
				.EnumerateDirectories(absoluteRootPath, SEARCH_ALL, SearchOption.TopDirectoryOnly)
				.Select(f => new FileInformation()
					{
						AbsolutePath = Path.GetFullPath(f),
						RelativePath = GetRelativePath(_path, f)
					})
				.Where(f => _filter.IsAllowed(f.RelativePath));

			var files = Directory
				.EnumerateFiles(absoluteRootPath, SEARCH_ALL, SearchOption.TopDirectoryOnly)
				.Select(f => new FileInformation()
					{
						AbsolutePath = Path.GetFullPath(f),
						RelativePath = GetRelativePath(_path, f)
					})
				.Where(f => _filter.IsAllowed(f.RelativePath));

			return directories
				.SelectMany(f => Enumerable.Concat(new[] { f }, GetFiles(f.AbsolutePath)))
				.Concat(files);
		}

		public static string GetRelativePath(string rootDirectory, string absolutePath)
		{
			var m = Regex.Match(absolutePath, String.Format("^{0}\\\\?(.+)$", Regex.Escape(rootDirectory)));
			if (m.Success)
			{
				return m.Groups[1].Value;
			}
			else
			{
				throw new ArgumentException(String.Format("'{0}' does not contain '{1}'", absolutePath, rootDirectory));
			}
		}
	}
}