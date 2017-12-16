using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HtmlAgilityPack;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using SkyTorrentWebService.Models;

namespace SkyTorrentWebService.Helpers
{
    public class SkyTorrentProcessor
    {

        public string Process(string skyTorrentUrl ,string searchEntry)
        {
            string processResult = JsonConvert.SerializeObject(ScrapePage(skyTorrentUrl, searchEntry));

            return processResult;
        }

        private List<SkyTorrentModel> ScrapePage(string skyTorrentUrl, string searchEntry)
        {
            List<SkyTorrentModel> result = new List<SkyTorrentModel>();
            var getHtmlWeb = new HtmlWeb();
            var document = getHtmlWeb.Load(skyTorrentUrl + searchEntry + "/");


            var torrentNames = (from x in document.DocumentNode.Descendants()
                where x.Name == "td" && x.Attributes.Contains("style")
                where x.Attributes["style"].Value == "word-wrap: break-word;"
                select x.InnerText.Split('\n', '\n')[1].Trim()).ToArray();

            //var torrentDownloadLinks = (from x in document.DocumentNode.Descendants()
            //    where x.Name == "a" && !x.Attributes.Contains("rel")
            //    select x.Attributes["href"].Value).ToArray();

            var torrentMagnetLinks = (from x in document.DocumentNode.Descendants()
                where x.Name == "a" && x.Attributes.Contains("rel")
                where x.Attributes["rel"].Value.Contains("nofollow")
                select x.Attributes["href"].Value).ToArray();

            var torrentSize = (from x in document.DocumentNode.Descendants()
                where x.Name == "td" && x.Attributes.Contains("class")
                where x.Attributes["class"].Value.Contains("is-hidden-touch")
                select x.InnerText).Where(x => x.Contains("MB") || x.Contains("GB")).ToArray();

            var torrentUpdateDates = (from x in document.DocumentNode.Descendants()
                where x.Name == "td" && x.Attributes.Contains("class")
                where x.Attributes["class"].Value.Contains("is-hidden-touch")
                select x.InnerText).Where(x => x.Contains("Jan") || x.Contains("Feb") || 
                x.Contains("Mar") || x.Contains("Apr") || x.Contains("May") || x.Contains("Jun") || x.Contains("Jul") || x.Contains("Aug") || 
                x.Contains("Sep") || x.Contains("Oct") || x.Contains("Nov") || x.Contains("Dec")).ToArray();

            var seeds = (from x in document.DocumentNode.Descendants()
                where x.Name == "td" && !x.Attributes.Contains("class")
                where x.Attributes["style"].Value.Contains("text-align: center;")
                select x.InnerText).Where((t, i) => i % 2 == 0).ToArray();

            var peers = (from x in document.DocumentNode.Descendants()
                where x.Name == "td" && !x.Attributes.Contains("class")
                where x.Attributes["style"].Value.Contains("text-align: center;")
                select x.InnerText).Where((t, i) => i % 2 != 0).ToArray();

            for (int i = 0; i < torrentNames.Count(); i++)
            {
                result.Add(new SkyTorrentModel()
                {
                    Name = torrentNames[i],
                    Magnet = torrentMagnetLinks[i],
                    UploadedDate = torrentUpdateDates[i],
                    Seed = Int32.Parse(seeds[i]),
                    Peers= Int32.Parse(peers[i]),
                    Size = torrentSize[i],
                    Verified = true
                });

            }

            return result;
        }
    }
}
