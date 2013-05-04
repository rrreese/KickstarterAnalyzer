using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace Scraper
{
    public class Spider
    {
        private const string root = "http://www.kickstarter.com";

        public IEnumerable<Uri> GetLinks()
        {
            var links = new List<Uri>();

            GetLinksOnPage(links, new Uri("http://www.kickstarter.com/discover/categories/video%20games/most-funded"));

            for (int i = 1; i < 50; i++)
            {
                GetLinksOnPage(links, new Uri("http://www.kickstarter.com/discover/categories/video%20games/most-funded?page=" + i));
            }

            
            throw new NotImplementedException();
        }

        private static bool GetLinksOnPage(List<Uri> links, Uri page)
        {           
            //Download category page
            using (Stream htmlStream = new WebClient().OpenRead(page))
            {
                var doc = new HtmlDocument();
                doc.Load(htmlStream);

                var hBlock =
                    doc.DocumentNode.Descendants("h2")
                       .Where(d => d.Attributes.Contains("class") && d.Attributes["class"].Value.Contains("bbcard_name"));

                var collection = hBlock.Select(htmlNode => htmlNode.ChildNodes[1].ChildNodes[1].Attributes["href"].Value).Select(linkFragment => new Uri(root + linkFragment));

                if (!collection.Any()) 
                    return false;

                links.AddRange(collection);
            }

            return true;
        }
    }
}
