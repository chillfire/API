using System;
using System.Collections.Generic;

namespace Medelinked.Core.Request
{
	public class QueryRequest
	{
		public string Username { get; set; }
		public string QueryName { get; set; }
		public List<QueryKeyValuePair> QueryParameters { get; set; }	
	}

	public class QueryKeyValuePair
	{
		public string Key { get; set; }
		public string Value { get; set; }
	}
}