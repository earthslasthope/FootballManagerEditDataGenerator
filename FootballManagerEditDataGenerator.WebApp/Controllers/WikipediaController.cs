using FootballManagerEditDataGenerator.DataScraper;
using Microsoft.AspNetCore.Mvc;

namespace FootballManagerEditDataGenerator.WebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WikipediaController : ControllerBase
    {
        private readonly WikipediaDataScraper dataScraper;

        public WikipediaController(WikipediaDataScraper dataScraper)
        {
            this.dataScraper = dataScraper;
        }

        [HttpGet("[action]/{pageTitle}")]
        public IActionResult Infobox(string pageTitle)
        {
            return Ok(dataScraper.GetInfoBox<WikipediaInfoBoxItemData>(pageTitle));
        }

        [HttpGet("[action]/{searchString}")]
        public IActionResult Search(string searchString)
        {
            return Ok(dataScraper.SearchPages(searchString));
        }
    }
}