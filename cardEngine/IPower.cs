namespace IPowers;

using Elements;

public interface IPower {
    string Conditions{get;set;}
    string Name{get;set;}
    Element Element{get;}
    public void ApplyEffect();
}