using System.Reflection.Metadata;

namespace Accounting;

public class DetailAccount : IAccount
{ 
    public DetailAccount(string name)
    {
        Name = name;
    }
    public Entry? LastProcessed{get; private set;}

    public void AddAccount(IAccount account) => throw new NotSupportedException();
    public void RemoveAccount(IAccount account) => throw new NotSupportedException();
    public List<Entry> Entries => new List<Entry>(Entries);
    public List<IAccount> ChildAccounts => [];

    public IAccount? ParentAccount { get ; set; }
    public string Name { get;set; }

    public void AddEntry(Entry entry) => Entries.Add(entry);
    public void RemoveEntry(Entry entry) => Entries.Remove(entry);

    //返回以当前账户为触发者账户的所有过账规则
    private List<PostingRule> AllOutboundRules(){
        throw new NotImplementedException("尚未实现");
    }

    //基于当前账户触发过账
    public void Process(){
        //对所有以当前账户为触发者的过账规则，处理过账
        this.AllOutboundRules().ForEach(r=>r.ProcessAccount(this));
        //记录最后一笔处理的分录
        LastProcessed = this.Entries[this.Entries.Count-1];
    }

    // 获取第一个未处理分录的索引
    private int GetFirstUnprocessedEntryIndex(){
        return LastProcessed == null? 0
                : Entries.IndexOf(LastProcessed) + 1 ;

    }
    //当前账户是否都没处理
    private bool IsUnprocessed(){
        return LastProcessed == null;
    }

    //获取当前账户未处理的分录
    public List<Entry> GetUnprecessedEntry(){
        return IsUnprocessed() ? new List<Entry>(Entries):
            new List<Entry>(Entries[GetFirstUnprocessedEntryIndex()..]);
                
    }

}
