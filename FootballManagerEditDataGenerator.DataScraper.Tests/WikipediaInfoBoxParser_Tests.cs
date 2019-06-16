﻿using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using HtmlAgilityPack;
using System.Linq;

namespace FootballManagerEditDataGenerator.DataScraper.Tests
{
    public class WikipediaInfoBoxParser_Tests
    {
        [Fact]
        public void WikipediaInfoBoxParser_Tests_Parse_ItParses()
        {
            var data = ParseSeattleSupersonicsWikiPage();

            Assert.NotNull(data);
        }

        [Fact]
        public void WikipediaInfoBoxParser_Tests_Parse_Contains()
        {
            var data = ParseSeattleSupersonicsWikiPage();

            Assert.True(data.ContainsKey("Conference"));
        }

        [Fact]
        public void WikipediaInfoBoxParser_Tests_Parse_SingleItemWithTextLink_Text()
        {
            var data = ParseSeattleSupersonicsWikiPage();
            var conference = data["Conference"].Data.First();

            Assert.Equal("Western", conference.Text);
        }

        [Fact]
        public void WikipediaInfoBoxParser_Tests_Parse_SingleItemWithTextLink_Link()
        {
            var data = ParseSeattleSupersonicsWikiPage();
            var conference = data["Conference"].Data.First();

            Assert.Equal("/wiki/Western_Conference_(NBA)", conference.Link);
        }

        [Fact]
        public void WikipediaInfoBoxParser_Tests_Parse_SingleItemWithTextLink_YearFrom_Null()
        {
            var data = ParseSeattleSupersonicsWikiPage();
            var conference = data["Conference"].Data.First();

            Assert.Null(conference.YearFrom);
        }

        [Fact]
        public void WikipediaInfoBoxParser_Tests_Parse_SingleItemWithTextLink_YearTo_Null()
        {
            var data = ParseSeattleSupersonicsWikiPage();
            var conference = data["Conference"].Data.First();

            Assert.Null(conference.YearTo);
        }

        [Fact]
        public void WikipediaInfoBoxParser_Tests_Parse_MultipleItems_DataCount()
        {
            var data = ParseSeattleSupersonicsWikiPage();
            var division = data["Division"];

            Assert.Equal(3, division.Data.Count());
        }

        [Fact]
        public void WikipediaInfoBoxParser_Tests_Parse_MultipleItems_AllHaveText()
        {
            var data = ParseSeattleSupersonicsWikiPage();
            var division = data["Division"];

            Assert.Equal("Western", division.Data.First().Text);
            Assert.Equal("Pacific", division.Data.ElementAt(1).Text);
            Assert.Equal("Northwest", division.Data.ElementAt(2).Text);
        }

        [Fact]
        public void WikipediaInfoBoxParser_Tests_Parse_ItemsWithYearRange_HasYears()
        {
            var data = ParseSeattleSupersonicsWikiPage();
            var division = data["Division"];

            Assert.Equal(1967, division.Data.First().YearFrom);
            Assert.Equal(1970, division.Data.ElementAt(1).YearFrom);
            Assert.Equal(2004, division.Data.ElementAt(2).YearFrom);

            Assert.Equal(1970, division.Data.First().YearTo);
            Assert.Equal(2004, division.Data.ElementAt(1).YearTo);
            Assert.Equal(2008, division.Data.ElementAt(2).YearTo);
        }

        [Fact]
        public void WikipediaInfoBoxParser_Tests_Parse_DateRangeInSecondLine()
        {
            var data = ParseSeattleSupersonicsWikiPage();
            var division = data["History"];

            Assert.Equal(1967, division.Data.First().YearFrom);
            Assert.Equal(2008, division.Data.ElementAt(1).YearFrom);

            Assert.Equal(2008, division.Data.First().YearTo);
            Assert.Null(division.Data.ElementAt(1).YearTo);
        }

        //[Fact]
        //public void WikipediaInfoBoxParser_Tests_Parse_HasEmptySpansWithBackgroundColor_TextIsHEXColorOfBackgroundColor()
        //{
        //    var data = ParseSeattleSupersonicsWikiPage();
        //    var teamColors = data["Team colors"];

