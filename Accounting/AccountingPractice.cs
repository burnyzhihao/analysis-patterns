namespace AP.Accounting;
//会计务实
public class AccountingPractice{
    //模拟一个预定义的会计务实
    public static AccountingPractice BasicBillingPaln = new AccountingPractice();

    public List<PostingRule> PostingRules{get;set;}=[];

    //返回所有过账规则中的过账账户
    public List<IAccount> GetPostingAccounts(){
        List<IAccount> accounts = [];
        foreach(PostingRule rule in PostingRules){
            accounts.Add(rule.TriggerAccount);
            accounts.AddRange(rule.OutputAccounts.Values);
        }
        return accounts;
    }

    //返回会计务实中的汇总账户
    public List<SummaryAccount> GetSummaryAccounts(){
        List<SummaryAccount> accounts = [];
        GetPostingAccounts().ForEach(account=>{
            if(account.GetType() == typeof(SummaryAccount)){
                SummaryAccount summaryAccount = (SummaryAccount)account;
                accounts.Add(summaryAccount);
            }
        });
        return accounts;
    }












    
}