
namespace AP.Accounting;

public interface IAccount
{
    public String Name {get;set;}
    //保存定义的高级汇总账户
    public static List<SummaryAccount> SummaryAccounts =>[];
    public static SummaryAccount? FindByName(String name){
        return SummaryAccounts.Find(a=>a.Name == name);
    }
    void AddEntry(Entry entry);
    void RemoveEntry(Entry entry);
    void AddAccount(IAccount account);
    void RemoveAccount(IAccount account);
    List<Entry> Entries { get; }
    IAccount? ParentAccount{get;set;}
    List<IAccount> ChildAccounts{get;}
}

public class DetailAccount : IAccount
{ 
    public DetailAccount(string name)
    {
        Name = name;
    }

    public void AddAccount(IAccount account) => throw new NotSupportedException();
    public void RemoveAccount(IAccount account) => throw new NotSupportedException();
    public List<Entry> Entries => new List<Entry>(Entries);
    public List<IAccount> ChildAccounts => [];

    public IAccount? ParentAccount { get ; set; }
    public string Name { get;set; }

    public void AddEntry(Entry entry) => Entries.Add(entry);
    public void RemoveEntry(Entry entry) => Entries.Remove(entry);
}


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

