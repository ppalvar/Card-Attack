namespace JsonItems;

using System;

/// <summary>
/// Elemento de Json base para almacenar cartas en disco
/// </summary>
public abstract class CardItem{
    public string name{ get; set; }
    public string description{ get; set; }
    public float appearingProbability{get;set;}
        
    public CardItem(string name, string description, float appearingProbability){
        this.name = name;
        this.description = description;
        this.appearingProbability = appearingProbability;
    }

    public override string ToString()
    {
        return this.name;
    }
}

/// <summary>
/// Elemento especializado para guardar una carta de monstruo
/// </summary>
public class MonsterCardItem : CardItem{
    public int attack{ get; set; }
    public int hp{ get; set; }

    public MonsterCardItem(string name,
                           string description, 
                           float appearingProbability,
                           int attack,
                           int hp) : base(name, description, appearingProbability)
    {
        this.attack = attack;
        this.hp = hp;
    }
}


/// <summary>
/// Elemento especializado para guardar una carta de efecto
/// </summary>
public class EffectCardItem : CardItem{
    public int attack{ get; set; }
    public int hp{ get; set; }

    public EffectCardItem(string name,
                           string description, 
                           float appearingProbability) : base(name, description, appearingProbability)
    {
        //TODO
    }
}
