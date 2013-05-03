using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Scraper;

namespace Tests
{
    [TestClass]
    public class StatsTests
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
        public void TestAverages()
        {
            var stats = new Stats(scraper.Projects);
            Assert.AreEqual(stats.AverageProject(), 4666666.6666666666666666666667m);
            Assert.AreEqual(stats.TotalProject(),14000000);
        }

        [DeploymentItem("Sample.html")]
        [DeploymentItem("Sample2.html")]
        [DeploymentItem("Sample3.html")]        
        [TestMethod]
        public void TestBin()
        {
            var stats = new Stats(scraper.Projects);

            var averageBins = stats.AverageBin();

            Assert.AreEqual(averageBins.Count, 8);
            Assert.AreEqual(averageBins[35],97930);
            Assert.AreEqual(averageBins[100], 311986);
            Assert.AreEqual(averageBins[10000], 30000);
        }
    }
}
