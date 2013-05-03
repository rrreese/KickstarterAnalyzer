using System;
using System.Diagnostics;

namespace Scraper
{
    [DebuggerDisplay("{Money} - {Backers}")]
    public class BackingLevel
    {
        private readonly Currency currency;

        public BackingLevel(Currency currency)
        {
            this.currency = currency;
        }

        public int Money { get; set; }
        public decimal MoneyUSD {get { if (this.currency == Currency.USD) return Money;
        else throw new NotImplementedException();
        }}
        public int Backers { get; set; }
        public int MaxBackersAllowed { get; set; }
        public int RemainingBackersAllowed { get; set; }
        public bool IsSoldOut { get; set; }
        public string Description { get; set; }
        public decimal TotalFunding { get { return Money * Backers; } }
    }
}