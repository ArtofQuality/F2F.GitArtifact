using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using F2F.Testing.Xunit.FakeItEasy;
using FluentAssertions;
using Xunit;
using Xunit.Extensions;

namespace F2F.FileFilter.UnitTests
{
	public class ExcludeFileFilter_Test : AutoMockFeature
	{
		[Fact(Skip = "")]
		public void Dummy()
		{
		}

		[Theory]
		[InlineData("", "a.txt")]
		[InlineData("b.txt", "a.txt")]
		[InlineData("abc/a.txt", "a.txt")]
		[InlineData("xxx/*.txt", "abc/b.txt")]
		public void IsAllowed_ShouldReturnTrue(string filter, string file)
		{
			// Arrange
			var sut = new ExcludeFileFilter(filter);

			// Act && Assert
			sut.IsAllowed(file).Should().BeTrue();
		}

		[Theory]
		[InlineData("*.txt", "a.txt")]
		[InlineData("*.txt", "abc/b.txt")]
		public void IsAllowed_ShouldReturnFalse(string filter, string file)
		{
			// Arrange
			var sut = new ExcludeFileFilter(filter);

			// Act && Assert
			sut.IsAllowed(file).Should().BeFalse();
		}
	}
}