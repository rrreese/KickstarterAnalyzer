﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Scraper;

namespace Tests
{
    [TestClass]
    public class SpiderTests
    {
        [Ignore]
        [TestMethod]
        public void SpiderTest()
        {
            var spider = new Spider();
            var links = spider.GetLinks("/discover/categories/video%20games/most-funded");

            
        }
    }
}
