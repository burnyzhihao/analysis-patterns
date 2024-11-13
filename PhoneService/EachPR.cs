using Accounting;
namespace PhoneService;

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