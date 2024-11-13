
namespace Accounting;

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