        //    Assert.Equal("#006633", teamColors.Data.First().Text);
        //    Assert.Equal("#FFBE00", teamColors.Data.ElementAt(1).Text);
        //    Assert.Equal("#FFFFFF", teamColors.Data.ElementAt(2).Text);
        //}

        private WikipediaInfobox<WikipediaInfoBoxItemData> ParseSeattleSupersonicsWikiPage()
        {
            string htmlTable = @"<table class=""infobox vcard"" style=""width:22em""><tbody><tr><th colspan=""2"" class=""fn org"" style=""text-align:center;font-size:125%;font-weight:bold;background-color:#006633;color:#FFFFFF;border:3px solid #FFBE00;"">Seattle SuperSonics</th></tr><tr><td colspan=""2"" style=""text-align:center""><a href=""/wiki/File:Seattle_SuperSonics_logo.svg"" class=""image"" title=""Seattle SuperSonics logo""><img alt=""Seattle SuperSonics logo"" src=""//upload.wikimedia.org/wikipedia/en/thumb/a/a4/Seattle_SuperSonics_logo.svg/200px-Seattle_SuperSonics_logo.svg.png"" decoding=""async"" width=""200"" height=""213"" srcset=""//upload.wikimedia.org/wikipedia/en/thumb/a/a4/Seattle_SuperSonics_logo.svg/300px-Seattle_SuperSonics_logo.svg.png 1.5x, //upload.wikimedia.org/wikipedia/en/thumb/a/a4/Seattle_SuperSonics_logo.svg/400px-Seattle_SuperSonics_logo.svg.png 2x"" data-file-width=""91"" data-file-height=""97""></a></td></tr><tr><th scope=""row"">Conference</th><td><a href=""/wiki/Western_Conference_(NBA)"" title=""Western Conference (NBA)"">Western</a></td></tr><tr><th scope=""row"">Division</th><td>Western (1967–1970)<br><a href=""/wiki/Pacific_Division_(NBA)"" title=""Pacific Division (NBA)"">Pacific</a> (1970–2004)<br><a href=""/wiki/Northwest_Division_(NBA)"" title=""Northwest Division (NBA)"">Northwest</a> (2004–2008)</td></tr><tr><th scope=""row"">Founded</th><td>1967</td></tr><tr><th scope=""row"">History</th><td><b>Seattle SuperSonics</b><br>1967–2008<br><b><a href=""/wiki/Oklahoma_City_Thunder"" title=""Oklahoma City Thunder"">Oklahoma City Thunder</a></b><br>2008–present<sup id=""cite_ref-1"" class=""reference""><a href=""#cite_note-1"">&#91;1&#93;</a></sup><sup id=""cite_ref-2"" class=""reference""><a href=""#cite_note-2"">&#91;2&#93;</a></sup></td></tr><tr><th scope=""row"">Arena</th><td><b><a href=""/wiki/KeyArena"" title=""KeyArena"">Seattle Center Coliseum/KeyArena at Seattle Center</a></b> (1967–1978, 1985–1994, 1995–2008)<br><b><a href=""/wiki/Kingdome"" title=""Kingdome"">Kingdome</a></b> (1978–1985)<br><b><a href=""/wiki/Tacoma_Dome"" title=""Tacoma Dome"">Tacoma Dome</a></b> (1994–1995)</td></tr><tr><th scope=""row"">Location</th><td><a href=""/wiki/Seattle"" title=""Seattle"">Seattle, Washington</a></td></tr><tr><th scope=""row"">Team colors</th><td>Green, gold, white<sup id=""cite_ref-3"" class=""reference""><a href=""#cite_note-3"">&#91;3&#93;</a></sup><br><span style=""background-color:#006633; color:white; border:1px solid #000000; text-align:center;"">&#160;&#160;&#160;&#160;</span> <span style=""background-color:#FFBE00; color:black; border:1px solid #000000; text-align:center;"">&#160;&#160;&#160;&#160;</span> <span style=""background-color:#FFFFFF; color:black; border:1px solid #000000; text-align:center;"">&#160;&#160;&#160;&#160;</span></td></tr><tr><th scope=""row"">Team manager</th><td><a href=""/wiki/Seattle_SuperSonics#General_Managers"" title=""Seattle SuperSonics"">Full list</a></td></tr><tr><th scope=""row"">Head coach</th><td><a href=""/wiki/Seattle_SuperSonics#Coaches"" title=""Seattle SuperSonics"">Full list</a></td></tr><tr><th scope=""row"">Ownership</th><td><a href=""/wiki/Sam_Schulman"" title=""Sam Schulman"">Sam Schulman</a> (1967–1983)<br><a href=""/wiki/Barry_Ackerley"" title=""Barry Ackerley"">Barry Ackerley</a> (1983–2001)<br>Basketball Club of Seattle (<a href=""/wiki/Howard_Schultz"" title=""Howard Schultz"">Howard Schultz</a>, Chairman) (2001–2006)<br><a href=""/wiki/Professional_Basketball_Club_LLC"" class=""mw-redirect"" title=""Professional Basketball Club LLC"">Professional Basketball Club LLC</a> (<a href=""/wiki/Clay_Bennett_(businessman)"" title=""Clay Bennett (businessman)"">Clay Bennett</a>, Chairman) (2006–2008)</td></tr><tr><th scope=""row"">Championships</th><td><b>1</b> (<a href=""/wiki/1979_NBA_Finals"" title=""1979 NBA Finals"">1979</a>)</td></tr><tr><th scope=""row""><span class=""nowrap"">Conference titles</span></th><td><b>3</b> (<a href=""/wiki/1978_NBA_Playoffs"" class=""mw-redirect"" title=""1978 NBA Playoffs"">1978</a>, <a href=""/wiki/1979_NBA_Playoffs"" class=""mw-redirect"" title=""1979 NBA Playoffs"">1979</a>, <a href=""/wiki/1996_NBA_Playoffs"" class=""mw-redirect"" title=""1996 NBA Playoffs"">1996</a>)</td></tr><tr><th scope=""row"">Division titles</th><td><b>6</b> (<a href=""/wiki/1978%E2%80%9379_NBA_season"" title=""1978–79 NBA season"">1979</a>, <a href=""/wiki/1993%E2%80%9394_NBA_season"" title=""1993–94 NBA season"">1994</a>, <a href=""/wiki/1995%E2%80%9396_NBA_season"" title=""1995–96 NBA season"">1996</a>, <a href=""/wiki/1996%E2%80%9397_NBA_season"" title=""1996–97 NBA season"">1997</a>, <a href=""/wiki/1997%E2%80%9398_NBA_season"" title=""1997–98 NBA season"">1998</a>, <a href=""/wiki/2004%E2%80%9305_NBA_season"" title=""2004–05 NBA season"">2005</a>)</td></tr><tr><th scope=""row"">Retired numbers</th><td><b>6</b> (<a href=""/wiki/Gus_Williams_(basketball)"" title=""Gus Williams (basketball)"">1</a>, <a href=""/wiki/Nate_McMillan"" title=""Nate McMillan"">10</a>, <a href=""/wiki/Lenny_Wilkens"" title=""Lenny Wilkens"">19</a>, <a href=""/wiki/Spencer_Haywood"" title=""Spencer Haywood"">24</a>, <a href=""/wiki/Fred_Brown_(basketball)"" title=""Fred Brown (basketball)"">32</a>, <a href=""/wiki/Jack_Sikma"" title=""Jack Sikma"">43</a>)</td></tr><tr><td colspan=""2"" style=""text-align:center""></td></tr><tr><td class=""toccolours"" style=""padding: 0; border: 0px solid; text-align: center;"" colspan=""2""><table style=""width:100%; text-align:center;""><tbody><tr><td><div style=""width: 100px; margin: 0 auto; padding: 0;""><div style=""background-color: white; position: relative; left: 0px; top: 0px; width: 100px; height: 95px; margin: 0 auto; padding: 0;""><div style=""position: absolute; left: 31px; top: 0px; width: 38px; height: 59px; background-color: #ffffff;""><img alt=""Kit body sonics home.png"" src=""//upload.wikimedia.org/wikipedia/commons/9/97/Kit_body_sonics_home.png"" decoding=""async"" width=""38"" height=""59"" style=""vertical-align: top"" data-file-width=""38"" data-file-height=""59""></div><div style=""position: absolute; left: 31px; top: 0px; width: 38px; height: 59px;""><img alt=""Home jersey"" src=""//upload.wikimedia.org/wikipedia/commons/thumb/7/73/Kit_body_basketball.svg/38px-Kit_body_basketball.svg.png"" decoding=""async"" title=""Home jersey"" width=""38"" height=""59"" style=""vertical-align: top"" srcset=""//upload.wikimedia.org/wikipedia/commons/thumb/7/73/Kit_body_basketball.svg/57px-Kit_body_basketball.svg.png 1.5x, //upload.wikimedia.org/wikipedia/commons/thumb/7/73/Kit_body_basketball.svg/76px-Kit_body_basketball.svg.png 2x"" data-file-width=""38"" data-file-height=""59""></div><div style=""position: absolute; left: 0px; top: 59px; width: 100px; height: 36px; background-color: #ffffff""><img alt=""Kit shorts sonics home.png"" src=""//upload.wikimedia.org/wikipedia/commons/e/ec/Kit_shorts_sonics_home.png"" decoding=""async"" width=""100"" height=""36"" style=""vertical-align: top"" data-file-width=""100"" data-file-height=""36""></div><div style=""position: absolute; left: 0px; top: 59px; width: 100px; height: 36px;""><img alt=""Team colours"" src=""//upload.wikimedia.org/wikipedia/commons/thumb/a/af/Kit_shorts.svg/100px-Kit_shorts.svg.png"" decoding=""async"" title=""Team colours"" width=""100"" height=""36"" style=""vertical-align: top"" srcset=""//upload.wikimedia.org/wikipedia/commons/thumb/a/af/Kit_shorts.svg/150px-Kit_shorts.svg.png 1.5x, //upload.wikimedia.org/wikipedia/commons/thumb/a/af/Kit_shorts.svg/200px-Kit_shorts.svg.png 2x"" data-file-width=""100"" data-file-height=""36""></div></div><div style=""padding-top: 0.6em; text-align: center;""><b>Home</b></div></div></td><td><div style=""width: 100px; margin: 0 auto; padding: 0;""><div style=""background-color: white; position: relative; left: 0px; top: 0px; width: 100px; height: 95px; margin: 0 auto; padding: 0;""><div style=""position: absolute; left: 31px; top: 0px; width: 38px; height: 59px; background-color: #eeeeee;""><img alt=""Kit body sonics road.png"" src=""//upload.wikimedia.org/wikipedia/commons/a/ab/Kit_body_sonics_road.png"" decoding=""async"" width=""38"" height=""59"" style=""vertical-align: top"" data-file-width=""38"" data-file-height=""59""></div><div style=""position: absolute; left: 31px; top: 0px; width: 38px; height: 59px;""><img alt=""Road jersey"" src=""//upload.wikimedia.org/wikipedia/commons/thumb/7/73/Kit_body_basketball.svg/38px-Kit_body_basketball.svg.png"" decoding=""async"" title=""Road jersey"" width=""38"" height=""59"" style=""vertical-align: top"" srcset=""//upload.wikimedia.org/wikipedia/commons/thumb/7/73/Kit_body_basketball.svg/57px-Kit_body_basketball.svg.png 1.5x, //upload.wikimedia.org/wikipedia/commons/thumb/7/73/Kit_body_basketball.svg/76px-Kit_body_basketball.svg.png 2x"" data-file-width=""38"" data-file-height=""59""></div><div style=""position: absolute; left: 0px; top: 59px; width: 100px; height: 36px; background-color: #eeeeee""><img alt=""Kit shorts sonics road.png"" src=""//upload.wikimedia.org/wikipedia/commons/7/70/Kit_shorts_sonics_road.png"" decoding=""async"" width=""100"" height=""36"" style=""vertical-align: top"" data-file-width=""100"" data-file-height=""36""></div><div style=""position: absolute; left: 0px; top: 59px; width: 100px; height: 36px;""><img alt=""Team colours"" src=""//upload.wikimedia.org/wikipedia/commons/thumb/a/af/Kit_shorts.svg/100px-Kit_shorts.svg.png"" decoding=""async"" title=""Team colours"" width=""100"" height=""36"" style=""vertical-align: top"" srcset=""//upload.wikimedia.org/wikipedia/commons/thumb/a/af/Kit_shorts.svg/150px-Kit_shorts.svg.png 1.5x, //upload.wikimedia.org/wikipedia/commons/thumb/a/af/Kit_shorts.svg/200px-Kit_shorts.svg.png 2x"" data-file-width=""100"" data-file-height=""36""></div></div><div style=""padding-top: 0.6em; text-align: center;""><b>Road</b></div></div></td><td><div style=""width: 100px; margin: 0 auto; padding: 0;""><div style=""background-color: white; position: relative; left: 0px; top: 0px; width: 100px; height: 95px; margin: 0 auto; padding: 0;""><div style=""position: absolute; left: 31px; top: 0px; width: 38px; height: 59px; background-color: #;""><img alt=""Kit body sonics altRoad.png"" src=""//upload.wikimedia.org/wikipedia/commons/0/0b/Kit_body_sonics_altRoad.png"" decoding=""async"" width=""38"" height=""59"" style=""vertical-align: top"" data-file-width=""38"" data-file-height=""59""></div><div style=""position: absolute; left: 31px; top: 0px; width: 38px; height: 59px;""><img alt=""Alternate jersey"" src=""//upload.wikimedia.org/wikipedia/commons/thumb/7/73/Kit_body_basketball.svg/38px-Kit_body_basketball.svg.png"" decoding=""async"" title=""Alternate jersey"" width=""38"" height=""59"" style=""vertical-align: top"" srcset=""//upload.wikimedia.org/wikipedia/commons/thumb/7/73/Kit_body_basketball.svg/57px-Kit_body_basketball.svg.png 1.5x, //upload.wikimedia.org/wikipedia/commons/thumb/7/73/Kit_body_basketball.svg/76px-Kit_body_basketball.svg.png 2x"" data-file-width=""38"" data-file-height=""59""></div><div style=""position: absolute; left: 0px; top: 59px; width: 100px; height: 36px; background-color: #""><img alt=""Kit shorts sonics altRoad.png"" src=""//upload.wikimedia.org/wikipedia/commons/d/d7/Kit_shorts_sonics_altRoad.png"" decoding=""async"" width=""100"" height=""36"" style=""vertical-align: top"" data-file-width=""100"" data-file-height=""36""></div><div style=""position: absolute; left: 0px; top: 59px; width: 100px; height: 36px;""><img alt=""Team colours"" src=""//upload.wikimedia.org/wikipedia/commons/thumb/a/af/Kit_shorts.svg/100px-Kit_shorts.svg.png"" decoding=""async"" title=""Team colours"" width=""100"" height=""36"" style=""vertical-align: top"" srcset=""//upload.wikimedia.org/wikipedia/commons/thumb/a/af/Kit_shorts.svg/150px-Kit_shorts.svg.png 1.5x, //upload.wikimedia.org/wikipedia/commons/thumb/a/af/Kit_shorts.svg/200px-Kit_shorts.svg.png 2x"" data-file-width=""100"" data-file-height=""36""></div></div><div style=""padding-top: 0.6em; text-align: center;""><b>Alternate</b></div></div></td></tr><tr class=""mw-empty-elt""></tr></tbody></table></td></tr><tr style=""display:none""><td colspan=""2""></td></tr></tbody></table>";

            var doc = new HtmlDocument();
            doc.LoadHtml(htmlTable);

            var parser = new WikipedaInfoBoxParser<WikipediaInfoBoxItemData>(doc.DocumentNode);
            return parser.Parse();
        }
    }
}