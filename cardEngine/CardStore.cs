namespace CardStores;

using System;
using Cards;
using JsonItems;
using JsonRW;

public class CardStore{
    #region Readers
    JsonRW<MonsterCardItem>? MonsterReader;
    JsonRW<EffectCardItem>? EffectReader;
    #endregion
    
    protected Dictionary<string, Card[]>? CardOptions{get;set;}
    private static int MaxCards{get;set;}
    private static int MaxMonsterCards{get;set;}

    public CardStore(int maxCards=20, int maxMonsterCards=5){
        LoadCardsFromDisk();
        MaxCards = maxCards;
        MaxMonsterCards = maxMonsterCards;
    }

    private void LoadCardsFromDisk(){
        MonsterReader = new JsonRW<MonsterCardItem>("cardsDB/" + nameof(MonsterCard) + ".json");
        EffectReader  = new JsonRW<EffectCardItem> ("cardsDB/" + nameof(EffectCard)  + ".json");
        
        CardOptions = new Dictionary<string, Card[]>();

        
        if (!(MonsterReader.Content is null)){
            List <MonsterCard> mCards = new List<MonsterCard>();
            
            foreach (MonsterCardItem i in MonsterReader.Content){
                mCards.Add(new MonsterCard(i));
            }

            CardOptions.Add(nameof(MonsterCard), mCards.ToArray());
        }

        if (!(EffectReader.Content is null)){
            List <EffectCard> eCards = new List<EffectCard>();
            
            foreach (EffectCardItem i in EffectReader.Content){
                eCards.Add(new EffectCard(i));
            }

            CardOptions.Add(nameof(EffectCard), eCards.ToArray());
        }
    }

    public void CreateCard<T>(object? args){
        if (typeof(T) == typeof(MonsterCard)){
            MonsterCard? m = args as MonsterCard;

            if (m!= null){
                MonsterCardItem tmp = new MonsterCardItem(m.Name, m.Description, m.AppearingProbability, m.AttackPoints, m.HP);
                if (MonsterReader != null && MonsterReader.Content != null){
                    MonsterReader.Add(tmp);
                }
            }
        }

        if (typeof(T) == typeof(EffectCard)){
            EffectCard? m = args as EffectCard;

            if (m!= null){
                EffectCardItem tmp = new EffectCardItem(m.Name, m.Description, m.AppearingProbability);
                if (EffectReader != null && EffectReader.Content != null){
                    EffectReader.Add(tmp);
                }
            }
        }
    }

    public Deck GetDeck(){
        Random rand = new Random();
        
        Card[] cards = new Card[MaxCards];

        for (int i = 0; i < MaxMonsterCards && CardOptions != null; i++){
            int r = (int) rand.NextInt64(0, CardOptions[nameof(MonsterCard)].Length);
            cards[i] = CardOptions[nameof(MonsterCard)][r];
        }

        for (int i = MaxMonsterCards; i < MaxCards && CardOptions != null; i++){
            int r = (int) rand.NextInt64(0, CardOptions[nameof(EffectCard)].Length);
            cards[i] = CardOptions[nameof(EffectCard)][r];
        }

        return new Deck(cards);
    }

    // public Deck GetDeck(int algo){
    //     //TODO
    //     return new Deck();
    // }
}
