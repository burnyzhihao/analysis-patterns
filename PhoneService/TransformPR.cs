using System;
using Accounting;

namespace PhoneService;
//根据计时账户分录生成计费账户分录的过账规则
public class TransformPR : EachEntryPR
{
    public TransformPR(IAccount trigger, Dictionary<string, IAccount> output) : base(trigger, output)
    {
        //初始化输出账户(使用预定义的汇总账户)，
        this.OutputAccounts = [];
        //network 账户
        OutputAccounts.Add("network",IAccount.FindByName("network")!);
        //network 收益账户
        OutputAccounts.Add("network_income",IAccount.FindByName("network_income")!);
        //“应收款”账户
        OutputAccounts.Add("activity",IAccount.FindByName("activity")!);
    }
    //处理过账：从计时账户分录生成两个分录
    public override void ProcessEntry(Entry entry)
    {
        //会计事项一： 从当前分录账户 到到network 账户，完成一个分钟时间循环
        Transaction.NewWithAmount(
            entry.Amount,
            entry.Account,
            (ServiceAccount)this.CurrentOutputAccounts["network"],
            this,
            entry.WhenCharged,
            [entry] 
        );
        //会计事项二： 从network收益账户 到 应收账款账户
        Transaction.NewWithAmount(
            this.MiniuteToMoney(entry),
            (ServiceAccount)this.CurrentOutputAccounts["network_income"],
            (ServiceAccount)this.CurrentOutputAccounts["activity"],
            this,
            entry.WhenCharged,
            [entry] 
        );
    }
    //由计时时间装换到费用
    private Decimal MiniuteToMoney(Entry entry){
        //日间
        if(entry.WhenCharged.Hour < 19 && entry.WhenCharged.Hour > 7){
            return (new DayRateCaculator()).CaculateMoneyForMiniute(entry.Amount);
        }
        else{
            return (new EveningRateCaculator()).CaculateMoneyForMiniute(entry.Amount);
        }
    }
}


