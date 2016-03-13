using System;

namespace Medelinked.Core.Request
{
	public class LoginModel
	{
		public string Username { get; set; }
		public string Password { get; set; }
		public string ProviderKey { get; set; }
		public string MedelinkedID { get; set; }
	}
}

