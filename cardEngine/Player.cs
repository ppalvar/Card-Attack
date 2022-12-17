namespace Players;

using System;
using System.Reflection;
using CardFactorys;
using Cards;
using Match;
using Powers;

/// <summary>
/// A class for the human players
/// </summary>
public class Player
{
    public bool IsPlaying { get; private set; } = false;
    public Deck? PlayerDeck { get; private set; }

    /// <summary>
    /// The cards currently in the player's hand
    /// </summary>
    /// <value>An array of cards, if one is null, then this space is empty</value>
    public Card?[] Hand { get; private set; }

    /// <summary>
    /// The cards currently in the player's table
    /// </summary>
    /// <value>An array of cards, if one is null, then this space is empty</value>
    public MonsterCard?[] Table { get; private set; }

    /// <summary>
    /// Marks the monsters as used when they activate a power or make an attack
    /// </summary>
    /// <value>A bool array, true for used monsters false otherwise</value>
    private bool[] UsedMonsters { get; set; }

    /// <summary>
    /// Makes a new Player
    /// </summary>
    /// <param name="cardsInHand">The number of cards the player will have in the hand</param>
    /// <param name="cardsInTable">The number of cards the player can put in the table</param>
    public Player(int cardsInHand = 5, int cardsInTable = 5)
    {
        this.Hand = new Card[cardsInHand];
        this.Table = new MonsterCard[cardsInTable];

        this.UsedMonsters = new bool[cardsInTable];
    }

    /// <summary>
    /// Plays a card.
    /// If it's a MonsterCard, then no target should be provided, the 
    /// card will automatically find a place
    /// If it's an EffectCard, then target must have the card that will 
    /// take the power of the effect card
    /// </summary>
    /// <param name="_card">The index in the hand of the card you wanna play</param>
    /// <param name="target">The card that will take the power (for EffectCards only)</param>
    public void Play(int _card, MonsterCard? target = null)
    {
        Card? card = this.Hand[_card];

        if (!this.IsPlaying)
        {
            throw new Exception("you can't play now");
        }

        if (card is EffectCard eCard && target != null)
        {
            eCard.UseCard(target);

            Hand[_card] = null;
        }
        else if (card is MonsterCard mCard && target == null)
        {
            //remove the played card from the hand
            Hand[_card] = null;

            for (int i = 0; i < this.Hand.Length; i++)
            {
                if (this.Table[i] == null)
                {
                    this.Table[i] = mCard;
                    break;
                }
            }
        }
        else
        {
            throw new ArgumentException("that movement can't be done");
        }
    }

    /// <summary>
    /// Makes an phisical attack. The HP of the victim will be lowered
    /// in equal amount to the attacker's AttackPoints
    /// </summary>
    /// <param name="monsterIndex">The index in the table of the card that will attack</param>
    /// <param name="target">The card that will take damage</param>
    public void Attack(int monsterIndex, MonsterCard target)
    {
        if (this.Table.Length > monsterIndex)
        {
            if (UsedMonsters[monsterIndex]) throw new Exception("this monster has been already used");

            UsedMonsters[monsterIndex] = true;

            MonsterCard? monster = this.Table[monsterIndex];

            if (monster != null)
            {
                MethodInfo? Attack = monster.GetType().GetMethod("Attack", BindingFlags.Instance | BindingFlags.NonPublic);
                if (Attack != null)
                    Attack.Invoke(monster, new Object[] { target });
            }
        }
    }

