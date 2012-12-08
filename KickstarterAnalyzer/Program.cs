namespace KickstarterAnalyzer
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    class Program
    {
        static void Main(string[] args)
        {
            var scraper = new Scraper.Scraper();

            // Find pages

            scraper.Links.Add(new Uri("http://www.kickstarter.com/projects/obsidian/project-eternity"));

            // Download Pages
            scraper.Download();

            // Scrape Pages

            // Analyse
        }
    }
}
