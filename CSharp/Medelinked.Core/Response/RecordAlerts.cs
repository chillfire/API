using System;
using System.Collections.Generic;

namespace Medelinked.Core.Response
{
	public class RecordAlerts
	{
		public string Message { get; set; }
		public List<RecordAlert> Alerts { get; set; }
	}
}