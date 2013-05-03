﻿using Scraper;

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

            this.scraper.Links.Add(new Uri(Directory.GetCurrentDirectory() + @"\Sample.html"));
            this.scraper.Links.Add(new Uri(Directory.GetCurrentDirectory() + @"\Sample2.html"));
            this.scraper.Links.Add(new Uri(Directory.GetCurrentDirectory() + @"\Sample3.html"));
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
    }
}
