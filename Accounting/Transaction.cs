namespace AP.Accounting;

//会计事项
public class Transaction
{
    public List<Entry> SourceEntries{get;} = [];
    public PostingRule Rule{get;}

    public Transaction(Decimal amount, DetailAccount fromAccount, DetailAccount toAccount
, DateTime charged, PostingRule creator, List<Entry> sources)
    {
        //在创建会计事项时同时创建两条分录
        sources.Add(new Entry(amount,charged,toAccount));
        sources.Add(new Entry(amount * (-1), charged, fromAccount));
        this.Rule = creator;
        if(!CheckInvariant()){
            throw new Exception("Sum of entries is not zero");
        }
    }
    //检查分录amount之和为零
    private Boolean CheckInvariant(){
        return SourceEntries.Sum(entry=>entry.Amount) == 0;
    } 
}

public class PostingRule {
    
 }