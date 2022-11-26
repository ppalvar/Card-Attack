namespace Match;

using System;
using Players;
using Cards;
using CardFactorys;
using Powers;

/// <summary>
/// If the match will be between a human and a computer or between two humans
/// </summary>
public enum MatchTypes{
    HumanVSHuman,
    ComputerVSHuman
}

/// <summary>
/// The object of a match
/// </summary>
public class Match{
    /// <summary>
    /// The player that is currently playing
    /// </summary>
    /// <value>Players.Player</value>
    public Player player {get;private set;}

    /// <summary>
    /// The player that will play next
    /// </summary>
    /// <value>Players.Player</value>
    public Player enemy  {get;private set;}

    /// <summary>
    /// A factory to get the Decks
    /// </summary>
    public CardFactory Factory{get;private set;}

    public int TurnCounter{get;private set;}
    private bool[] UsedMonsters{get; set;}

    public bool isPlaying{get;private set;} = false;

    /// <summary>
    /// Gets a new instance of Match.Match
    /// </summary>
    /// <param name="matchType">Enum with game types</param>
    /// <param name="cardsInHand">how many cards can have a player in the hand</param>
    /// <param name="cardsInTable">how many cards can have a player in the hand</param>
    /// <param name="maxEffectCards">how many effect cards can have a player</param>
    /// <param name="maxMonsterCards">how many monster cards can have a player</param>
    public Match(MatchTypes matchType, int cardsInHand, int cardsInTable, int maxEffectCards=20, int maxMonsterCards=5){
        player = new Player(cardsInHand, cardsInTable);
        
        if (matchType == MatchTypes.HumanVSHuman){
            enemy = new Player(cardsInHand, cardsInTable);
        }
        else {
            enemy = new VirtualPlayer(cardsInHand, cardsInTable);
        }

        this.Factory = new CardFactory(maxEffectCards, maxMonsterCards);

        TurnCounter = 0;
        UsedMonsters = new bool[cardsInTable];
    }

    public void SetDeck(Deck deck, bool player) {
        if (isPlaying)return;

        if (player) {
            this.player.SetDeck(deck);
        }
        else {
            this.enemy.SetDeck(deck);
        }
    }

    public void SetDeck(string[] names, bool player) {
        if (isPlaying)return;

        Deck deck = Factory.GetDeck(names);

        SetDeck(deck, player);
    }

    public void SetDeck(bool player) {
        if (isPlaying)return;
        
        Deck deck = Factory.GetDeck();

        SetDeck(deck, player);
    }

    public void BeginGame() {
        if (!isPlaying) {
            isPlaying = true;
            player.BeginTurn();
        }
    }

    /// <summary>
    /// Ends the current turn and begins the turn of the enemy
    /// </summary>
    public void EndTurn() {
        for (int i = 0; i < UsedMonsters.Length; i++) {
            UsedMonsters[i] = false;
        }

        player.EndTurn();
        enemy.BeginTurn();

        Player tmp = this.player;
        this.player = this.enemy;
        this.enemy  = tmp;

        this.TurnCounter ++;
    }

    /// <summary>
    /// Plays a card on the field if it is a Monster Card
    /// and equips it to a monster if is an EffectCard
    /// </summary>
    /// <param name="card">The card you wanna play</param>
    /// <param name="target">The card on wich you wanna equip the power (EffectCard only)</param>
    public void Play(int card, MonsterCard? target){        
        player.Play(card, target);
    }

    /// <summary>
    /// Decides if there is a winner.
    /// A player wins when the other loses.
    /// A player loses when they lost all their cards
    /// </summary>
    /// <returns></returns>
    public Player? Winner(){
        bool playerFlag = false;
        bool enemyFlag  = false;

        for (int i = 0; i < player.Table.Length; i++){
            if (player.Table[i] != null){
                playerFlag = true;
            }

            if (enemy.Table[i] != null){
                enemyFlag = true;
            }
        }

        for (int i = 0; i < player.Hand.Length; i++){
            if (player.Hand[i] != null){
                playerFlag = true;
            }

            if (enemy.Hand[i] != null){
                enemyFlag = true;
            }
        }

        if (player.HasNoMonsters() && !playerFlag)return enemy;
        if (enemy.HasNoMonsters()  && !enemyFlag) return player;

        return null;
    }

    /// <summary>
    /// Makes phisical attack to an specified monster
    /// </summary>
    /// <param name="monsterIndex">The 0-starting index of the monster that will attack</param>
    /// <param name="target">The monster that will be attacked</param>
    public bool Attack(int monsterIndex, MonsterCard target) {
        if (TurnCounter < 2)throw new Exception("can't attack yet");

        if (player.Table.Length > monsterIndex){
            if (UsedMonsters[monsterIndex])throw new Exception("this monster has been already used");

            UsedMonsters[monsterIndex] = true;

            MonsterCard? monster = player.Table[monsterIndex];
            
            if (monster != null){
                monster.Attack(target);
            }
        }

        enemy.RemoveDeadMonsters();

        return AutoEndTurn();
    }

    /// <summary>
    /// Uses a power in a specified monster
    /// </summary>
    /// <param name="monsterIndex">The 0-starting index of the monster that will use a power</param>
    /// <param name="powerIndex">The 0-starting index of the power that will be used</param>
    /// <param name="target">The actual card, victim of the power</param>
    public void UsePower(int monsterIndex, int powerIndex, MonsterCard target) {
        if (monsterIndex >= player.Table.Length || powerIndex >= MonsterCard.MaxPowers) {
            return;
        }

        if (UsedMonsters[monsterIndex])return;

        UsedMonsters[monsterIndex] = true;

        MonsterCard? monsterCard = player.Table[monsterIndex];
        
        if (monsterCard != null) {
            Power? power = monsterCard.Powers[powerIndex];
            
            if (power != null) {
                power.UsePower(monsterCard, target, this);
            }
        }
    }

    /// <summary>
    /// Detects when is no possible to make any other 
    /// movement and ends the turn
    /// </summary>
    private bool AutoEndTurn() {
        bool hasEmptySlot = false;

        for (int i = 0; i < UsedMonsters.Length; i++) {
            if (player.Table[i] == null) hasEmptySlot = true;
            if (!UsedMonsters[i] && player.Table[i] != null)return false;
        }

        foreach (Card? card in player.Hand) {
            if (card != null && card is MonsterCard && hasEmptySlot)return false;
        }

        EndTurn();
        return true;
    }
}