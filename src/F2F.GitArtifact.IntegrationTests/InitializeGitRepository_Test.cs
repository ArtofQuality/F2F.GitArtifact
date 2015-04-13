using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using F2F.Testing.Xunit;
using FluentAssertions;
using Xunit;

namespace F2F.GitArtifact.IntegrationTests
{
	public class InitializeGitRepository_Test : FileSandboxFeature
	{
		[Fact]
		public void Initialize_ShouldCreateGitFolder()
		{
			var git = new Git(Sandbox.Directory);
			var sut = new InitializeGitRepository(git);

			sut.Initialize();

			Directory.Exists(Path.Combine(Sandbox.Directory, ".git")).Should().BeTrue();
		}
	}
}