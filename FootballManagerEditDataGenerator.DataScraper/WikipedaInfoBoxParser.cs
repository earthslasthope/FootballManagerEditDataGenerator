using Fizzler.Systems.HtmlAgilityPack;
using HtmlAgilityPack;
using Humanizer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FootballManagerEditDataGenerator.DataScraper
{
    internal class WikipedaInfoBoxParser
    {
        private readonly HtmlNode rootTableElement;

        public WikipedaInfoBoxParser(HtmlNode rootTableNode)
        {
            this.rootTableElement = rootTableNode;
        }

        public WikipediaInfobox Parse()
        {
            var scrapedTableElements = this.rootTableElement.QuerySelectorAll("tbody > tr")
                .Select(tr => new
                {
                    th = tr.QuerySelector("th"),
                    td = tr.QuerySelector("td")
                });

            var dictionaryValues = scrapedTableElements
                .Where(tr => tr.th != null && tr.th.ChildNodes.Count == 1 && tr.th.ChildNodes[0].NodeType == HtmlNodeType.Text && tr.td != null)
                .Select(tr =>
                {
                    string property = tr.th.InnerHtml;
                    string text = null;
                    string hyperlink = null;

                    var childNodes = tr.td.ChildNodes;

                    if (childNodes?.Count == 1)
                    {
                        text = childNodes[0].InnerText;
                    }

                    if (childNodes?.Count == 1 && childNodes[0].NodeType == HtmlNodeType.Element && childNodes[0].Name == "a")
                    {
                        hyperlink = childNodes[0].Attributes["href"]?.Value;
                    }

                    return new
                    {
                        property,
                        text,
                        hyperlink
                    };
                });

            var dictionary = dictionaryValues.ToDictionary(
                x => x.property.Dehumanize(), 
                x => new WikipediaInfoboxItem
                {
                    Property = x.property,
                    Values = new List<WikipediaInfoBoxItemData>
                    {
                        new WikipediaInfoBoxItemData
                        {
                            Text = x.text,
                            Link = x.hyperlink
                        }
                    }
                });

            var result = new WikipediaInfobox();

            foreach (var kvp in dictionary)
            {
                result[kvp.Key] = kvp.Value;
            }

            return result;
        }

        /// <summary>
        /// For a HTML node containing one or more collections of text with hyperlink followed
        /// by possibly year ranges.
        /// This is expected to handle a variety of scenarios. See WikipediaInfoBoxParser_Tests
        /// for examples.
        /// </summary>
        /// <param name="node">
        /// HTML node containing either plain text or hyperlink. It can include br tags
        /// and it's also possible for there to be more than one such data.
        /// </param>
        /// <returns>Collection of Info Box Data Items for one field.</returns>
        internal IEnumerable<WikipediaInfoBoxItemData> ParseDataFromNode(HtmlNode node)
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

            return new List<WikipediaInfoBoxItemData>
            {
                new WikipediaInfoBoxItemData
                {
                    Text = text,
                    Link = hyperlink
                }
            };
        }
    }
}