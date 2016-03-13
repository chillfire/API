using System;
using System.Collections.Generic;

namespace Medelinked.Core.Response
{
	public class HealthData
	{
		public HealthData ()
		{
		}

		public string Message { get; set; }
		public List<Record> Records { get; set; }
	}
}

