using System.Runtime.CompilerServices;
using AP.Accounting;

namespace AP.PhoneService;
public class UseCases{
    public static void Main(String[] args){
        Console.WriteLine("hello");
    }

    //建立通话：建模为从“network”账户到“基本时间”账户的一笔会计事项,entry 单位是分钟
    public static void SetupCalls(){
        Customer customer = new Customer("adam");
        //新建电话服务
        PhoneService theService = PhoneService.createNewInstance(
            AccountingPractice.BasicBillingPaln,customer,"1234567");
        //从“network”的汇总账户中，返回一个服务账户
        ServiceAccount networkAccount = theService.FindServiceAccountBySummaryAccountName("network");
        //从“基本时间”汇总账户中，返回一个服务账户
        ServiceAccount? basicAccount = ServiceAccount.FindWithPhoneServcie(theService, IAccount.FindByName("basic time")!);
        //创建通话(会计事项)
        Transaction transaction = Transaction.NewWithAmount(10,networkAccount,basicAccount!,DateTime.Now);
    }


}