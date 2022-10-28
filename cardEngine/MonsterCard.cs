namespace Cards;

using System;
using JsonItems;
using IPowers;
using Elements;

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

    public bool IsAlive{get;private set;}
    public bool IsDead{get{return !IsAlive;}private set {IsAlive = !value;}}
    #endregion

    public MonsterCard(string Name, string Description, float  AppearingProbability, int AttackPoints, int HP, string element) : base(Name, Description, AppearingProbability, element){
        this.Name = Name;
        this.HP = HP;
        this.AttackPoints = AttackPoints;

        // Type T_Element = this.element.GetType();

        Powers = new IPower[MaxPowers];
        AssignedPowers = new bool[MaxPowers];
        IsAlive = true;
    }

    public MonsterCard(MonsterCardJsonItem args) : base(args.name, args.description, args.appearingProbability, args.element){
        this.Name = args.name;
        this.HP = args.hp;
        this.AttackPoints = args.attack;

        Powers = new IPower[MaxPowers];
        AssignedPowers = new bool[MaxPowers];
        IsAlive = true;
    }

    public void Attack(ref MonsterCard target){
        target.HP = Math.Max(target.HP - this.AttackPoints, 0);

        if (target.HP <= 0){
            target.IsAlive = false;
        }
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

    public void UsePowerAt(int index) {
        if (this.Powers[index] != null){
            this.Powers[index].ApplyEffect();
        }
    }
}