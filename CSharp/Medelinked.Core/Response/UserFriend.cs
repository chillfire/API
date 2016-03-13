using System;
using System.Collections.Generic;

namespace Medelinked.Core.Response
{
    public class UserFriend
    {
		public string ID { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Photo { get; set; }
        public bool IsConnected { get; set; }
        public DateTime? DateConnectionRequested { get; set; }
        public DateTime? DateConnected { get; set; }
		public bool IsFriendRequest { get; set; }
    }
}