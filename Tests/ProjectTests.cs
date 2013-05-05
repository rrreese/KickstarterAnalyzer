using System.Collections.Generic;
using Scraper;

namespace Tests
{
    using System;
    using System.IO;
    using System.Linq;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class ProjectTests
    {
        private Scraper.Scraper scraper;

        [TestInitialize]
        public void Setup()
        {
            this.scraper = new Scraper.Scraper();

            this.scraper.Links = new Uri[]
                {
                    new Uri(Directory.GetCurrentDirectory() + @"\Sample.html"),
                    new Uri(Directory.GetCurrentDirectory() + @"\Sample2.html"),
                    new Uri(Directory.GetCurrentDirectory() + @"\Sample3.html")
                };

            this.scraper.Download();
        }

        [DeploymentItem("Sample.html")]
        [DeploymentItem("Sample2.html")]
        [DeploymentItem("Sample3.html")]
        [TestMethod]
        public void TestProject()
        {
            var project = this.scraper.Projects.First();
            Assert.AreEqual(project.Backers, 73986);
            Assert.AreEqual(project.TotalFunding, 3000000m);
            Assert.AreEqual(project.FundingGoal, 1100000);
            Assert.AreEqual(project.Description, "Sample Project is aproject developed by Sample Company.");
            Assert.AreEqual(project.FundingSucceeded, true);
            Assert.AreEqual(project.Name, "Sample Project");
            Assert.AreEqual(project.Company, "Sample Company");
            Assert.AreEqual(project.Currency, Currency.USD);
            Assert.AreEqual(project.Link, "http://www.example.com/link");
            Assert.AreEqual(project.StartDate, DateTime.Parse("Sep 14, 2012"));
            Assert.AreEqual(project.EndDate, DateTime.Parse("Oct 16, 2012"));
            Assert.AreEqual(project.Category, "Video Games");

            Assert.AreEqual(this.scraper.Projects.Count, 3);
        }


        [DeploymentItem("Sample.html")]
        [DeploymentItem("Sample2.html")]
        [DeploymentItem("Sample3.html")]
        [TestMethod]
        public void TestBinning()
        {
            TestBins(this.scraper.Projects[0], new[] { 1585, 293790, 0,0,0,49000,36000,30000});
            TestBins(this.scraper.Projects[1], new[] { 2219,0,513228,0,0,0,130000,30000 });
            TestBins(this.scraper.Projects[2], new[] { 1585, 0, 422730, 0, 0, 0, 98650, 30000 });
        }

        private static void TestBins(Project project, IList<int> testValues)
        {            
            var bins = project.Binify(Project.StandardBins).ToList();

            for (int index = 0; index < bins.Count; index++)
            {
                var bin = bins[index];
                Assert.AreEqual(bin.Total, testValues[index]);                
            }
        }
    }
}
