namespace Players;

using System;
using CardFactorys;
using Cards;

public class Player{
    public bool IsPlaying{get; private set;} = false;
    public Deck? PlayerDeck{get;set;}
    public Card[] Hand{get;private set;}
    public Card[] Table{get;private set;}

    public Player(int cardsInHand=5, int cardsInTable=5){
        this.Hand = new Card[cardsInHand];
        this.Table = new Card[cardsInTable];
    }

    public void Play(Card? card, ref Card? target){
        if (card is EffectCard && target is MonsterCard){
            //TODO
        }
        else if (card is MonsterCard
                 && this.Table.Contains(target) 
                 && !(target is MonsterCard) 
                 && !(target is EffectCard)){
            
            target = card;

            bool flag = true;

            this.Hand = this.Hand.Where(val => {
                if (val == card && flag){
                    flag = false;
                    return false;
                }
                return true;
            }).ToArray();

            if (this.PlayerDeck != null)
                this.PlayerDeck.DeleteMonsterCard(card);
        }
        else {  
            throw new ArgumentException("that movement can't be done");
        }
    }
    
    public virtual void BeginTurn(){
        #region Draw Cards
        for (int i = 0; i < this.Hand.Length && this.PlayerDeck != null && this.PlayerDeck.DeckCards.Length > 0; i++){
            if (!(this.Hand[i] is MonsterCard)){
                this.Hand[i] = this.PlayerDeck.Draw();//TODO
            }
        }
        #endregion
        
        IsPlaying = true;
    }

    public virtual void EndTurn(){
        IsPlaying = false;
    }
}