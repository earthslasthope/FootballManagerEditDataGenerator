using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using HtmlAgilityPack;

namespace FootballManagerEditDataGenerator.DataScraper.Strategies.InfoboxParsing
{
    public class YearRangeExtractingStrategy<TInfoboxData> : IInfoboxParsingStrategy<TInfoboxData>
        where TInfoboxData : WikipediaInfoboxItemData, new()
    {
        private readonly IInfoboxParsingStrategy<TInfoboxData> decoratee;

        public YearRangeExtractingStrategy(IInfoboxParsingStrategy<TInfoboxData> decoratee)
        {
            this.decoratee = decoratee;
        }

        public IEnumerable<TInfoboxData> ParseDataFromNode(HtmlNode node)
        {
            var pattern = new Regex("([(]?(\\d{4})\\-((\\d{4})|present)[)]?)");

            var matches = pattern.Match(node.InnerHtml);
            var resultData = new TInfoboxData();

            if (matches.Success)
            {
                node.InnerHtml = pattern.Replace(node.InnerHtml, "");

                int.TryParse(matches.Groups[2].Value, out var yearFrom);
                int.TryParse(matches.Groups[matches.Groups.Count - 1].Value, out var yearTo);

                resultData.YearFrom = yearFrom;
                resultData.YearTo = yearTo;
            }

            var decorateeData = decoratee.ParseDataFromNode(node);

            if (decorateeData.Count() == 1)
            {
                decorateeData.First().YearFrom = resultData.YearFrom;
                decorateeData.First().YearTo = resultData.YearTo;
            }

            return decorateeData;
        }
    }
}