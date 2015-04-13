using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using F2F.Testing.Sandbox;
using F2F.Testing.Xunit;
using F2F.Testing.Xunit.FakeItEasy;
using F2F.Testing.Xunit.Sandbox;
using FluentAssertions;
using Ploeh.AutoFixture;
using Xunit;

namespace F2F.GitArtifact.IntegrationTests
{
	public class InitializeGitRepository_Test : TestFixture
	{
		public InitializeGitRepository_Test()
		{
			Register(new AutoMockFeature());
			Register(new FileSandboxFeature(new ResourceFileLocator(GetType())));
		}

		[Fact]
		public void Initialize_ShouldCreateGitFolder()
		{
			var logger = this.Fixture().Create<ILogger>();
			var git = new Git(logger, this.Sandbox().Directory);
			var sut = new InitializeGitRepository(git);

			sut.Initialize();

			Directory.Exists(Path.Combine(this.Sandbox().Directory, ".git")).Should().BeTrue();
		}
	}
}