    /// <summary>
    /// Uses a power
    /// </summary>
    /// <param name="monsterIndex">The index in the table of the monster that will use a power</param>
    /// <param name="powerIndex">The index in the monster's power of the power you wanna use</param>
    /// <param name="target">The monster victim of this power</param>
    /// <param name="match">The state of the match</param>
    public void UsePower(int monsterIndex, int powerIndex, MonsterCard target, Match match)
    {
        if (UsedMonsters[monsterIndex]) throw new Exception("this monster has been already used");

        UsedMonsters[monsterIndex] = true;

        MonsterCard? monsterCard = this.Table[monsterIndex];

        if (monsterCard != null)
        {
            Power? power = monsterCard.Powers[powerIndex];

            if (power != null)
            {
                MethodInfo? UsePower = power.GetType().GetMethod("UsePower", BindingFlags.Instance | BindingFlags.NonPublic);
                if (UsePower != null)
                    UsePower.Invoke(power, new Object[] { monsterCard, target, match });
            }
        }
    }

    /// <summary>
    /// Begins a new turn. Must be called before making some action
    /// </summary>
    /// <param name="match">The state of the match</param>
    public virtual void BeginTurn(Match? match)
    {
        #region Draw Cards
        for (int i = 0; i < this.Hand.Length && this.PlayerDeck != null && this.PlayerDeck.HasCards; i++)
        {
            if (this.Hand[i] == null)
            {
                this.Hand[i] = this.PlayerDeck.Draw();
            }
        }
        #endregion

        for (int i = 0; i < UsedMonsters.Length; i++)
        {
            UsedMonsters[i] = false;
        }

        IsPlaying = true;
    }

    /// <summary>
    /// Ends the current turn
    /// </summary>
    public virtual void EndTurn()
    {
        IsPlaying = false;
    }

    /// <summary>
    /// Removes from the table all monsters with the IsDead property set to true
    /// </summary>
    public void RemoveDeadMonsters()
    {
        for (int i = 0; i < this.Table.Length; i++)
        {
            MonsterCard? tmp = this.Table[i] as MonsterCard;
            if (tmp != null && tmp.IsDead)
            {
                this.Table[i] = null;
            }
        }
    }

    /// <summary>
    /// Checks if the player has some monster either in the hand, table or deck
    /// </summary>
    /// <returns>true if the player has some monster left, false otherwise</returns>
    public bool HasNoMonsters()
    {
        bool flag = false;

        if (PlayerDeck != null)
        {
            flag = PlayerDeck.HasMonsters;

            foreach (Card? card in this.Hand)
            {
                if (card != null && card is MonsterCard) flag = true;
            }

            foreach (Card? card in this.Table)
            {
                if (card != null && card is MonsterCard) flag = true;
            }
        }

        return this.PlayerDeck != null && !flag;
    }

    /// <summary>
    /// Checks if the user has some monster left that hasn't attacked
    /// or used a power yet
    /// </summary>
    public bool HasUnusedMonsters()
    {
        for (int i = 0; i < UsedMonsters.Length; i++)
        {
            if (!UsedMonsters[i] && this.Table[i] != null) return true;
        }
        return false;
    }

    /// <summary>
    /// Sets the deck for the player
    /// </summary>
    /// <param name="deck">A Deck instance</param>
    public void SetDeck(Deck deck)
    {
        if (this.PlayerDeck == null)
        {
            this.PlayerDeck = deck;
        }
    }

    /// <summary>
    /// Drops a card. It will not be regained never and the slot will
    /// be empty until you draw a card in the next turn
    /// </summary>
    /// <param name="index">The index in the hand of the card you wanna drop</param>
    /// <returns>true if this card exists and can be dropped, false otherwise</returns>
    public bool DropCard(int index)
    {
        if (index < this.Hand.Length && this.Hand[index] != null)
        {
            this.Hand[index] = null;
            return true;
        }

        return false;
    }

    /**
        This methods MUST have the format :
            protected object MethodName(object param);
        
        they are used to modify the game state with code written in 
        the MLC language, defined in ../interpreterMLC/Interpreter.cs
    **/
    #region State modifiers
    protected object TableAt(object index)
    {
        MonsterCard? monster = this.Table[(int)index];
        if (monster != null)
        {
            return monster;
        }
        else return new MonsterCard("None", "None", "None", 0, 0, 0);
    }
    #endregion
}