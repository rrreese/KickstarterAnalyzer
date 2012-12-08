using System;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests
{
    [TestClass]
    public class ScraperTests
    {
        private Scraper.Scraper scraper;

        [TestInitialize]
        public void Setup()
        {
            scraper = new Scraper.Scraper();

            scraper.Links.Add(new Uri( Directory.GetCurrentDirectory() + @"\..\..\Sample.html"));
            scraper.Download();
        }

        [TestMethod]
        public void TestLevelCount()
        {
             Assert.AreEqual(scraper.Projects.First().Levels.Count, 6);
        }

        [TestMethod]
        public void TestLevel5()
        {
            var level = scraper.Projects.First().Levels.First();
            Assert.AreEqual(level.Money, 5);
            Assert.AreEqual(level.Backers, 317);
            Assert.AreEqual(level.IsSoldOut, false);
            Assert.AreEqual(level.Description, "5 unlimited description");
        }

        [TestMethod]
        public void TestLevel1000()
        {
            var level = scraper.Projects.First().Levels.First(l => l.Money == 1000);
            Assert.AreEqual(level.Money, 1000);
            Assert.AreEqual(level.Backers, 49);
            Assert.AreEqual(level.RemainingBackersAllowed, 1);
            Assert.AreEqual(level.MaxBackersAllowed, 50);
            Assert.AreEqual(level.IsSoldOut, false);
            Assert.AreEqual(level.Description, "5 unlimited description");
        }

        [TestMethod]
        public void TestLevel6000()
        {
            var level = scraper.Projects.First().Levels.First(l => l.Money == 6000);
            Assert.AreEqual(level.Money, 6000);
            Assert.AreEqual(level.Backers, 5);
            Assert.AreEqual(level.RemainingBackersAllowed, 0);
            Assert.AreEqual(level.MaxBackersAllowed, 5);
            Assert.AreEqual(level.IsSoldOut, true);
            Assert.AreEqual(level.Description, "6000 Tier Description Sold Out");
        }
    }
}
