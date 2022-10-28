namespace Powers;

using IPowers;
using Elements;
using System.Reflection;

public static class PowersManager {
    static public Type[] allPowers{get;private set;} = GetPowers();

    public static IPower? GetPower(string Name, string Conditions) {
        foreach (Type p in allPowers) {
            if (p.Name == Name)return GetPower(p, Conditions);
        }return null;
    }

    public static IPower GetPower(Type power, string Conditions){
        Object? g_power = Activator.CreateInstance(power, Conditions);
        
        if (g_power != null){
            IPower? p = g_power as IPower;
            if (p != null)return p;
        }

        throw new Exception("can't create an instance of this power");
    }

    private static Type[] GetPowers(){
        Assembly assembly = Assembly.GetExecutingAssembly();

        return assembly.GetTypes()
               .Where(type => type.Namespace != null && "Powers" == type.Namespace.ToString())
               .Where(name => !name.Name.Contains("PowersManager")).SkipLast(1).ToArray();
    }
}

public class FireBall : IPower{
    public Element Element{get;} = new Fire();
    
    public string Conditions{get;set;}
    public string Name{get;set;}

    public FireBall(string Conditions){
        this.Conditions = Conditions;
        this.Name = nameof(FireBall);
    }
    public void ApplyEffect(){
        
    }
}

public class Splash : IPower{
    public Element Element{get;} = new Water();

    public string Conditions{get;set;}
    public string Name{get;set;}

    public Splash(string Conditions){
        this.Conditions = Conditions;
        this.Name = nameof(Splash);
    }
    public void ApplyEffect(){
        
    }
}
