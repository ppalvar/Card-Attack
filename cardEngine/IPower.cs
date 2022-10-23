namespace IPowers;

using Elements;

public interface IPower {
    string Conditions{get;set;}
    Element Element{get;}
    public void ApplyEffect();
}