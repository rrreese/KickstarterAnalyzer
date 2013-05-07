using System;
using System.IO;
using System.Xml;

namespace Scraper
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml.Linq;

    public class XML : IOutput
    {
        public IEnumerable<Project> Projects { get; set; }
        public string Generate()
        {
            XDocument xdoc = GenerateXDoc(this.Projects);


            return xdoc.ToString();
        }

        private static XDocument GenerateXDoc(IEnumerable<Project> projects)
        {
            XDocument xdoc = new XDocument();
            var rootElement = new XElement("projects");
            xdoc.Add(rootElement);
            foreach (var project in projects)
            {
                rootElement.Add(
                    new XElement("project",
                                 new XElement("name", project.Name),
                                 new XElement("fundingSucceeded", project.FundingSucceeded),
                                 new XElement("link", project.Link),
                                 new XElement("currency", project.Currency),
                                 new XElement("company", project.Company),
                                 new XElement("fundingGoal", project.FundingGoal),
                                 new XElement("backers", project.Backers),
                                 new XElement("totalFunding", project.TotalFunding),
                                 new XElement("description", project.Description),
                                 new XElement("startDate", project.StartDate),
                                 new XElement("endDate", project.EndDate),
                                 new XElement("category", project.Category),
                                 new XElement("levels", project.Levels.Select(
                                     x => new XElement("level",
                                                       new XElement("backers", x.Backers),
                                                       new XElement("maxBackersAllowed", x.MaxBackersAllowed),
                                                       new XElement("remainingBackersAllowed", x.RemainingBackersAllowed),
                                                       new XElement("isSoldOut", x.IsSoldOut),
                                                       new XElement("description", x.Description),
                                                       new XElement("totalFunding", x.TotalFunding),
                                                       new XElement("money", x.Money),
                                                       new XElement("moneyUSD", x.MoneyUSD))))));
            }

            return xdoc;
        }

        public void Save(string filename)
        {
            var xdoc = GenerateXDoc(this.Projects);



            XmlWriterSettings settings = new XmlWriterSettings();
            settings.CheckCharacters = false;
            using (XmlWriter writer = XmlWriter.Create(Path.Combine(filename), settings))
            {
                xdoc.Save(writer);
            }

        }

        public void Load(string testXML)
        {
            var xdoc = XDocument.Load(testXML);

            this.Projects = xdoc.Descendants("project")
                                .Select(projectElement => new Project
                {
                    TotalFunding = decimal.Parse(projectElement.Elements("totalFunding").First().Value)
                });
        }
    }
}