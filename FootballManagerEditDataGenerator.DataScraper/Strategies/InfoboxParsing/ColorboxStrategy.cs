using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using HtmlAgilityPack;

namespace FootballManagerEditDataGenerator.DataScraper.Strategies.InfoboxParsing
{
    public class ColorboxStrategy<TInfoboxData> : IInfoboxParsingStrategy<TInfoboxData>
        where TInfoboxData : WikipediaInfoboxItemData, new()
    {
        public IEnumerable<TInfoboxData> ParseDataFromNode(HtmlNode node)
        {
            return ColorboxStrategyExtensions.ParseColorsFromChildNodes(node).Select(color => new TInfoboxData() { Text = color });
        }
    }

    internal static class ColorboxStrategyExtensions
    {
        public static IEnumerable<string> ParseColorsFromChildNodes(HtmlNode node)
        {
            var regex = new Regex("(background-color:)\\s*(#([a-fA-F0-9]{6}|[a-fA-F0-9]{3}))");

            var spanStyles = node.ChildNodes.Where(x => x.Name == "span" && x.Attributes.Contains("style"));

            var matches = spanStyles.Select(n => regex.Match(n.Attributes["style"].Value)).Where(m => m.Groups.Count > 0);

            return matches.Select(m => m.Groups[2].Value);
        }
    }
}