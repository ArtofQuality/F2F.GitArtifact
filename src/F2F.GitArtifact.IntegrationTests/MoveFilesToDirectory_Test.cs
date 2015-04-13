using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using F2F.FileFilter;
using F2F.Testing.Sandbox;
using F2F.Testing.Xunit;
using F2F.Testing.Xunit.FakeItEasy;
using F2F.Testing.Xunit.Sandbox;
using FakeItEasy;
using FluentAssertions;
using Ploeh.AutoFixture;
using Xunit;

namespace F2F.GitArtifact.IntegrationTests
{
	public class MoveFilesToDirectory_Test : TestFixture
	{
		public MoveFilesToDirectory_Test()
		{
			Register(new AutoMockFeature());
			Register(new FileSandboxFeature(new ResourceFileLocator(GetType())));
		}

		[Fact]
		public void MoveFiles_ShouldMoveAllFiles()
		{
			var logger = this.Fixture().Create<ILogger>();
			var targetDirectory = this.Sandbox().CreateDirectory("out");
			var sut = new MoveFilesToDirectory(logger, targetDirectory);

			var files = this.Fixture().CreateMany<string>().Select(i => String.Format("in/{0}", i)).ToList();
			var sourceFiles = files.Select(f => new FileInformation() { AbsolutePath = this.Sandbox().CreateFile(f), RelativePath = f });
			var targetFiles = files.Select(f => Path.Combine(targetDirectory, f));

			var fileProvider = this.Fixture().Create<IFileProvider>();
			A.CallTo(() => fileProvider.GetFiles()).Returns(sourceFiles);

			sut.MoveFiles(fileProvider).Should().BeTrue();
			targetFiles.Select(f => File.Exists(f)).Should().BeSubsetOf(new[] { true });
		}
	}
}