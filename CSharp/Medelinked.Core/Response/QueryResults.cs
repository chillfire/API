using System;
using System.Collections.Generic;

namespace Medelinked.Core.Response
{
	using Request;

	public class QueryResults
	{
		public string Status { get; set; }
		public List<QueryKeyValuePair> Results { get; set; }
		public Dictionary<string, string> ResultsDictionary { get; set; }
	}
		
}
