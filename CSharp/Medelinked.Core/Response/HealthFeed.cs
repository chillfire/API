using System;
using System.Collections.Generic;

namespace Medelinked.Core.Response
{
	
	public class HealthFeed
	{
		public List<FeedItemModel> FeedItems { get; set; }
		public string Message { get; set; }
	}

    public class FeedItemModel
    {
        public string Title { get; set; }
        public DateTime ActivityDate { get; set; }
        public string Summary { get; set; }
        public string ActivityType { get; set; }
        public string RecordType { get; set; }
        public string ImageURL { get; set; }
        public string ExternalLinkURL { get; set; }
        public string RecordID { get; set; }
        public string AssociatedProviderID { get; set; }
		public List<FeedItemProperty> FeedItemProperties { get; set; }
		public Dictionary<string, string> ItemProperties { get; set; }
    }

	public class FeedItemProperty
	{
		public string ItemKey { get; set; }
		public string ItemValue { get; set; }
	}

}