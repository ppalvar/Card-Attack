namespace Cards;

using System;
using JsonItems;

/// <summary>
/// Estructura que almacena las propiedades asociadas a los monstruos
/// </summary>

/// <summary>
/// Cartas monstruo: estas se colocan en el campo
/// y pueden atacar directamente los monstruos enemigos
/// </summary>
public class MonsterCard : Card{
    private delegate void Hability(Card? target);
    
    #region Modifiers
    public int AttackPoints = 0;
    public int HP = 0;

    Hability[]? habilities{get;set;}
    #endregion

    public MonsterCard(string Name, string Description, float  AppearingProbability, int AttackPoints, int HP) : base(Name, Description, AppearingProbability){
        this.Name = Name;
        this.HP = HP;
    }

    public MonsterCard(MonsterCardItem args) : base(args.name, args.description, args.appearingProbability){
        this.Name = args.name;
        this.HP = args.hp;
    }

    public override void Play(){
        //TODO
    }

    public override void Drop(){
        //TODO
    }

    public void Attack(){
        //TODO
    }
}