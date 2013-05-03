using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scraper
{
    public class CSV
    {
        public const string CSVSeperator = ",";

        private readonly IEnumerable<Project> projects;

        public CSV(IEnumerable<Project> projects)
        {
            this.projects = projects;
        }

        public string GenerateCSVString()
        {
            var csv = new StringBuilder();

            foreach (Project project in projects)
            {
                csv.Append(project.Name);
                csv.Append(CSVSeperator);
                csv.Append(project.TotalFunding);
                csv.Append(CSVSeperator);
                csv.Append(project.FundingGoal);
                csv.Append(CSVSeperator);
                csv.Append(project.FundingSucceeded);
                csv.Append(CSVSeperator);
                csv.Append(project.StartDate);
                csv.Append(CSVSeperator);
                csv.Append(project.EndDate);
                csv.Append(CSVSeperator);

                
                foreach (var bin in project.Binify(Project.StandardBins))
                {
                    csv.Append(bin.Total);
                    csv.Append(CSVSeperator);
                }

                csv.Append(Environment.NewLine);
            }

            return csv.ToString();
        }
    }
}
