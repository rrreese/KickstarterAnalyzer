﻿namespace Scraper
{
    using HtmlAgilityPack;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Text.RegularExpressions;

    public class Scraper
    {      
        public Scraper()
        {
            this.Links = new List<Uri>();
            this.Projects = new List<Project>();
        }

        public IEnumerable<Uri> Links { get; set; }

        public List<Project> Projects { get; set; }
       
        public void Download()
        {
            foreach (var link in this.Links.Select(uri => "http://" + uri.Authority + uri.AbsolutePath).Distinct())
            {
                Console.WriteLine(link);

                var project = new Project();

                using (Stream htmlStream = new WebClient().OpenRead(link))
                {
                    var doc = new HtmlDocument();
                    doc.Load(htmlStream);

                    project.Name = GetName(doc);

                    project.Company = GetCompany(doc);

                    project.Description = GetProjectDescription(doc);

                    project.TotalFunding = GetFunding(doc);

                    project.FundingGoal = GetFundingGoal(doc);

                    project.Backers = GetTotalProjectBackers(doc);

                    Currency currency;
                    if (Currency.TryParse(GetCurrency(doc), out currency))
                    {
                        project.Currency = currency;
                    }
                    else
                    {
                        Console.WriteLine("CURRENCY NOT FOUND" + GetCurrency(doc));
                        continue;
                    }

                    project.Link = GetLink(doc);

                    project.FundingSucceeded = GetFundingSucceeded(doc);

                    project.Levels.AddRange(GetLevels(doc, project.Currency));

                   // project.StartDate = GetStartDate(doc);

                    project.EndDate = GetEndDate(doc);

                    project.Category = GetCatgory(doc);

                    this.Projects.Add(project);
                }
            }
        }

        public void Save(IOutput output, string filename)
        {
            output.Projects = this.Projects;
            output.Save(filename);
        }


        public void Load(string testXML)
        {
            var xml = new XML();
            xml.Load(testXML);
            this.Projects = xml.Projects.ToList();
        }

        private static string GetCatgory(HtmlDocument doc)
        {
            var categoryNode = GetNodesFor(doc, "li", "class", "category");

            return categoryNode.First().ChildNodes.ToList()[1].ChildNodes.ToList()[1].InnerHtml.Trim();
        }

        private static IEnumerable<HtmlNode> GetNodesFor(HtmlDocument doc, string element, string attribute, string attributeValue)
        {
            return doc.DocumentNode
                      .Descendants(element)
                      .Where(d => d.Attributes.Contains(attribute) && d.Attributes[attribute].Value.Contains(attributeValue));
        }

        private static IEnumerable<HtmlNode> GetNodesFor(HtmlNode node, string element, string attribute, string attributeValue)
        {
            return node.Descendants(element)
                       .Where(d => d.Attributes.Contains(attribute) && d.Attributes[attribute].Value.Contains(attributeValue));
        }

        private static DateTime GetStartDate(HtmlDocument doc)
        {
            var startNode = GetNodesFor(doc, "li", "class", "posted");

            return DateTime.Parse(startNode.First().ChildNodes.ToList()[4].InnerHtml.Replace("\n", string.Empty));
        }

        private static DateTime GetEndDate(HtmlDocument doc)
        {
            var startNode = GetNodesFor(doc, "span", "id", "project_duration_data");
            var x = startNode.First().Attributes.First(a => a.Name == "data-end_time");
           
            return DateTime.Parse(x.Value);
        }

        private static bool GetFundingSucceeded(HtmlDocument doc)
        {
            var bannerNode = GetNodesFor(doc, "div", "id", "funding-successful-banner");

            return bannerNode.Any();
        }

        private static Uri GetLink(HtmlDocument doc)
        {
            var descriptionNode = doc.DocumentNode
                               .Descendants("meta")
                               .Where(d => d.Attributes.Contains("property") && d.Attributes["property"].Value.Contains("og:url"));

            return new Uri(descriptionNode.First().Attributes["content"].Value);
        }

        private static int GetFundingGoal(HtmlDocument doc)
        {
            var fundingNode = GetNodesFor(doc, "div", "id", "pledged");

            return (int)decimal.Parse(fundingNode.First().Attributes["data-goal"].Value);
        }

        private static string GetCurrency(HtmlDocument doc)
        {
            var fundingNode = doc.DocumentNode
                                 .Descendants("data")
                                 .Where(d => d.Attributes.Contains("data-currency"));

            return fundingNode.First().Attributes["data-currency"].Value;
        }

        private static decimal GetFunding(HtmlDocument doc)
        {
            var fundingNode = GetNodesFor(doc, "div", "id", "pledged");

            return decimal.Parse(fundingNode.First().Attributes["data-pledged"].Value);
        }

        private static int GetTotalProjectBackers(HtmlDocument doc)
        {
            var backerNode = GetNodesFor(doc, "div", "id", "backers_count");

            return int.Parse(backerNode.First().Attributes["data-backers-count"].Value);
        }

        private static string GetCompany(HtmlDocument doc)
        {
            var companyNode = GetNodesFor(doc, "a", "data-modal-class", "modal_project_by");

            return companyNode.First().InnerText;
        }

        private static string GetProjectDescription(HtmlDocument doc)
        {
            var descriptionNode = GetNodesFor(doc, "meta", "property", "og:description");

            return descriptionNode.First().Attributes["content"].Value;
        }

        private static string GetName(HtmlDocument doc)
        {
            var titleNode = GetNodesFor(doc, "meta", "property", "og:title");

            return titleNode.First().Attributes["content"].Value;
        }

        private static IEnumerable<BackingLevel> GetLevels(HtmlDocument doc, Currency currency)
        {
            var rewardNode = GetNodesFor(doc, "div", "class", "NS-projects-reward");

            foreach (var htmlNode in rewardNode)
            {
                var level = new BackingLevel(currency);

                level.Money = GetMoney(htmlNode);

                level.Backers = GetBackers(htmlNode);

                SetBackersAllowedValues(htmlNode, level);

                level.IsSoldOut = GetIsSoldOut(htmlNode);

                if (level.IsSoldOut) level.MaxBackersAllowed = level.Backers;

                level.Description = GetDescription(htmlNode);

                yield return level;
            }
        }

        private static string GetDescription(HtmlNode htmlNode)
        {
            var limitBackersNode = GetNodesFor(htmlNode, "div", "class", "desc");

            return limitBackersNode.First().Descendants().Skip(1).First().InnerText;
        }

        private static void SetBackersAllowedValues(HtmlNode htmlNode, BackingLevel level)
        {
            var limitBackersNode = GetNodesFor(htmlNode, "span", "class", "limited-number").ToList();

            if (limitBackersNode.Any())
            {
                // Format: (3 of 6 left)
                level.RemainingBackersAllowed = int.Parse(limitBackersNode.First().InnerText.Split(' ')[0].Replace("(", string.Empty));
                level.MaxBackersAllowed = int.Parse(limitBackersNode.First().InnerText.Split(' ')[2]);
            }
        }

        private static bool GetIsSoldOut(HtmlNode htmlNode)
        {
            var soldOutNode = GetNodesFor(htmlNode, "span", "class", "sold-out");

            return soldOutNode.Any();
        }

        private static int GetBackers(HtmlNode htmlNode)
        {
            var numberOfBackersNode = GetNodesFor(htmlNode, "span", "class", "num-backers");

            return int.Parse(numberOfBackersNode.First().InnerText.Split(' ')[0]);
        }

        private static int GetMoney(HtmlNode htmlNode)
        {
            var h5Node = htmlNode.Descendants("h5");

            // Format: Pledge $5,000 or more
            string numberString =
                new Regex(@"[0-9]+(,[0-9]+)*").Match(h5Node.First().InnerText).Groups[0].ToString().Replace(",", string.Empty);

            return int.Parse(numberString);
        }
    }
}
