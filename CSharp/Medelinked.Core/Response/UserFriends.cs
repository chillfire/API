using System;
using System.Collections.Generic;


namespace Medelinked.Core.Response
{
    public class UserFriends
    {
        public List<UserFriend> Friends { get; set; }
        public string Message { get; set; }
    }
}