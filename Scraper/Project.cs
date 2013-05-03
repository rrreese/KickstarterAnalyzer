using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scraper
{
    public enum Currency
    {
        USD,
        GBP,
        EUR
    }

    [DebuggerDisplay("{Name} - {Levels.Count} - Funding Succeeded: {FundingSucceeded}")]
    public class Project
    {
        public static readonly IEnumerable<int> StandardBins = new[] { 10, 35, 100, 250, 500, 1000, 5000, 10000 };

        public Project()
        {
            this.Levels = new List<BackingLevel>();
        }

        public string Name { get; set; }

        public bool FundingSucceeded { get; set; }

        public Uri Link { get; set; }

        public List<BackingLevel> Levels { get; set; }

        public Currency Currency { get; set; }

        public string Company { get; set; }

        public int FundingGoal { get; set; }

        public int Backers { get; set; }

        public decimal TotalFunding { get; set; }

        public string Description { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public string Category { get; set; }

        /// <summary>
        /// Takes a range of bins and places funding from the actual backer level
        /// into a bin of backer levels. This allows easier comparison between projects
        /// that use different ranges of backer levels.
        /// </summary>
        /// <param name="bins"></param>
        public IEnumerable<Bin> Binify(IEnumerable<int> bins)
        {
            int lastBin = 0;
            foreach (var bin in bins)
            {
                var binSum = this.Levels.Where(lv => lv.MoneyUSD <= bin && lv.Money > lastBin).Sum(lv => lv.TotalFunding);

                yield return new Bin { Start = lastBin, End = bin, Total = binSum };

                lastBin = bin;
            }
        }

        [DebuggerDisplay("{Start}-{End} {Total}")]
        public class Bin
        {
            public int Start { get; set; }

            public int End { get; set; }

            public decimal Total { get; set; }
        }
    }
}
