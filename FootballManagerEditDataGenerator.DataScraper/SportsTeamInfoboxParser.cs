using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using FootballManagerEditDataGenerator.DataScraper.Strategies.InfoboxParsing;
using HtmlAgilityPack;

namespace FootballManagerEditDataGenerator.DataScraper
{
    internal class SportsTeamInfoboxParser : WikipedaInfoboxParser<SportsTeamInfobox, SportsTeamInfoboxData>
    {
        public SportsTeamInfoboxParser(HtmlNode rootTableNode) : base(rootTableNode)
        {
        }

        public override SportsTeamInfobox Parse()
        {
            return ApplyColors(base.Parse());
        }

        internal override IInfoboxParsingStrategy<SportsTeamInfoboxData> SelectStrategy(HtmlNode node)
        {
            if (node.ChildNodes.Any(x => x.Name == "span" && x.Attributes.Contains("style") && x.Attributes["style"].Value.Contains("background-color")))
            {
                return new ColorboxStrategy<SportsTeamInfoboxData>();
            }

            return base.SelectStrategy(node);
        }

        private SportsTeamInfobox ApplyColors(SportsTeamInfobox infobox)
        {
            var regex = new Regex("#([a-fA-F0-9]{6}|[a-fA-F0-9]{3})");

            var matches = infobox.Where(x => x.Value.Data != null && x.Value.Data.All(d => d.Text != null && regex.IsMatch(d.Text)));

            if (matches.Count() != 1)
            {
                return infobox;
            }

            var selectedMatch = matches.First();

            var result = new SportsTeamInfobox();
            
            foreach (var key in infobox.Keys)
            {
                if (key == selectedMatch.Key)
                {
                    continue;
                }

                result[key] = infobox[key];
            }

            result.Kits = infobox.Kits;
            result.TeamColors = selectedMatch.Value.Data.Select(x => x.Text);

            return result;
        }
    }
}