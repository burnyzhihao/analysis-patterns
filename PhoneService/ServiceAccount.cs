namespace PhoneService;

using System.Reflection.Emit;
using Accounting;
//服务账户
public class ServiceAccount : DetailAccount{
    private ServiceAccount(String name,PhoneService phoneService,SummaryAccount account) : base(name){
        Name = name;
        ParentAccount = account;
        PhoneService = phoneService;
    }
    public PhoneService PhoneService{get;}
    //工厂
    public static ServiceAccount NewWithPhoneService(String name,PhoneService phoneService,SummaryAccount account){
        return new ServiceAccount(name,phoneService,account);
    }

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
    private PhoneService(AccountingPractice accountingPractice,Customer customer,String phoneLine){
        this.Name = customer.Name + "#" + (customer.PhoneServices.Count +1 ).ToString(); 
        this.accountingPractice = accountingPractice;
        this.Customer = customer;
        this.PhoneLine = phoneLine;
    }

    public static PhoneService createNewInstance(AccountingPractice accountingPractice
        ,Customer customer,String phoneLine)=> new PhoneService(accountingPractice,customer,phoneLine);
    public String Name{get;}
    public String PhoneLine{get;}
    public AccountingPractice accountingPractice{get;}
    public Customer Customer{get;}
    public List<ServiceAccount> ServiceAccounts =[];

    //为当前电话服务寻找一个服务账户
    public ServiceAccount FindServiceAccountBySummaryAccountName(String name){
        SummaryAccount? account= IAccount.FindByName(name);
        if(account == null) throw new Exception("Summary account not found");
        return ServiceAccount.FindWithPhoneServcie(this, account);
    }

    //为会计务实中的每个汇总账户创建一个服务账户
    private void CreateServiceAccounts(){
        this.accountingPractice.GetSummaryAccounts().ForEach(sAccount=>{
            ServiceAccount.NewWithPhoneService("",this,sAccount);
        });
    }
}