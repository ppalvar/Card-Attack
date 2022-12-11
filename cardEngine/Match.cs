namespace Match;

using System;
using Players;
using Cards;
using CardFactorys;
using Powers;

/// <summary>
/// If the match will be between a human and a computer or between two humans
/// </summary>
public enum MatchTypes
{
    HumanVSHuman,
    ComputerVSHuman
}

/// <summary>
/// The object of a match
/// </summary>
public class Match
{
    /// <summary>
    /// The player that is currently playing
    /// </summary>
    /// <value>Players.Player</value>
    public Player player { get; private set; }

    /// <summary>
    /// The player that will play next
    /// </summary>
    /// <value>Players.Player</value>
    public Player enemy { get; private set; }

    /// <summary>
    /// A factory to get the Decks
    /// </summary>
    public CardFactory Factory { get; private set; }

    public int TurnCounter { get; private set; }

    public bool isPlaying { get; private set; } = false;

    /// <summary>
    /// Gets a new instance of Match.Match
    /// </summary>
    /// <param name="matchType">Enum with game types</param>
    /// <param name="cardsInHand">how many cards can have a player in the hand</param>
    /// <param name="cardsInTable">how many cards can have a player in the hand</param>
    /// <param name="maxEffectCards">how many effect cards can have a player</param>
    /// <param name="maxMonsterCards">how many monster cards can have a player</param>
    public Match(MatchTypes matchType, int cardsInHand, int cardsInTable, int maxEffectCards = 20, int maxMonsterCards = 5)
    {
        player = new Player(cardsInHand, cardsInTable);

        if (matchType == MatchTypes.HumanVSHuman)
        {
            enemy = new Player(cardsInHand, cardsInTable);
        }
        else
        {
            enemy = new VirtualPlayer(cardsInHand, cardsInTable);
        }

        this.Factory = new CardFactory(maxEffectCards, maxMonsterCards);

        TurnCounter = 0;
    }

    /// <summary>
    /// Sets the Deck for a player.
    /// Can only be called before the method BeginGame
    /// </summary>
    /// <param name="deck">The deck that will be assigned</param>
    /// <param name="player">true for the first player, false for the second one </param>
    public void SetDeck(Deck deck, bool player)
    {
        if (isPlaying) throw new Exception("Cannot change the deck in the middle of a fight");

        if (player)
        {
            this.player.SetDeck(deck);
        }
        else
        {
            this.enemy.SetDeck(deck);
        }
    }

    /// <summary>
    /// Sets the Deck for a player.
    /// Can only be called before the method BeginGame
    /// </summary>
    /// <param name="names">The names of the cards you wanna in your deck</param>
    /// <param name="player">true for the first player, false for the second one </param>
    public void SetDeck(string[] names, bool player)
    {
        if (isPlaying) throw new Exception("Cannot change the deck in the middle of a fight");

        Deck deck = Factory.GetDeck(names);

        SetDeck(deck, player);
    }

    /// <summary>
    /// Sets a random Deck for a player.
    /// Can only be called before the method BeginGame
    /// </summary>
    /// <param name="player">true for the first player, false for the second one </param>
    public void SetDeck(bool player)
    {
        if (isPlaying) return;

        Deck deck = Factory.GetDeck();

        SetDeck(deck, player);
    }

    /// <summary>
    /// Begins the game and starts the turn of the first player
    /// </summary>
    public void BeginGame()
    {
        if (!isPlaying)
        {
            isPlaying = true;
            player.BeginTurn(this);
        }
    }

    /// <summary>
    /// Ends the current turn and begins the turn of the enemy
    /// </summary>
    public void EndTurn()
    {
        player.EndTurn();
        enemy.BeginTurn(this);

        Player tmp = this.player;
        this.player = this.enemy;
        this.enemy = tmp;

        this.TurnCounter++;
    }

    /// <summary>
    /// Plays a card on the field if it is a Monster Card
    /// and equips it to a monster if is an EffectCard
    /// </summary>
    /// <param name="card">The card you wanna play</param>
    /// <param name="target">The card on wich you wanna equip the power (EffectCard only)</param>
    public void Play(int card, MonsterCard? target)
    {
        player.Play(card, target);
    }

    /// <summary>
    /// Makes phisical attack to an specified monster
    /// </summary>
    /// <param name="monsterIndex">The 0-starting index of the monster that will attack</param>
    /// <param name="target">The monster that will be attacked</param>
    public bool Attack(int monsterIndex, MonsterCard target)
    {
        if (TurnCounter < 2) throw new Exception("can't attack yet");

        player.Attack(monsterIndex, target);

        player.RemoveDeadMonsters();
        enemy.RemoveDeadMonsters();

        return AutoEndTurn();
    }

    /// <summary>
    /// Uses a power in a specified monster
    /// </summary>
    /// <param name="monsterIndex">The 0-starting index of the monster that will use a power</param>
    /// <param name="powerIndex">The 0-starting index of the power that will be used</param>
    /// <param name="target">The actual card, victim of the power</param>
    public bool UsePower(int monsterIndex, int powerIndex, MonsterCard target)
    {
        if (monsterIndex >= player.Table.Length || powerIndex >= MonsterCard.MaxPowers)
        {
            return false;
        }

        player.UsePower(monsterIndex, powerIndex, target, this);

        player.RemoveDeadMonsters();
        enemy.RemoveDeadMonsters();

        return AutoEndTurn();
    }

    /// <summary>
    /// Decides if there is a winner.
    /// A player wins when the other loses.
    /// A player loses when they lost all their cards
    /// </summary>
    /// <returns></returns>
    public Player? Winner()
    {
        if (player.HasNoMonsters()) return enemy;
        if (enemy.HasNoMonsters() ) return player;

        return null;
    }

    /// <summary>
    /// Makes a copy of the match
    /// </summary>
    /// <returns>A new Match object with the same data</returns>
    public Match Clone()
    {
        return (Match)this.MemberwiseClone();
    }

    /// <summary>
    /// Detects when is no possible to make any other 
    /// movement and ends the turn
    /// </summary>
    private bool AutoEndTurn()
    {
        bool hasEmptySlot = false;

        if (player.HasUnusedMonsters()) return false;

        for (int i = 0; i < player.Table.Length; i++)
        {
            if (player.Table[i] == null) hasEmptySlot = true;
        }

        foreach (Card? card in player.Hand)
        {
            if (card != null && card is MonsterCard && hasEmptySlot) return false;
        }

        EndTurn();
        return true;
    }

    /**
        This methods MUST have the format :
            protected object MethodName(object param);
        
        they are used to modify the game state with code written in 
        the MLC language, defined in ../interpreterMLC/Interpreter.cs
    **/
    #region State modifiers
    protected object EndTurn(object empty)
    {
        EndTurn();
        return empty;
    }
    #endregion
}