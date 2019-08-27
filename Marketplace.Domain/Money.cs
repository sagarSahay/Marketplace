namespace Marketplace.Domain
{
    using System;
    using Framework;

    public class Money : Value<Money>
    {
        private readonly ICurrencyLookup currencyLookup;
        private const string DefaultCurrency = "EUR";

        protected Money(decimal amount, string currencyCode , ICurrencyLookup currencyLookup)
        {
            if (string.IsNullOrEmpty(currencyCode))
            {
                throw new ArgumentNullException(nameof(currencyCode), "Currency code must be specified");
            }

            this.currencyLookup = currencyLookup;

            var currency = currencyLookup.FindCurrency(currencyCode);

            if (!currency.InUse)
            {
                throw new ArgumentException($"Currency {currencyCode} is not valid.");
            }
            
            if (Math.Round(amount, currency.DecimalPlaces) != amount)
            {
                throw new ArgumentOutOfRangeException(nameof(amount), $"Amount cannot have more than {currency.DecimalPlaces} decimals.");
            }

            Amount = amount;
            CurrencyCode = currencyCode;
            this.currencyLookup = currencyLookup;
        }

        protected Money(decimal amount, string currencyCode)
        {
            Amount = amount;
            CurrencyCode = currencyCode;
        }

        public string CurrencyCode { get; }

        public decimal Amount { get; }

        public Money Add(Money summand)
        {
            if (CurrencyCode != summand.CurrencyCode)
            {
                throw new CurrencyMismatchException("Cannot sum amount with different currencies");
            }

            return new Money(Amount + summand.Amount,CurrencyCode, currencyLookup );
        }

        public Money Subtract(Money subtrahend)
        {
            if (CurrencyCode != subtrahend.CurrencyCode)
            {
                throw new CurrencyMismatchException("Cannot subtract amount with different currencies");
            }

            return new Money(Amount - subtrahend.Amount, CurrencyCode, currencyLookup);
        }

        public static Money operator +(Money summand1, Money summand2) => summand1.Add(summand2);

        public static Money operator -(Money subtrahend1, Money subtrahend2) => subtrahend1.Subtract(subtrahend2);

        public static Money FromDecimal(decimal amount, 
            string currency, 
            ICurrencyLookup currencyLookup) =>
            new Money(amount, currency, currencyLookup);

        public static Money FromString(string amount, 
            string currency , 
            ICurrencyLookup currencyLookup) =>
            new Money(decimal.Parse(amount), currency, currencyLookup);
    }

    public class CurrencyMismatchException : Exception
    {
        public CurrencyMismatchException(string message) : base(message)
        {
        }
    }
}