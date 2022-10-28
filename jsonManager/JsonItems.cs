namespace JsonItems;

using System;

/// <summary>
/// Elemento de Json base para almacenar cartas en disco
/// </summary>
public abstract class CardJsonItem{
    public string name{ get; set; }
    public string description{ get; set; }
    public float appearingProbability{get;set;}
    public string element{get;set;}
        
    public CardJsonItem(string name, string description, float appearingProbability, string element){
        this.name = name;
        this.description = description;
        this.appearingProbability = appearingProbability;
        this.element = element;
    }

    public override string ToString()
    {
        return this.name;
    }
}

/// <summary>
/// Elemento especializado para guardar una carta de monstruo
/// </summary>
public class MonsterCardJsonItem : CardJsonItem{
    public int attack{ get; set; }
    public int hp{ get; set; }

    public MonsterCardJsonItem(string name,
                           string description, 
                           float appearingProbability,
                           string element,
                           int attack,
                           int hp) : base(name, description, appearingProbability, element)
    {
        this.attack = attack;
        this.hp = hp;
    }
}


/// <summary>
/// Elemento especializado para guardar una carta de efecto
/// </summary>
public class EffectCardJsonItem : CardJsonItem{
    public string powerName{get;set;}
    public string powerCondition{get;set;}

    public EffectCardJsonItem(string name,
                           string description, 
                           float appearingProbability,
                           string element,
                           string powerName,
                           string powerCondition) : base(name, description, appearingProbability, element)
    {
        this.powerCondition = powerCondition;
        this.powerName = powerName;
    }
}
