using System;
using System.Collections.Generic;

namespace FootballManagerEditDataGenerator.DataScraper
{
    public class WikipediaInfobox<TInfoboxData> : Dictionary<string, WikipediaInfoboxItem<TInfoboxData>>, IDictionary<string, WikipediaInfoboxItem<TInfoboxData>>
        where TInfoboxData : WikipediaInfoBoxItemData
    {
        public void Add(WikipediaInfoboxItem<TInfoboxData> item)
        {
            if (ContainsKey(item.Property))
            {
                throw new ArgumentException("Item already exists");
            }

            Add(item.Property, item);
        }
    }

    public class WikipediaInfoboxItem<TInfoboxData>
        where TInfoboxData : WikipediaInfoBoxItemData
    {
        public string Property { get; set; }
        public IEnumerable<WikipediaInfoBoxItemData> Data { get; set; }
    }

    public class WikipediaInfoBoxItemData
    {
        public string Text { get; set; }
        public string Link { get; set; }
        public int? YearFrom { get; set; }
        public int? YearTo { get; set; }
    }
}