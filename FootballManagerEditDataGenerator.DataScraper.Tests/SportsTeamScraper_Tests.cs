using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace FootballManagerEditDataGenerator.DataScraper.Tests
{
    public class SportsTeamScraper_Tests
    {
        private SportsTeamScraper sportsTeamScraper;

        private const string TEST_TEAM = "Liverpool F.C.";

        public SportsTeamScraper_Tests()
        {
            this.sportsTeamScraper = new SportsTeamScraper(new WikipediaDataScraper());
        }

        [Fact]
        public void SportsTeamScraper_GetPlayers_ReturnsPlayers()
        {
            var players = sportsTeamScraper.GetPlayers(TEST_TEAM);

            Assert.NotNull(players);
        }
    }
}
