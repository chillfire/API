using System;
using System.Collections.Generic;

namespace Medelinked.Core.Request
{
	public class SharingRequest
	{
		public SharingRequest ()
		{
		}

		public string EmailAddress { get; set; }
		public string Forename { get; set; }
		public string Surname { get; set; }
		public List<Guid> Records { get; set; }
	}
}

