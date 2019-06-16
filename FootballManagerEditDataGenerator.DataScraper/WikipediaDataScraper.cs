using HtmlAgilityPack;
using Fizzler.Systems.HtmlAgilityPack;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Humanizer;

namespace FootballManagerEditDataGenerator.DataScraper
{
    public class WikipediaDataScraper
    {
        private const string API_URL = "https://en.wikipedia.org/w/api.php";

        public IRestResponse<WikipediaRestResponse> Query(string page)
        {
            var client = GetRestClient();
            var request = GetBaseRestRequest();
            request.AddParameter("page", page);

            return client.Execute<WikipediaRestResponse>(request);
        }

        public IEnumerable<WikipediaPageSection> GetTopLevelSections(string page)
        {
            var client = GetRestClient();
            var request = GetBaseRestRequest();
            request.AddParameter("page", page);
            request.AddParameter("prop", "sections");

            var response = client.Execute<WikipediaRestResponse>(request);

            return response.Data.Parse.Sections.Where(x => x.Toclevel == 1);
        }

        public object GetSection(string pageTitle, string sectionId)
        {
            var client = GetRestClient();
            var request = GetBaseRestRequest();
            request.AddParameter("page", pageTitle);
            request.AddParameter("section", sectionId);

            var response = client.Execute<WikipediaRestResponse>(request);

            return response;
        }

        public IEnumerable<WikipediaSearchResult> SearchPages(string searchText)
        {
            var client = GetRestClient();
            var request = GetBaseRestRequest("query");
            request.AddParameter("list", "search");
            request.AddParameter("srsearch", searchText);

            var response = client.Execute<WikipediaRestResponse>(request);

            return response.Data.Query.Search
                .Select(x => new WikipediaSearchResult
                {
                    PageTitle = x.Title,
                    SnippetHtml = x.Snippet
                })
                .ToList();
        }

        public TInfobox GetInfoBox<TInfobox, TInfoboxData>(string pageTitle)
            where TInfobox : WikipediaInfobox<TInfoboxData>, new()
            where TInfoboxData : WikipediaInfoBoxItemData, new()
        {
            var client = GetRestClient();
            var request = GetBaseRestRequest();
            request.AddParameter("page", pageTitle);

            var response = client.Execute<WikipediaRestResponse>(request);
            var parsedText = response.Data.Parse.Text;
            var document = new HtmlDocument();
            document.LoadHtml(parsedText);

            var tableNode = document.DocumentNode.QuerySelector("table.infobox");
            var parser = new WikipedaInfoBoxParser<TInfobox, TInfoboxData>(tableNode);

            return parser.Parse();
        }

        private IRestClient GetRestClient()
        {
            var client = new RestClient(API_URL);
            client.UseSerializer(() => new RestSharpSerializer());

            return client;
        }

        private IRestRequest GetBaseRestRequest(string action = "parse")
        {
            var request = new RestRequest(Method.GET);
            request.AddParameter("action", action);
            request.AddParameter("format", "json");
            return request;
        }
    }
}