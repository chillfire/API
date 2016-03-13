using System;

namespace Medelinked.Core.Response
{
	public class RecordAlert
	{
		 public string RecordCategory { get; set; }
		 public decimal UpperLimit { get; set; }
		 public decimal LowerLimit { get; set; }
		 public string ProviderEmailToAlert { get; set; }
		 public string MobileNumberToAlert { get; set; }
		 public string Username { get; set; }
	}
}