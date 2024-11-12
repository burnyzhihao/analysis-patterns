namespace AP.Accounting;

public class SummaryAccount : IAccount
{
    public SummaryAccount(string name)
    {
        Name = name;
    }
    public List<IAccount> ChildAccounts => [];
    public void AddAccount(IAccount account)
    {
        ChildAccounts.Add(account);
        account.ParentAccount = this;
    }
    public void RemoveAccount(IAccount account)
    {
        account.ParentAccount = null;
        ChildAccounts.Remove(account);
    }

    public void AddEntry(Entry entry) => throw new NotSupportedException();
    public void RemoveEntry(Entry entry) => throw new NotSupportedException();
    public List<Entry> Entries
    {
        get
        {
            List<Entry> totalEntries = new List<Entry>();
            foreach (var account in ChildAccounts)
            {
                totalEntries.AddRange(account.Entries);
            }
            return totalEntries;
        }
    }
    public IAccount? ParentAccount { get => null; set => throw new NotImplementedException(); }
    public string Name { get;set; }

  
}
