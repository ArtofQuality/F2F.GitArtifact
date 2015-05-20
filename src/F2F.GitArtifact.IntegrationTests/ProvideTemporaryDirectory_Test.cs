using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using F2F.Sandbox;
using F2F.Testing.Xunit;
using F2F.Testing.Xunit.FakeItEasy;
using F2F.Testing.Xunit.Sandbox;
using FluentAssertions;
using Ploeh.AutoFixture;
using Xunit;

namespace F2F.GitArtifact.IntegrationTests
{
	public class ProvideTemporaryDirectory_Test : TestFixture
	{
		public ProvideTemporaryDirectory_Test()
		{
			Register(new AutoMockFeature());
			Register(new FileSandboxFeature(new ResourceFileLocator(GetType())));
		}

		[Fact]
		public void Dispose_WithInitializedGitRepository_ShouldNotThrow()
		{
			var logger = this.Fixture().Create<ILogger>();
			var sut = new ProvideTemporaryDirectory(this.Sandbox().Directory);
			var git = new Git(logger, sut.Directory);
			var init = new InitializeGitRepository(git);

			init.Initialize();

			Action a = () => sut.Dispose();

			a.ShouldNotThrow();
		}
	}
}