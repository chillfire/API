using System;
using System.Collections.Generic;

namespace Medelinked.Core.Response
{
	public class HealthProviders
	{
		public HealthProviders ()
		{
		}

		public string Message { get; set; }
		public List<HealthProvider> Providers { get; set; }
	}
}

