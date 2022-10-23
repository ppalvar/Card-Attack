namespace CardFactorys;

using System;
using Cards;
using JsonItems;
using JsonRW;

public class CardFactory{
    #region Readers
    JsonRW<MonsterCardJsonItem>? MonsterReader;
    JsonRW<EffectCardJsonItem>? EffectReader;
    #endregion
    
    protected Dictionary<string, Card[]>? CardOptions{get;set;}
    private static int MaxCards{get;set;}
    private static int MaxMonsterCards{get;set;}

    public CardFactory(int maxCards=20, int maxMonsterCards=5){
        LoadCardsFromDisk();
        MaxCards = maxCards;
        MaxMonsterCards = maxMonsterCards;
    }

    public Card[] GetCardOptions(){
        Card[] options = new Card[MaxCards];

        for (int i = 0; i < MaxMonsterCards && this.CardOptions != null; i++){
            options[i] = this.CardOptions[nameof(MonsterCard)][i];
        }
        for (int i = MaxMonsterCards; i < MaxCards && this.CardOptions != null; i++){
            options[i] = this.CardOptions[nameof(EffectCard)][i];
        }

        return options;
    }

    public void CreateCard<T>(object? args){
        if (typeof(T) == typeof(MonsterCard)){
            MonsterCard? m = args as MonsterCard;

            if (m!= null){
                MonsterCardJsonItem tmp = new MonsterCardJsonItem(m.Name, m.Description, m.AppearingProbability,m.element.Type, m.AttackPoints, m.HP);
                if (MonsterReader != null && MonsterReader.Content != null){
                    MonsterReader.Add(tmp);
                    if (this.CardOptions != null){
                        this.CardOptions[nameof(MonsterCard)] = this.CardOptions[nameof(MonsterCard)].Append(m).ToArray();
                    }
                }
            }
        }

        if (typeof(T) == typeof(EffectCard)){
            EffectCard? m = args as EffectCard;

            if (m!= null){
                EffectCardJsonItem tmp = new EffectCardJsonItem(m.Name, m.Description, m.AppearingProbability, m.element.Type);
                if (EffectReader != null && EffectReader.Content != null){
                    EffectReader.Add(tmp);
                    if (this.CardOptions != null){
                        this.CardOptions[nameof(EffectCard)] = this.CardOptions[nameof(EffectCard)].Append(m).ToArray();
                    }
                }
            }
        }
    }

    public Deck GetDeck(){
        Card[] monsterCards = new Card[MaxMonsterCards];
        Card[] cards = new Card[MaxCards - MaxMonsterCards];

        for (int i = 0; i < MaxMonsterCards && CardOptions != null; i++){
            monsterCards[i] = GetRandomCard(CardOptions[nameof(MonsterCard)]);
        }

        for (int i = 0; i < MaxCards - MaxMonsterCards && CardOptions != null; i++){
            cards[i] = GetRandomCard(CardOptions[nameof(MonsterCard)]);
        }

        return new Deck(monsterCards, cards);
    }

    public Deck GetDeck(Card[] requestedCards){
        Card[] monsterCards = new Card[MaxMonsterCards];
        Card[] cards = new Card[MaxCards - MaxMonsterCards];

        int j = 0, k = 0;
        try{
            for (int i = 0; i < requestedCards.Length; i++){
                if (requestedCards[i] is MonsterCard){
                    monsterCards[j++] = requestedCards[i];
                }
            }
            
            for (int i = 0; i < requestedCards.Length; i++){
                if (requestedCards[i] is EffectCard){
                    cards[k++] = requestedCards[i];
                }
            }
        }
        catch (ArgumentException){
            throw new Exception("the requested deck cannot be created");
        }

        for (;j < MaxMonsterCards && CardOptions != null; j++){
            monsterCards[j] = GetRandomCard(CardOptions[nameof(MonsterCard)]);
        }

        for (;k < MaxCards - MaxMonsterCards && CardOptions != null; k++){
            cards[k] = GetRandomCard(CardOptions[nameof(EffectCard)]);
        }

        return new Deck(monsterCards, cards);
    }


    private void LoadCardsFromDisk(){
        MonsterReader = new JsonRW<MonsterCardJsonItem>("cardsDB/" + nameof(MonsterCard) + ".json");
        EffectReader  = new JsonRW<EffectCardJsonItem> ("cardsDB/" + nameof(EffectCard)  + ".json");
        
        CardOptions = new Dictionary<string, Card[]>();

        
        if (MonsterReader.Content != null){
            List <MonsterCard> mCards = new List<MonsterCard>();
            
            foreach (MonsterCardJsonItem i in MonsterReader.Content){
                mCards.Add(new MonsterCard(i));
            }

            CardOptions.Add(nameof(MonsterCard), mCards.ToArray());
        }

        if (EffectReader.Content != null){
            List <EffectCard> eCards = new List<EffectCard>();
            
            foreach (EffectCardJsonItem i in EffectReader.Content){
                eCards.Add(new EffectCard(i));
            }

            CardOptions.Add(nameof(EffectCard), eCards.ToArray());
        }
    }

    private Card GetRandomCard(Card[] cards){
        float total = 0.0f;
        float[] weights = new float[cards.Length];

        for (int i = 0; i < cards.Length; i++){
            weights[i] = cards[i].AppearingProbability;
            total += weights[i];
        }

        for (int i = 0; i < cards.Length; i++){
            weights[i] /= total;

            if (i != 0){
                weights[i] += weights[i - 1];
            }
        }

        Random random = new Random();

        float randWeight = random.NextSingle();

        for (int i = 0; i < cards.Length; i++){
            if (randWeight <= weights[i]){
                Console.WriteLine();
                return cards[i];
            }
        }

        throw new Exception("something happened");
    }
}
