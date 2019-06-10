using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FootballManagerEditDataGenerator.DataScraper
{
    public class SportsTeamScraper
    {
        private readonly WikipediaDataScraper dataScraper;

        public SportsTeamScraper(WikipediaDataScraper dataScraper)
        {
            this.dataScraper = dataScraper;
        }

        public object GetPlayers(string pageTitle)
        {
            var pageSections = this.dataScraper.GetTopLevelSections(pageTitle);
            var playersSection = pageSections.FirstOrDefault(x => x.Line == "Players");

            if (playersSection == null)
            {
                return null;
            }

            return this.dataScraper.GetSection(pageTitle, playersSection.Number);
        }
    }
}