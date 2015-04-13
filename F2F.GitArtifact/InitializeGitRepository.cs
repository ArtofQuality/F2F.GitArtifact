using System;
using System.Collections.Generic;
using System.Linq;

namespace F2F.GitArtifact
{
	public class InitializeGitRepository
	{
		private readonly Git _git;
		private readonly string _userName;
		private readonly string _userEmail;

		public InitializeGitRepository(Git git)
			: this(git, null, null)
		{
		}

		public InitializeGitRepository(Git git, string userName, string userEmail)
		{
			_git = git;
			_userName = userName;
			_userEmail = userEmail;
		}

		public void Initialize()
		{
			_git.Init();
			_git.Config("core.autocrlf false");

			if (!String.IsNullOrEmpty(_userName)) _git.Config(String.Format("user.name \"{0}\"", _userName));
			if (!String.IsNullOrEmpty(_userEmail)) _git.Config(String.Format("user.email \"{0}\"", _userEmail));
		}
	}
}