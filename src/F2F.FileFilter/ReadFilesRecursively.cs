using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace F2F.FileFilter
{
	public class ReadFilesRecursively : IFileProvider
	{
		private const string SEARCH_ALL = "*";

		private readonly string _path;

		public ReadFilesRecursively(string path)
		{
			_path = path;
		}

		public IEnumerable<FileInformation> GetFiles()
		{
			var files = Directory.EnumerateFiles(_path, SEARCH_ALL, SearchOption.AllDirectories);
			var directories = Directory.EnumerateDirectories(_path, SEARCH_ALL, SearchOption.AllDirectories);

			return directories
				.Concat(files)
				.Select(f => Path.GetFullPath(f))
				.Select(f => new FileInformation()
					{
						AbsolutePath = f,
						RelativePath = GetRelativePath(_path, f)
					});
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