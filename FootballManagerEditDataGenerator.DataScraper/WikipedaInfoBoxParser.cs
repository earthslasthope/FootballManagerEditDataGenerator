using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Fizzler.Systems.HtmlAgilityPack;
using FootballManagerEditDataGenerator.DataScraper.Strategies.InfoboxParsing;
using HtmlAgilityPack;
using Humanizer;

[assembly: InternalsVisibleTo("FootballManagerEditDataGenerator.DataScraper.Tests")]
namespace FootballManagerEditDataGenerator.DataScraper
{
    internal class WikipedaInfoboxParser<TInfobox, TInfoboxData>
        where TInfobox : WikipediaInfobox<TInfoboxData>, new()
        where TInfoboxData : WikipediaInfoboxItemData, new()
    {
        private readonly HtmlNode rootTableElement;

        public WikipedaInfoboxParser(HtmlNode rootTableNode)
        {
            this.rootTableElement = rootTableNode;
        }

        public virtual TInfobox Parse()
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
                    var data = ParseDataFromNode(tr.td);

                    return new
                    {
                        property,
                        data
                    };
                });

            var dictionary = dictionaryValues.ToDictionary(
                x => x.property.Dehumanize(), 
                x => new WikipediaInfoboxItem<TInfoboxData>
                {
                    Property = x.property,
                    Data = x.data
                });

            var result = new TInfobox();

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
        internal virtual IEnumerable<TInfoboxData> ParseDataFromNode(HtmlNode node)
        {
            var strategy = SelectStrategy(node);

            return strategy.ParseDataFromNode(node);
        }

        internal virtual IInfoboxParsingStrategy<TInfoboxData> SelectStrategy(HtmlNode node)
        {
            var defaultStrategy = new DefaultStrategy<TInfoboxData>();

            if (node.ChildNodes.Any(x => x.Name == "br"))
            {
                return new MultiLineStrategy<TInfoboxData>(defaultStrategy);
            }

            return defaultStrategy;
        }
    }
}