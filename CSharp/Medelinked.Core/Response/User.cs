using System;

namespace Medelinked.Core.Response
{
	public class User
	{
		public User ()
		{
		}

		public string ID { get; set; }
		public string Name { get; set; }
		public string DateOfBirth { get; set; }
		public string NHSNumber { get; set; }
		public string RegisteredGP { get; set; }
		public string Nationality { get; set; }
		public string Photo { get; set; }
		public string GPAddress { get; set; }
		public string GPPhone { get; set; }
		public string Gender { get; set; }
		public string BloodGroup { get; set; }
		public string ZaptagID { get; set; }
		public string OptOutOfEmergencyRecord { get; set; }
		public string Message { get; set; }
		public string LocalProfileImage { get; set; }
		public string TwoFactorAuthenticationEnabled { get; set; }
	}
}

