namespace Players;

using System;
using CardFactorys;
using Cards;

/// <summary>
/// A class for the human players
/// </summary>
public class Player{
    public bool IsPlaying{get; private set;} = false;
    public Deck? PlayerDeck{get;private set;}

    /// <summary>
    /// The e
    /// </summary>
    /// <value></value>
    public Card?[] Hand{get;private set;}
    public MonsterCard?[] Table{get;private set;}

    public Player(int cardsInHand=5, int cardsInTable=5){
        this.Hand = new Card[cardsInHand];
        this.Table = new MonsterCard[cardsInTable];
    }

    public void Play(int _card, MonsterCard? target=null){
        Card? card = this.Hand[_card];

        if (!this.IsPlaying){
            throw new Exception("you can't play now");
        }

        if (card is EffectCard eCard && target != null){
            eCard.UseCard(target);

            Hand[_card] = null;        
        }
        else if (card is MonsterCard mCard && target == null)
        {
            //remove the played card from the hand
            Hand[_card] = null;
            
            for (int i = 0; i < this.Hand.Length; i++) {
                if (this.Table[i] == null) {
                    this.Table[i] = mCard;
                    break;
                }
            }
        }
        else { 
            throw new ArgumentException("that movement can't be done");
        }
    }
    
    public virtual void BeginTurn(){
        #region Draw Cards
        for (int i = 0; this.Hand != null && i < this.Hand.Length && this.PlayerDeck != null && this.PlayerDeck.DeckCards.Length > 0; i++){
            if (this.Hand[i] == null){
                this.Hand[i] = this.PlayerDeck.Draw();
            }
        }
        #endregion
        
        IsPlaying = true;
    }

    public virtual void EndTurn(){
        IsPlaying = false;
    }

    public void RemoveDeadMonsters() {
        for (int i = 0; i < this.Table.Length; i++) {
            MonsterCard? tmp = this.Table[i] as MonsterCard;
            if (tmp != null && tmp.IsDead) {
                this.Table[i] = null;
            }
        }
    }

    public bool HasNoMonsters() {
        bool flag = false;

        if (PlayerDeck != null) {
            foreach (Card card in PlayerDeck.DeckCards) {
                flag = flag | card is MonsterCard;
            }
        }
        
        return this.PlayerDeck != null && !flag;
    }

    public void SetDeck(Deck deck) {
        if (this.PlayerDeck == null) {
            this.PlayerDeck = deck;
        }
    }

    /**
        This methods MUST have the format :
            protected object MethodName(object param);
        
        they are used to modify the game state with code written in 
        the MLC language, defined in ../interpreterMLC/Interpreter.cs
    **/
    #region State modifiers
        protected object EndTurn(object empty) {
            EndTurn();
            return empty;
        }

        protected object HandAt(object index) {
             Card? monster = this.Hand[(int) index];
            if (monster != null) {
                return monster;
            }
            else return new Object();
        }

        protected object TableAt(object index) {
            MonsterCard? monster = this.Table[(int) index];
            if (monster != null) {
                return monster;
            }
            else return new Object();
        }
    #endregion
}