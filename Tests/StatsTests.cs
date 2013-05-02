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
            this.scraper.Download();
        }

        [TestMethod]
        public void TestBin()
        {
            var stats = new Stats(scraper.Projects);

            stats.AverageBin();
        }
    }
}
