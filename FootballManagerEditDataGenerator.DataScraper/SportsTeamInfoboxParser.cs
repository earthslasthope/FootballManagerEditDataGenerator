using System;
using System.Collections.Generic;
using System.Text;
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
            return base.Parse();
        }

        internal override IEnumerable<SportsTeamInfoboxData> ParseDataFromNode(HtmlNode node)
        {
            return base.ParseDataFromNode(node);
        }
    }
}