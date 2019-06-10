using System;
using System.Collections.Generic;
using System.Text;

namespace FootballManagerEditDataGenerator.DataScraper
{
    public class WikipediaRestResponse
    {
        public WikipediaPageParse Parse { get; set; }
        public WikipediaPageQuery Query { get; set; }
    }

    public class WikipediaPageParse
    {
        public string Title { get; set; }
        public int PageId { get; set; }
        public IEnumerable<WikipediaPageSection> Sections { get; set; }
        public string Text { get; set; }
    }

    public class WikipediaPageQuery
    {
        public IEnumerable<WikipediaPageSearch> Search { get; set; }
    }

    public class WikipediaPageSearch
    {
        public int Ns { get; set; }
        public string Title { get; set; }
        public int Pageid { get; set; }
        public int Size { get; set; }
        public int Wordcount { get; set; }
        public string Snippet { get; set; }
        public DateTime Timestamp { get; set; }
    }

    public class WikipediaPageSection
    {
        public int Toclevel { get; set; }
        public string Level { get; set; }
        public string Line { get; set; }
        public string Number { get; set; }
        public string Index { get; set; }
        public string Fromtitle { get; set; }
        public int Byteoffset { get; set; }
        public string Anchor { get; set; }
    }
}
