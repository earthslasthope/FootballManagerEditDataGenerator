using System;
using System.Linq;
using Xunit;

namespace FootballManagerEditDataGenerator.DataScraper.Tests
{
    public class WikipediaDataScraper_Tests
    {
        private WikipediaDataScraper dataScraper;

        public WikipediaDataScraper_Tests()
        {
            this.dataScraper = new WikipediaDataScraper();
        }

        [Fact]
        public void WikipediaDataScraper_Query_ItReturnsResponse()
        {
            var data = this.dataScraper.Query("Liverpool F.C.");

            Assert.NotNull(data);
        }

        [Fact]
        public void WikipediaDataScraper_GetTopLevelSections_ItReturnsData()
        {
            var data = this.dataScraper.GetTopLevelSections("Liverpool F.C.");

            Assert.NotNull(data);
            Assert.True(data.Any());
        }

        [Fact]
        public void WikipediaDataScraper_GetInfoBox_ItReturnsData()
        {
            var data = this.dataScraper.GetInfoBox<WikipediaInfoBoxItemData>("Seattle_SuperSonics");

            Assert.NotNull(data);
        }

        [Fact]
        public void WikipediaDataScraper_SearchPages_ItReturnsMatchingPages()
        {
            var data = this.dataScraper.SearchPages("Liverpool FC");

            Assert.True(data.Any(x => x.PageTitle == "Liverpool F.C."));
        }
    }
}
