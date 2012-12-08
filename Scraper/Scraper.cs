namespace Scraper
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Text.RegularExpressions;
    using System.Xml.Linq;

    using HtmlAgilityPack;

    public class Scraper
    {
        public Scraper()
        {
            this.Links = new List<Uri>();
            this.Projects = new List<Project>();
        }

        public List<Uri> Links { get; set; }

        public List<Project> Projects { get; set; }

        public void Download()
        {
            foreach (var link in this.Links)
            {
                var project = new Project();

                Stream htmlStream = new WebClient().OpenRead(link);

                HtmlDocument doc = new HtmlDocument();
                doc.Load(htmlStream);

                project.Levels.AddRange(GetLevels(doc));

                this.Projects.Add(project);
            }
        }

        private static IEnumerable<BackingLevel> GetLevels(HtmlDocument doc)
        {
            var rewardNode = doc.DocumentNode.Descendants("div")
                                             .Where(d => d.Attributes.Contains("class") && d.Attributes["class"].Value.Contains("NS-projects-reward"));

            foreach (var htmlNode in rewardNode)
            {
                var level = new BackingLevel();
              
                level.Money = GetMoney(htmlNode);

                level.Backers = GetBackers(htmlNode);

                SetBackersAllowedValues(htmlNode, level);

                level.IsSoldOut = GetIsSoldOut(htmlNode);

                if (level.IsSoldOut)
                {
                    level.MaxBackersAllowed = level.Backers;
                }

                level.Description = GetDescription(htmlNode);

                yield return level;
            }
        }

        private static string GetDescription(HtmlNode htmlNode)
        {
            var limitBackersNode = htmlNode.Descendants("div")
                                           .Where(d => d.Attributes.Contains("class") && d.Attributes["class"].Value.Contains("desc")).ToList();

            return limitBackersNode.First().Descendants().Skip(1).First().InnerText;
        }

        private static void SetBackersAllowedValues(HtmlNode htmlNode, BackingLevel level)
        {
            var limitBackersNode = htmlNode.Descendants("span")
                                           .Where(d => d.Attributes.Contains("class") && d.Attributes["class"].Value.Contains("limited-number")).ToList();

            if (limitBackersNode.Any())
            {
                // Format: (3 of 6 left)
                level.RemainingBackersAllowed = int.Parse(limitBackersNode.First().InnerText.Split(' ')[0].Replace("(", string.Empty));
                level.MaxBackersAllowed = int.Parse(limitBackersNode.First().InnerText.Split(' ')[2]);
            }
        }

        private static bool GetIsSoldOut(HtmlNode htmlNode)
        {
            var soldOutNode =
                htmlNode.Descendants("span").Where(
                    d => d.Attributes.Contains("class") && d.Attributes["class"].Value.Contains("sold-out"));

            return soldOutNode.Any();
        }

        private static int GetBackers(HtmlNode htmlNode)
        {
            var numberOfBackersNode =
                htmlNode.Descendants("span").Where(
                    d => d.Attributes.Contains("class") && d.Attributes["class"].Value.Contains("num-backers"));

            return int.Parse(numberOfBackersNode.First().InnerText.Split(' ')[0]);
        }

        private static int GetMoney(HtmlNode htmlNode)
        {
            var h3Node = htmlNode.Descendants("h3");

            // Format: Pledge $5,000 or more
            string numberString =
                new Regex(@"[0-9]+(,[0-9]+)*").Match(h3Node.First().InnerText).Groups[0].ToString().Replace(",", string.Empty);

            return int.Parse(numberString);
        }

        [DebuggerDisplay("{Name} - {Levels.Count} - Funding Succeeded: {FundingSucceeded}")]
        public class Project
        {
            public Project()
            {
                this.Levels = new List<BackingLevel>();
            }

            public string Name { get; set; }

            public bool FundingSucceeded { get; set; }

            public Uri Link { get; set; }

            public List<BackingLevel> Levels { get; set; }

            public string Currency { get; set; }
        }

        [DebuggerDisplay("{Money} - {Backers}")]
        public class BackingLevel
        {
            public int Money { get; set; }

            public int Backers { get; set; }

            public int MaxBackersAllowed { get; set; }

            public int RemainingBackersAllowed { get; set; }

            public bool IsSoldOut { get; set; }

            public string Description { get; set; }
        }
    }
}
