using System;
using System.Collections.Generic;

namespace Medelinked.Core.Response
{
	public class HealthSnapshotData
	{
	    public List<HealthSnapshotModel> HealthSnapshots { get; set; }
		public string Message { get; set; }
	}
}