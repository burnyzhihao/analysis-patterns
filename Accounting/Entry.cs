using System.Data;

namespace AP.Accounting;

public class Entry
{
    public Entry(Decimal amount, DateTime charged, DetailAccount account)
    {
        this.Account = account;
        this.Amount = amount;
        this.WhenCharged = charged;
        account.AddEntry(this);
    }
    public decimal Amount { get; }
    public DateTime WhenCharged { get; }
    public DetailAccount Account { get; }
}