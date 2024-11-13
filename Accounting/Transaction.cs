namespace Accounting;

//会计事项
public class Transaction
{
    public List<Entry> SourceEntries{get;} = [];
    public PostingRule? Rule{get;}

    private Transaction(Decimal amount, DetailAccount fromAccount, DetailAccount toAccount
                            , DateTime charged, PostingRule? creator, List<Entry> sources)
    {
        //在创建会计事项时同时创建两条分录
        sources.Add(new Entry(amount,charged,toAccount));
        sources.Add(new Entry(amount * (-1), charged, fromAccount));
        this.Rule = creator;
        if(!CheckInvariant()){
            throw new Exception("Sum of entries is not zero");
        }
    }
    //工厂，创建一个会计事项
    public static Transaction NewWithAmount(Decimal amount, DetailAccount fromAccount, DetailAccount toAccount
                            ,PostingRule creator, DateTime charged,List<Entry> sources)
    {
        return new Transaction(amount,fromAccount,toAccount,charged,creator,sources);
    }
    //工厂，创建一个无分录会计事项
    public static Transaction NewWithAmount(Decimal amount, DetailAccount fromAccount, DetailAccount toAccount
                            , DateTime charged)
    {
        return new Transaction(amount,fromAccount,toAccount,charged,null,[]);
    }
    //检查分录amount之和为零
    private Boolean CheckInvariant() => SourceEntries.Sum(entry => entry.Amount) == 0;
}

