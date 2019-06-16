using System;
using System.Collections.Generic;
using System.Text;

namespace FootballManagerEditDataGenerator.DataScraper
{
    public class SportsTeamInfobox : WikipediaInfobox<SportsTeamInfoboxData>
    {
        public IEnumerable<string> TeamColors { get; set; }
        public Dictionary<string, KitFile> Kits { get; set; }
    }

    public class SportsTeamInfoboxData : WikipediaInfoboxItemData
    {
        
    }

    public class KitFile
    {
        public byte[] Blob { get; set; }
        public string MimeType { get; set; }
    }
}