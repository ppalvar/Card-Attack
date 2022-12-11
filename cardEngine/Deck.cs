namespace CardFactorys;

using System;
using Cards;


/// <summary>
/// The deck of cards a player will have
/// </summary>
public class Deck
{
    /// <summary>
    /// The cards in the deck
    /// </summary>
    private Card[] DeckCards { get; set; }

    public bool HasCards
    {
        get { return DeckCards.Length != 0; }
    }

    public bool HasMonsters
    {
        get
        {
            foreach (var c in DeckCards)
            {
                if (c != null && c is MonsterCard) return true;
            }
            return false;
        }
    }

    public Deck(Card[] cards)
    {
        this.DeckCards = cards;
    }

    /// <summary>
    /// Gets a randomized card from the deck
    /// </summary>
    /// <returns>A random card</returns>
    public Card Draw()
    {
        Random rand = new Random();
        int p = rand.Next(0, this.DeckCards.Length);

        Card tmp = this.DeckCards[p];

        bool flag = true;
        this.DeckCards = this.DeckCards.Where(val =>
        {
            if (val == tmp && flag)
            {
                flag = false;
                return false;
            }
            return true;
        }).ToArray();

        return tmp;
    }
}