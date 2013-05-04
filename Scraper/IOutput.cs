using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Scraper
{
    public interface IOutput
    {
        string Generate();

        void Save(string filename);
    }
}
