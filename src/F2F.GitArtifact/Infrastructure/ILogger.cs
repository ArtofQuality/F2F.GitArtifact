﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace F2F.GitArtifact
{
	public interface ILogger
	{
		void Info(string format, params object[] args);

		void Error(string format, params object[] args);
	}
}