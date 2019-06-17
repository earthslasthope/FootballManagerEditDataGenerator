using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HtmlAgilityPack;

namespace FootballManagerEditDataGenerator.DataScraper.Strategies.InfoboxParsing
{
    /// <summary>
    /// This strategy takes a decoratee strategy which gets applied to elements separated by br
    /// tags and returns results with a list of data corresponding to the results of each set of
    /// elements separated by br.
    /// </summary>
    public class MultiLineStrategy<TInfoboxData> : IInfoboxParsingStrategy<TInfoboxData>
    {
        private readonly IInfoboxParsingStrategy<TInfoboxData> decoratee;

        public MultiLineStrategy(IInfoboxParsingStrategy<TInfoboxData> decoratee)
        {
            this.decoratee = decoratee;
        }

        public IEnumerable<TInfoboxData> ParseDataFromNode(HtmlNode node)
        {
            List<List<HtmlNode>> groupedChildNodes = new List<List<HtmlNode>>();
            var currentGroup = new List<HtmlNode>();
            groupedChildNodes.Add(currentGroup);

            foreach (var childNode in node.ChildNodes)
            {
                if (childNode.Name == "br")
                {
                    currentGroup = new List<HtmlNode>();
                    groupedChildNodes.Add(currentGroup);
                }
                else
                {
                    currentGroup.Add(childNode);
                }
            }

            var childNodes = groupedChildNodes.Select(nodes => 
            {
                var returnNode = HtmlNode.CreateNode("<div></div>");
                
                foreach (var childNode in nodes)
                {
                    returnNode.AppendChild(childNode);
                }

                return returnNode;
            });

            var results = childNodes.Select(x => decoratee.ParseDataFromNode(x));

            return results.SelectMany(x => x.ToList()).ToList();
        }
    }
}