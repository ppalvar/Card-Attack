namespace CardFactorys;

using  System;
using Cards;


/// <summary>
/// El mazo de cartas del jugador
/// </summary>
public class Deck{
    private Card[] deckCards;

    public Card[] DeckCards{
        get{return deckCards;}
        private set{deckCards = value;}
    }

    public Deck(Card[] cards){
        this.DeckCards = this.deckCards = cards;
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
}