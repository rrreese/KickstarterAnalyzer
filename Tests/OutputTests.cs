namespace Tests
{
    using System;
    using System.IO;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Scraper;
    using Utilities;

    [TestClass]
    public class OutputTests
    {
        private Scraper scraper;

        [TestInitialize]
        public void Setup()
        {
            this.scraper = new Scraper
                {
                    Links = new[]
                        {
                            new Uri(Directory.GetCurrentDirectory() + @"\Sample.html"),
                            new Uri(Directory.GetCurrentDirectory() + @"\Sample2.html"),
                            new Uri(Directory.GetCurrentDirectory() + @"\Sample3.html")
                        }
                };

            this.scraper.Download();
        }

        [DeploymentItem("Sample.html")]
        [DeploymentItem("Sample2.html")]
        [DeploymentItem("Sample3.html")]
        [TestMethod]
        public void CSVTest()
        {
            var csvOutput = new CSV { Projects = scraper.Projects };
            var csv = csvOutput.Generate();

            Assert.AreEqual(csv.CountString(Environment.NewLine), 3);
        }

        [DeploymentItem("Sample.html")]
        [DeploymentItem("Sample2.html")]
        [DeploymentItem("Sample3.html")]
        [TestMethod]
        public void XMLTest()
        {
            var xmlOutput = new XML { Projects = scraper.Projects };
            var xml = xmlOutput.Generate();

            Assert.IsTrue(xml.Length > 8000);
            Assert.IsTrue(xml.RemoveAllWhiteSpace().StartsWith("<projects><project><name>"));
        }
    }
}
