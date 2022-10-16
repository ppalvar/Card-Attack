namespace Executer;

using CardStores;
using Cards;

public class Program{
    public static void Main(){
        CardStore store = new CardStore(5, 5);
        
        MonsterCard m = new MonsterCard("Gato", "adiknd", 0.1f, 10, 100);
        store.CreateCard<MonsterCard>(m);
        
        Deck deck = store.GetDeck();

        foreach (Card c in deck.DeckCards){
            Console.WriteLine(c);
        }
    }
}