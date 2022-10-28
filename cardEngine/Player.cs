namespace Players;

using System;
using CardFactorys;
using Cards;

public class Player{
    public bool IsPlaying{get; private set;} = false;
    public Deck? PlayerDeck{get;set;}
    public Card[]? Hand{get;private set;}
    public MonsterCard[]? Table{get;private set;}

    public Player(int cardsInHand=5, int cardsInTable=5){
        this.Hand = new Card[cardsInHand];
        this.Table = new MonsterCard[cardsInTable];
    }

    public void Play(Card card, ref MonsterCard? target){
        if (!this.IsPlaying){
            throw new Exception("you can't play now");
        }

        if (card is EffectCard && target is MonsterCard && target != null){
            EffectCard tmpCard = (EffectCard) card;
            tmpCard.UseCard(ref target);
        }
        else if (card is MonsterCard
                 && this.Table != null
                 && this.Hand != null
                 && this.Table.Contains(target) 
                 && !(target is MonsterCard))
        {

            target = (MonsterCard) card;

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
        for (int i = 0; this.Hand != null && i < this.Hand.Length && this.PlayerDeck != null && this.PlayerDeck.DeckCards.Length > 0; i++){
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

    public void RemoveDeadMonsters() {
        for (int i = 0; this.Table != null && i < this.Table.Length; i++) {
            MonsterCard? tmp = this.Table[i] as MonsterCard;
            Card[] tmpCard = new Card[1];//this line only avoids a NULL error
            if (tmp != null && tmp.IsDead)this.Table[i] = (MonsterCard) tmpCard[0];
        }
    }
}