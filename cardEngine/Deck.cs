namespace CardFactorys;

using  System;
using Cards;


/// <summary>
/// El mazo de cartas del jugador
/// </summary>
public class Deck{
    private Card[] monsterCards, deckCards;

    public Card[] MonsterCards{
        get{return monsterCards;}
        private set{monsterCards = value;}
    }
    public Card[] DeckCards{
        get{return deckCards;}
        private set{deckCards = value;}
    }

    public Deck(Card[] monsterCards, Card[] cards){
        this.MonsterCards = this.monsterCards = monsterCards;
        this.DeckCards = this.deckCards = cards;
    }

    public void DeleteMonsterCard(Card card){
        bool flag = true;
        if (this.MonsterCards != null){
            this.MonsterCards = this.MonsterCards.Where(val => {
                if (card == val && flag){
                    flag = false;
                    return false;
                }
                return true;
            }).ToArray();
        }
    }

    private Card GetCard(ref Card[] cards){
        Random rand = new Random();
        int p = rand.Next(0, cards.Length);

        Card tmp = cards[p];

        bool flag = true;
        cards = cards.Where(val => {
            if (val == tmp && flag){
                flag = false;
                return false;
            }
            return true;
        }).ToArray();

        return tmp;
    }

    public Card Draw(){
        if (this.deckCards != null)
            return GetCard(ref this.deckCards);
        else throw new NullReferenceException();
    }

    public Card DrawMonster(){
        if (this.monsterCards != null)
            return GetCard(ref this.monsterCards);
        else throw new NullReferenceException();
    }
}