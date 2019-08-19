namespace Marketplace.Domain
{
    using System;

    public class Price : Money
    {
        public Price(decimal amount, string currencyCode, ICurrencyLookup currencyLookup)
            : base(amount, currencyCode, currencyLookup)
        {
            if (amount < 0)
            {
                throw new ArgumentException("Price cannot be negative", nameof(amount));
            }
        }

        public Price(decimal ePrice, string eCurrencyCode) : base(ePrice, eCurrencyCode)
        {
            throw new NotImplementedException();
        }

        public static Price FromDecimal(decimal amount, string currencyCode, ICurrencyLookup currencyLookup)
        {
            return new Price(amount, currencyCode, currencyLookup);
        }
    }
}