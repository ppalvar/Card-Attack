namespace Cards;

using System;
using CardJsonItems;
using Powers;

/// <summary>
/// Monster cards: this can be placed in the table, attack other monster cards,
/// take powers from Effect cards and use them
/// </summary>
public class MonsterCard : Card
{
    /// <summary>
    /// The damage this monster makes to it's enemies. Minimum attack will be zero
    /// </summary>
    public int AttackPoints
    {
        get
        {
            return _AttackPoints;
        }
        private set
        {
            _AttackPoints = value > 0 ? value : 0;
        }
    }
    private int _AttackPoints;

    /// <summary>
    /// The life points the monster haves
    /// </summary>
    /// <value>Non-negative integer</value>
    public int HP { get; private set; }

    /// <summary>
    /// The power set of a monster
    /// </summary>
    public Power?[] Powers { get; private set; }//Now it only assigns max 3 powers

    /// <summary>
    /// The maximum number of powers a monster can have
    /// </summary>
    public const int MaxPowers = 3;


    public bool IsAlive { get; private set; }
    public bool IsDead { get { return !IsAlive || HP <= 0; } private set { IsAlive = !value; } }

    /// <summary>
    /// Creates a new Monster card. It will be only for instance and will NOT be 
    /// saved in the disk
    /// </summary>
    /// <param name="Name">The name of the card</param>
    /// <param name="Description">A brief description</param>
    /// <param name="Image">The name or address of an image</param>
    /// <param name="AppearingProbability">How likely is this card to appear</param>
    /// <param name="AttackPoints">The damage it makes to other monsters</param>
    /// <param name="HP">The amount of life this monster will have</param>
    public MonsterCard(string Name, string Description, string Image, float AppearingProbability, int AttackPoints, int HP) : base(Name, Description, Image, AppearingProbability)
    {
        this.Name = Name;
        this.HP = HP;
        this.AttackPoints = AttackPoints;

        Powers = new Power[MaxPowers];
        IsAlive = true;
    }

    /// <summary>
    /// Creates a new Monster card. It will be only for instance and will NOT be 
    /// saved in the disk
    /// </summary>
    /// <param name="args">A JsonItem with all the data needed</param>
    public MonsterCard(MonsterCardJsonItem args) : base(args.name, args.description, args.image, args.appearingProbability)
    {
        this.Name = args.name;
        this.HP = args.hp;
        this.AttackPoints = args.attack;

        Powers = new Power[MaxPowers];
        IsAlive = true;
    }

    /// <summary>
    /// Attack phisically a monster
    /// </summary>
    /// <param name="target">A monster card</param>
    private void Attack(MonsterCard target)
    {
        target.HP = target.HP - this.AttackPoints;

        if (target.HP <= 0)
        {
            target.IsAlive = false;
        }
    }

    /// <summary>
    /// Equips a power to this monster. The powers are not removable
    /// and are limited by the MaxPowers constant.
    /// </summary>
    /// <param name="power">A Power object instance</param>
    public void AssignPower(Power power)
    {
        for (int i = 0; i < this.Powers.Length; i++)
        {
            if (Powers[i] == null)
            {
                Powers[i] = power; return;
            }
            else
            {
                // do nothing
            }
        }

        throw new Exception("the maximum number of powers has been reached");
    }

    /// <summary>
    /// Clones this instance of MonsterCard. It's returned as object type, must be casted
    /// </summary>
    /// <returns>An object instance being a clone of this card</returns>
    public override object Clone()
    {
        Power?[] pClone = new Power?[MaxPowers];

        return new MonsterCard(Name, Description, Image, AppearingProbability, AttackPoints, HP)
        {
            Powers = pClone
        };
    }

    /**
        This methods MUST have the format :
            private object MethodName(object param);
        
        they are used to modify the game state with code written in 
        the MLC language, defined in ../interpreterMLC/Interpreter.cs
    **/
    #region State modifiers
    private object IncreaseHP(object amount)
    {
        this.HP += (int)amount;
        if (this.HP <= 0) this.IsAlive = false;

        return this.HP;
    }

    private object IncreaseAttack(object amount)
    {
        this.AttackPoints += (int)amount;
        return this.AttackPoints;
    }

    private object Kill(object live)
    {
        this.IsAlive = (bool)live;
        return live;
    }
    #endregion
}