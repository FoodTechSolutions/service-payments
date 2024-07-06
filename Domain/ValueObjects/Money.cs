namespace Domain.ValueObjects;

public class Money
{
    public decimal Amount { get; private set; }
    public string Currency { get; private set; }

    public Money(decimal amount, string currency)
    {
        switch (amount)
        {
            case < 0:
                throw new ArgumentException("Amount must be greater than 0.");
            case 0:
                throw new ArgumentException("Amount must be greater than 0.");
        }

        Amount = amount;
        Currency = currency;
    }
}
