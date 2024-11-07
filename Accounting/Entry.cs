using System.Data;

namespace AP.Accounting;

public class Entry(Decimal amount, DateTime charged, DetailAccount account)
{
    public decimal Amount => amount;
    public DateTime WhenCharged => charged;
    public DetailAccount Account => account;
}