namespace AP.PhoneService;
using AP.Accounting;
//服务账户
public class ServiceAccount : DetailAccount{
    public ServiceAccount(string name) : base(name){}

    //为特定的电话服务查找一个特定 汇总账户下的服务账户
    public static ServiceAccount? FindWithPhoneServcie(PhoneService phoneService
                                    ,SummaryAccount summaryAccount){
        foreach (ServiceAccount account in phoneService.ServiceAccounts)
        {
            if(account.ParentAccount == summaryAccount){
                return account;
            }          
        }
        return null;
    }    

}
//电话服务
public class PhoneService{
    public List<ServiceAccount> ServiceAccounts =[];

    //为当前电话服务寻找一个服务账户
    public ServiceAccount FindServiceAccountBySummaryAccountName(String name){
        SummaryAccount? account= IAccount.FindByName(name);
        if(account == null) throw new Exception("Summary account not found");
        return ServiceAccount.FindWithPhoneServcie(this, account);
    }
}