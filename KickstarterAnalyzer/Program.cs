using System.Linq;

namespace KickstarterAnalyzer
{
    using System;
    using Scraper;

    class Program
    {
        static void Main(string[] args)
        {
            var scraper = new Scraper();

            var spider = new Spider();

            scraper.Links = spider.GetLinks("discover/advanced?category_id=12&woe_id=0&sort=most_funded");

            scraper.Links = scraper.Links;//.Take(10); //Todo: add top x option
            
            scraper.Download();

            Console.WriteLine("Downloaded and scraped:");
            foreach (var project in scraper.Projects)
            {
                Console.WriteLine(project.Name);    
            }

            Console.WriteLine("Please Enter Output file name:");
            var filename = Console.ReadLine();

            scraper.Save(new XML(), filename + ".xml");
            scraper.Save(new CSV(), filename + ".csv");
        }
    }
}
