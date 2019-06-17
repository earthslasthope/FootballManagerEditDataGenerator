using System.Collections.Generic;
using HtmlAgilityPack;

namespace FootballManagerEditDataGenerator.DataScraper.Strategies.InfoboxParsing
{
    public interface IInfoboxParsingStrategy<TInfoboxData>
    {
        IEnumerable<TInfoboxData> ParseDataFromNode(HtmlNode node);
    }
}