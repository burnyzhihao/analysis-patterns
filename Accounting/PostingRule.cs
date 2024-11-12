using System.ComponentModel;
using AP.PhoneService;

namespace AP.Accounting;
//过账规则
public abstract class PostingRule() {
    //触发者账户
    public IAccount? TriggerAccount{get; protected set;}
    //输出账户
    public Dictionary<String,IAccount>? OutputAccounts{get; protected set;}

    //过账处理的当前账户
    protected IAccount? currentInputAccount;
    //过账处理的输出账户
    protected Dictionary<String,IAccount> CurrentOutputAccounts=>[];

    //处理过账
    
    public abstract void ProcessAccount(DetailAccount account);
 }
//逐分录过账规则
public abstract class EachEntryPR : PostingRule
{
    public EachEntryPR(IAccount trigger, Dictionary<String,IAccount> output) {}

    //设置当前处理账户
    private void SetCurrentInputAccount(IAccount account){
        this.currentInputAccount = account;
        this.SetCurrentOutputAccount();
    }
    //设置当前处理的输出账户
    private void SetCurrentOutputAccount(){
        //对当前过账规则的每个输出账户（应为汇总账户），查找对应的服务账户（明细账户），添加到输出账户列表
        //使用输入账户关联的电话服务匹配查找
        foreach(String key in OutputAccounts!.Keys){
            if(OutputAccounts.ContainsKey(key)){
            SummaryAccount summary = (SummaryAccount)OutputAccounts[key];
            IAccount? account = ServiceAccount.FindWithPhoneServcie
                    (((ServiceAccount)currentInputAccount!).PhoneService
                    ,summary);

            this.OutputAccounts.Add(key,account!);
            
            }
           
        }
    }
    //处理过账
    public override void  ProcessAccount(DetailAccount account){
        SetCurrentInputAccount(account);
        account.GetUnprecessedEntry().ForEach(entry=> ProcessEntry(entry));
    }
    //处理每个分录
    public abstract void ProcessEntry(Entry entry);

}
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