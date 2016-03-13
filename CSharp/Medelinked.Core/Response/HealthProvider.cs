using System;

namespace Medelinked.Core.Response
{
	public class HealthProvider
	{
		public HealthProvider ()
		{
			this.IsSearchResult = false;
			this.IsConnected = false;
		}

		public string ID { get; set; }
		public string Name { get; set; }
		public string ProviderType { get; set; }
		public string AddressLine1 { get; set; }
		public string AddressLine2 { get; set; }
		public string Country { get; set; }
		public string Postcode { get; set; }
		public string PhoneNumber { get; set; }
		public string Email { get; set; }
		public string Website { get; set; }
		public string MainContact { get; set; }
		public bool IsSearchResult { get; set; }
		public bool IsConnected { get; set; }

		public string CompleteAddress(bool NoLineBreaks = false)
		{
			//Build the address of the current item
			string addr = this.AddressLine1;

			if (!String.IsNullOrEmpty(this.AddressLine2))
				addr += ", <br>" + this.AddressLine2;

			if (!String.IsNullOrEmpty(this.Country))
				addr += ", <br>" + this.Country;

			if (!String.IsNullOrEmpty(this.Postcode))
				addr += ", " + this.Postcode;

			if (NoLineBreaks)
				addr = addr.Replace ("<br>", String.Empty);

			return addr;
		}
	}
}

