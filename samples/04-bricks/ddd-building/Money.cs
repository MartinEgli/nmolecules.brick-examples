using System;

namespace Samples.Block04.Bricks.DddBuilding;

[DddValueObject]
internal readonly record struct Money(decimal Amount, string Currency)
{
    public static Money Zero(string currency) => new(0m, currency);

    public Money Add(Money other)
    {
        if (!string.Equals(Currency, other.Currency, StringComparison.Ordinal))
        {
            throw new InvalidOperationException("Money values must use the same currency.");
        }

        return new Money(Amount + other.Amount, Currency);
    }
}
