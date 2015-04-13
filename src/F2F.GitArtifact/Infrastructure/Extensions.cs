using System;
using System.Collections.Generic;
using System.Linq;

namespace F2F.GitArtifact
{
	public static class Extensions
	{
		private const int NO_ERROR = 0;

		public static bool IsSuccessful(this int self)
		{
			return self == NO_ERROR;
		}

		public static void MustBeSuccessful(this int self, string format, params object[] args)
		{
			if (!self.IsSuccessful())
			{
				throw new InvalidOperationException(String.Format(format, args));
			}
		}

		public static void MustBeSuccessful(this bool self, string format, params object[] args)
		{
			if (!self)
			{
				throw new InvalidOperationException(String.Format(format, args));
			}
		}
	}
}