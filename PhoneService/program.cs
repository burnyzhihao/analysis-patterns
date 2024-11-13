namespace PhoneService;

public class Program{
    public Program(){
        Pro = "hi";
    }

    public string Pro {get; init;} = "hello";

    //public readonly String Test{get;set;}
    public void Test(){
        //Pro = "hi";
        Dictionary<decimal,decimal> t = new Dictionary<decimal, decimal>();
        t.Add(0.1m, 0.23m);
    }

}

public class NewProgram:Program{
    public void NewTest(){
        Program p = new Program(){Pro = "isdf"};

    }
}