using System;

namespace Medelinked.Core.Request
{
	
	public class ProviderSearchModel
	{
	        public string Filter { get; set; }
	        public string ProviderType { get; set; }
	        public string Country { get; set; }

	        // 0 = All, 1 = Zaptag, 2 = NHS, 3 = Bupa
	        public int ServiceType { get; set; }
	}
	
}