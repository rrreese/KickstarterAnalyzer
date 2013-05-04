using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Scraper;
using Utilities;

namespace Tests
{
    [TestClass]
    public class OutputTests
    {
        private Scraper.Scraper scraper;

        [TestInitialize]
        public void Setup()
        {
            this.scraper = new Scraper.Scraper();

            this.scraper.Links.Add(new Uri(Directory.GetCurrentDirectory() + @"\Sample.html"));
            this.scraper.Links.Add(new Uri(Directory.GetCurrentDirectory() + @"\Sample2.html"));
            this.scraper.Links.Add(new Uri(Directory.GetCurrentDirectory() + @"\Sample3.html"));
            this.scraper.Download();
        }

        [DeploymentItem("Sample.html")]
        [DeploymentItem("Sample2.html")]
        [DeploymentItem("Sample3.html")]
        [TestMethod]
        public void CSVTest()
        {
            var csvOutput = new CSV(scraper.Projects);
            var csv = csvOutput.Generate();

            Assert.AreEqual(csv.CountString(Environment.NewLine), 3);
        }
    }
}
