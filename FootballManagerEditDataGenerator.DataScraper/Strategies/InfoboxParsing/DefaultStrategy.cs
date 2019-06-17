using System;
using System.Collections.Generic;
using System.Text;
using HtmlAgilityPack;

namespace FootballManagerEditDataGenerator.DataScraper.Strategies.InfoboxParsing
{
    public class DefaultStrategy<TInfoboxData> : IInfoboxParsingStrategy<TInfoboxData>
        where TInfoboxData : WikipediaInfoboxItemData, new()
    {
        public IEnumerable<TInfoboxData> ParseDataFromNode(HtmlNode node)
        {
            string text = null;
            string hyperlink = null;

            var childNodes = node.ChildNodes;

            if (childNodes?.Count == 1)
            {
                text = childNodes[0].InnerText;
            }

            if (childNodes?.Count == 1 && childNodes[0].NodeType == HtmlNodeType.Element && childNodes[0].Name == "a")
            {
                hyperlink = childNodes[0].Attributes["href"]?.Value;
            }

            return new List<TInfoboxData>
            {
                new TInfoboxData
                {
                    Text = text,
                    Link = hyperlink
                }
            };
        }
    }
}