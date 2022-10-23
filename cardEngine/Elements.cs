namespace Elements;

using System;
using Cards;

public abstract class Element{
    public string Type{get;private set;}

    protected Element (string Type){
        this.Type = Type;
    }
}

public class Fire : Element{
    public Fire() : base("Fire"){}
}

public class Water : Element{
    public Water() : base("Water") {}
}

public class Wind : Element{
    public Wind() : base("Wind") {}
}

public class Earth : Element{
    public Earth() : base("Earth") {
    }
}