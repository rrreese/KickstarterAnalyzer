using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scraper
{
    public class Stats
    {
        private IEnumerable<Project> Projects { get; set; }

        public Stats(IEnumerable<Project> projects)
        {
            this.Projects = projects;
        }

        public decimal TotalProject()
        {
            return this.Projects.Sum(project => project.TotalFunding);
        }

        public decimal AverageProject()
        {
            return this.TotalProject() / this.Projects.Count();
        }

        public Dictionary<int,decimal> AverageBin()
        {
            int[] standardBins = { 10, 35, 100, 250, 500, 1000, 5000, 10000 };

            var binTotals = standardBins.ToDictionary<int, int, decimal>(bin => bin, bin => 0);

            foreach (Project project in this.Projects)
            {
                var bins = project.Binify(standardBins);

                foreach (var bin in bins)
                {
                    binTotals[bin.End] += bin.Total;
                }
            }

            foreach (var binTotal in binTotals)
            {
                binTotals[binTotal.Key] = binTotal.Value/this.Projects.Count();
            }

            return binTotals;
        }
    }
}
