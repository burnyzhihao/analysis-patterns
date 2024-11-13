using Accounting;

namespace PhoneService;

//昼夜分开的过账规则，  用于处理划分昼夜的通话时间
public class EveningDaySplitPR : EachEntryPR{

    public EveningDaySplitPR(IAccount trigger, Dictionary<string, IAccount> output) 
                            : base(trigger, output)
    {
        //初始化输出账户(使用预定义的汇总账户)，日间时间账户， 夜间时间账户
        this.OutputAccounts = [];
        OutputAccounts.Add("evening",IAccount.FindByName("evening")!);
        OutputAccounts.Add("day",IAccount.FindByName("day")!);
    }
    //处理日间和夜间通话的过账逻辑
    public override void ProcessEntry(Entry entry)
    {
        //产生一笔会计事物并分配分录
        Transaction.NewWithAmount(
            entry.Amount,
            entry.Account,
            this.GetSuitableAccountForPostingEntry(entry),
            this,
            entry.WhenCharged,
            [entry]
        );
    }
    //为分录选择合适的过账账户(服务账户)
    private ServiceAccount GetSuitableAccountForPostingEntry(Entry entry){
        if(entry.WhenCharged.Hour> 19 && entry.WhenCharged.Hour<=7){
            //夜间计时
            return (ServiceAccount)this.CurrentOutputAccounts["evening"];
        }else{
            return (ServiceAccount)this.CurrentOutputAccounts["day"];
        }
    }
}