using System;
using System.Globalization;

namespace PhoneService;

public abstract class MinutesToMoneyCalculator
{

    protected abstract Dictionary<Decimal,Decimal> Ratetable{get;}
    protected abstract Decimal TopRate{get;}
    //计算费率
    public virtual Decimal CaculateMoneyForMiniute(decimal minute){
        Decimal totalFee = 0m;
        Decimal remainMinutes = minute;
        
        foreach(decimal key in Ratetable.Keys){
            decimal lastKey = 0m;
            if(minute > key){
                totalFee += (key-lastKey) * Ratetable[key];
                remainMinutes = minute - key;
            }else{
                totalFee += minute * Ratetable[key];
                break;
            }
        }
 
        //超出费率表部分
        if(remainMinutes>0){
            totalFee += remainMinutes * TopRate;
        }
        return totalFee;
    }
    
}
//日间费率计算
public class DayRateCaculator : MinutesToMoneyCalculator
{
    protected override Dictionary<decimal, decimal> Ratetable =>
            new Dictionary<decimal, decimal>(){
                {1m, 0.9m}
            };

    protected override decimal TopRate => 0.3m;

}
//夜间费率计算
public class EveningRateCaculator : MinutesToMoneyCalculator
{
    protected override Dictionary<decimal, decimal> Ratetable =>
            new Dictionary<decimal, decimal>(){
                {1m, 0.9m}, //一分钟之内 0.9元
                {2m,0.7m}, //一到两分钟 0.7元
            };
    //超出两分钟 0.2元
    protected override decimal TopRate => 0.2m;

}

