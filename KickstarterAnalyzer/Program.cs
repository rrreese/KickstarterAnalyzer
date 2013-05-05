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
            scraper.Links = spider.GetLinks("/discover/categories/video%20games/most-funded");
            scraper.Download();

            Console.WriteLine("Downloaded and scraped:");
            foreach (var project in scraper.Projects)
            {
                Console.WriteLine(project.Name);    
            }

            Console.WriteLine("Please Enter Output file name:");
            var filename = Console.ReadLine();

            scraper.Output(new CSV(), filename);
        }
    }
}
