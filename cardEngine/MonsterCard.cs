namespace Cards;

using System;
using JsonItems;
using IPowers;
using Elements;

/// <summary>
/// Estructura que almacena las propiedades asociadas a los monstruos
/// </summary>

/// <summary>
/// Cartas monstruo: estas se colocan en el campo
/// y pueden atacar directamente los monstruos enemigos
/// </summary>
public class MonsterCard : Card{
    #region Modifiers
    public int AttackPoints{get;set;}
    public int HP{get;set;}

    private IPower[] Powers{get;set;}//Now it only assigns max 3 powers
    private bool[] AssignedPowers{get;set;}

    const int MaxPowers = 3;
    #endregion

    public MonsterCard(string Name, string Description, float  AppearingProbability, int AttackPoints, int HP, string element) : base(Name, Description, AppearingProbability, element){
        this.Name = Name;
        this.HP = HP;
        this.AttackPoints = AttackPoints;

        // Type T_Element = this.element.GetType();

        Powers = new IPower[MaxPowers];
        AssignedPowers = new bool[MaxPowers];
    }

    public MonsterCard(MonsterCardJsonItem args) : base(args.name, args.description, args.appearingProbability, args.element){
        this.Name = args.name;
        this.HP = args.hp;
        this.AttackPoints = args.attack;

        Powers = new IPower[MaxPowers];
        AssignedPowers = new bool[MaxPowers];
    }

    public override void Play(){
        //TODO
    }

    public override void Drop(){
        //TODO
    }

    public void Attack(ref MonsterCard target){
        target.HP = Math.Max(target.HP - this.AttackPoints, 0);
    }

    public void AssignPower(IPower power){
        if (power.Element.Type != this.element.Type){
            throw new Exception($"can't assign power of type {power.Element.Type} to a monster of type {this.element.Type}");
        }

        for (int i = 0; i < this.Powers.Length; i++){
            if (!AssignedPowers[i]){
                Powers[i] = power;
                AssignedPowers[i] = true;
                return;
            }
        }

        throw new Exception("the maximum number of powers has been reached");
    }
}