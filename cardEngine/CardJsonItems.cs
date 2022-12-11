namespace CardJsonItems;

using JsonItems;

/// <summary>
/// Base Json element to store cards in disk
/// </summary>
public abstract class CardJsonItem : IJsonItem{
    public string name{ get; set; }
    public string description{ get; set; }
    public string image{get;set;}
    public float appearingProbability{get;set;}
        
    public CardJsonItem(string name, string description, string image, float appearingProbability){
        this.name = name;
        this.image = image;
        this.description = description;
        this.appearingProbability = appearingProbability;
    }

    public override string ToString()
    {
        return this.name;
    }
}

/// <summary>
/// Json Item Used to store a Monster Card in disk
/// </summary>
public class MonsterCardJsonItem : CardJsonItem{
    public int attack{ get; set; }
    public int hp{ get; set; }

    public MonsterCardJsonItem(string name,
                           string description,
                           string image,
                           float appearingProbability,
                           int attack,
                           int hp) : base(name, description, image, appearingProbability)
    {
        this.attack = attack;
        this.hp = hp;
    }
}


/// <summary>
/// Json Item used to store an Effect Card in disk
/// </summary>
public class EffectCardJsonItem : CardJsonItem{
    public string powerName{get;set;}
    public string powerCode{get;set;}

    public EffectCardJsonItem(string name,
                           string description,
                           string image,
                           float appearingProbability,
                           string powerName,
                           string PowerCode) : base(name, description, image, appearingProbability)
    {
        this.powerName = powerName;
        this.powerCode = PowerCode;
    }
}
