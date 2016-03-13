using System;
using System.Collections.Generic;
using System.Linq;

namespace Medelinked.Core.Response
{
    public class UserMessage
    {
        public string From { get; set; }
        public string To { get; set; }
        public string Subject { get; set; }
        public string Timestamp { get; set; }
        public string MessageText { get; set; }
        public bool Sent { get; set; }
		public bool Read { get; set; }
        public string Username { get; set; }
    }
}