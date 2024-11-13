using System;
using Accounting;

namespace PhoneService;
//按月过账的过账规则，用于计税
public class MonthlyChargePR : PostingRule
{
    //按月过账
    public override void ProcessAccount(DetailAccount account)
    {
        this.currentInputAccount = account;
        //处理每个月的过账
        foreach(int month in MonthsToProcess(account)){
            ProcessForMonth(month);
        }
    }
    //处理给定月份的过账
    private void ProcessForMonth(int month)
    {
        throw new NotImplementedException();
    }

    //获取账户里存在未处理的分录的月份集合
    private HashSet<int> MonthsToProcess(DetailAccount account){
        return account.GetUnprecessedEntry().Select(entry=>entry.WhenCharged.Hour).ToHashSet();
    }
    

}
