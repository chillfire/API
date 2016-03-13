using System;
using System.Collections.Generic;

namespace Medelinked.Core.Request
{
	public class CommandRequest
	{
		public string CommandName { get; set; }
		public List<QueryKeyValuePair> CommandParameters { get; set; }	
	}
}

