using System.ComponentModel;

namespace Accounting;
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


