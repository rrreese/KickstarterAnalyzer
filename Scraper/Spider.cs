﻿using System;
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
        private const string root = "http://www.kickstarter.com/";

        public IEnumerable<Uri> GetLinks(string baseUri)
        {
            var links = new List<Uri>();

            GetLinksOnPage(links, new Uri(root + baseUri + "&page=1"));

            for (int i = 2; i < 50; i++)
            {
                if (!GetLinksOnPage(links, new Uri(root + baseUri + "&page=" + i)))
                    break;
            }

            return links;
        }

        private static bool GetLinksOnPage(List<Uri> links, Uri page)
        {           
            //Download category page
            using (Stream htmlStream = new WebClient().OpenRead(page))
            {
                var doc = new HtmlDocument();
                doc.Load(htmlStream);

                var hBlock = doc.DocumentNode.SelectNodes("//h6[@class='project-title mobile-center']");

                var collection = hBlock.Select(htmlNode => htmlNode.ChildNodes[1].Attributes["href"].Value).Select(linkFragment => new Uri(root + linkFragment));

                if (!collection.Any()) 
                    return false;

                links.AddRange(collection);
            }

            return true;
        }
    }
}
