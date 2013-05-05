namespace Scraper
{
    using System.Collections.Generic;

    public interface IOutput
    {
        IEnumerable<Project> Projects { get; set; }

        string Generate();

        void Save(string filename);
    }
}
