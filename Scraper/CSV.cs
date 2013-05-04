using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scraper
{
    public class CSV : IOutput
    {
        public const string CSVSeperator = ",";

        private readonly IEnumerable<Project> projects;

        public CSV(IEnumerable<Project> projects)
        {
            this.projects = projects;
        }

        public string Generate()
        {
            var csv = new StringBuilder();

            csv.Append("Name");
            csv.Append(CSVSeperator);
            csv.Append("Total Funding");
            csv.Append(CSVSeperator);
            csv.Append("Funding Goal");
            csv.Append(CSVSeperator);
            csv.Append("Funding Succeeded");
            csv.Append(CSVSeperator);
            csv.Append("Start Date");
            csv.Append(CSVSeperator);
            csv.Append("End Date");
            csv.Append(CSVSeperator);

            foreach (var bin in Project.StandardBins)
            {
                csv.Append(bin);
                csv.Append(CSVSeperator);
            }
            
            foreach (Project project in projects)
            {
                csv.Append(Environment.NewLine);

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
            }

            return csv.ToString();
        }

        public void Save(string filename)
        {
            using (StreamWriter outfile = new StreamWriter(filename))
            {
                outfile.Write(this.Generate());
            }
        }
    }
}
