using System;

namespace Medelinked.Core.Request
{
	public class ChangePasswordModel
	{
		public string CurrentPassword { get; set; }
		public string NewPassword { get; set; }
		public string ConfirmNewPassword { get; set; }
		public string Email { get; set; }
		public string MedelinkedID { get; set; }
		public string ProviderID { get; set; }
	}
}

