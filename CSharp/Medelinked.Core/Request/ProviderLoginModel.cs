using System;

namespace Medelinked.Core.Request
{
	public class ProviderLoginModel
    {
        public string ProviderID { get; set; }
        public string Passcode { get; set; }
        public string SessionID { get; set; }
        public string InstanceUrl { get; set; }
    }
}