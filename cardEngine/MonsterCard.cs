namespace Cards;

using System;
using JsonItems;
using Powers;

/// <summary>
/// Cartas monstruo: estas se colocan en el campo
/// y pueden atacar directamente los monstruos enemigos
/// </summary>
public class MonsterCard : Card{
    #region Modifiers

    /// <summary>
    /// The damage this monster makes to it's enemies
    /// </summary>
    /// <value>Integer(if < 0, then it heals)</value>
    public int AttackPoints{get;private set;}

    /// <summary>
    /// The life points the monster haves
    /// </summary>
    /// <value>Non-negative integer</value>
    public int HP{get;set;}

    /// <summary>
    /// The power set of a monster
    /// </summary>
    public Power?[] Powers{get;private set;}//Now it only assigns max 3 powers
    private bool[] AssignedPowers{get;set;}

    public const int MaxPowers = 3;

    public bool IsAlive{get;private set;}
    public bool IsDead{get{return !IsAlive;}private set {IsAlive = !value;}}
    #endregion

    public MonsterCard(string Name, string Description, string Image, float  AppearingProbability, int AttackPoints, int HP, string element) : base(Name, Description, Image, AppearingProbability, element){
        this.Name = Name;
        this.HP = HP;
        this.AttackPoints = AttackPoints;

        // Type T_Element = this.element.GetType();

        Powers = new Power[MaxPowers];
        AssignedPowers = new bool[MaxPowers];
        IsAlive = true;
    }

    public MonsterCard(MonsterCardJsonItem args) : base(args.name, args.description, args.image, args.appearingProbability, args.element){
        this.Name = args.name;
        this.HP = args.hp;
        this.AttackPoints = args.attack;

        Powers = new Power[MaxPowers];
        AssignedPowers = new bool[MaxPowers];
        IsAlive = true;
    }

    /// <summary>
    /// Attack phisically a monster
    /// </summary>
    /// <param name="target">A monster card</param>
    public void Attack(MonsterCard target){
        target.HP = target.HP - this.AttackPoints;

        if (target.HP <= 0){
            target.IsAlive = false;
        }
    }

    /// <summary>
    /// Equips a power to this monster. The powers are not removable
    /// and are limited by the MaxPowers constant.
    /// </summary>
    /// <param name="power"></param>
    public void AssignPower(Power power){
        for (int i = 0; i < this.Powers.Length; i++){
            if (!AssignedPowers[i]){
                Powers[i] = power;
                AssignedPowers[i] = true;
                return;
            }
        }

        throw new Exception("the maximum number of powers has been reached");
    }

    /**
        This methods MUST have the format :
            private object MethodName(object param);
        
        they are used to modify the game state with code written in 
        the MLC language, defined in ../interpreterMLC/Interpreter.cs
    **/
    #region State modifiers
        private object IncreaseHP(object amount) {
            this.HP += (int)amount;
            if (this.HP <= 0)this.IsAlive = false;

            return this.HP;
        }

        private object IncreaseAttack(object amount) {
            this.AttackPoints += (int)amount;
            return this.AttackPoints;
        }

        private object Kill(object live) {
            this.IsAlive = (bool) live;
            return live;
        }
    #endregion
}