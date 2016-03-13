using System;

namespace Medelinked.Core.Response
{
	public class HealthFile
	{
		public HealthFile ()
		{
		}

		public string Name { get; set; }
		public string Url { get; set; }
		public string Base64FileContents { get;  set; }
		public bool IsNew { get; set; }
	}
}

