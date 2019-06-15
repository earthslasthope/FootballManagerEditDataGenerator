using System;
using System.Collections.Generic;

namespace FootballManagerEditDataGenerator.DataScraper
{
    public class WikipediaInfobox : Dictionary<string, WikipediaInfoboxItem>, IDictionary<string, WikipediaInfoboxItem>
    {
        public void Add(WikipediaInfoboxItem item)
        {
            if (ContainsKey(item.Property))
            {
                throw new ArgumentException("Item already exists");
            }

            Add(item.Property, item);
        }
    }

    public class WikipediaInfoboxItem
    {
        public string Property { get; set; }
        public IEnumerable<WikipediaInfoBoxItemData> Values { get; set; }
    }

    public class WikipediaInfoBoxItemData
    {
        public string Text { get; set; }
        public string Link { get; set; }
        public int? YearFrom { get; set; }
        public int? YearTo { get; set; }
    }
}