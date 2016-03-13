using System;
using System.Collections.Generic;
using System.Linq;

namespace Medelinked.Core.Response
{
    public class UserMessages
    {
        public string Message { get; set; }
        public List<UserMessage> Messages { get; set; }
    }
}