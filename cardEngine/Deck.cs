namespace CardStores;

using  System;
using Cards;


/// <summary>
/// El mazo de cartas del jugador
/// </summary>
public class Deck{
    public Card[] DeckCards{get;set;}

    public Deck(Card[] cards){
        this.DeckCards = cards;
    }

    public Card Draw(){
        return DeckCards[0];
    }
}