namespace CardFactorys;

using System;
using Cards;
using CardJsonItems;
using JsonRW;
using Interpreter;

/// <summary>
/// Class that allows to read cards from the disk and manage them.
/// Includes features for show the available cards, add a new card to the files
/// and get Deck objects.
/// </summary>
public class CardFactory
{
    #region Readers
    private JsonRW<MonsterCardJsonItem>? MonsterReader { get; set; }
    private JsonRW<EffectCardJsonItem>? EffectReader { get; set; }
    #endregion

    private Dictionary<string, Card[]>? CardOptions { get; set; }
    private static int MaxCards { get; set; }
    private static int MaxMonsterCards { get; set; }
    private static int MaxEffectCards{get;set;}

    /// <summary>
    /// Creates a new instance of CardFactory
    /// </summary>
    /// <param name="maxEffectCards">The maximum amount of EffectCards the player will have</param>
    /// <param name="maxMonsterCards">The maximum amount of EffectCards the player will have</param>
    public CardFactory(int maxEffectCards = 20, int maxMonsterCards = 5)
    {
        LoadCardsFromDisk();
        MaxCards = maxEffectCards + maxMonsterCards;
        MaxMonsterCards = maxMonsterCards;
        MaxEffectCards = maxEffectCards;
    }

    /// <summary>
    /// Gets all the available cards (those already created and added to the factory and files)
    /// </summary>
    /// <returns>An array containing all available cards</returns>
    public Card[] GetCardOptions()
    {
        List<Card> options = new List<Card>();

        if (this.CardOptions != null)
        {
            foreach (Card c in this.CardOptions[nameof(MonsterCard)])
            {
                options.Add(c);
            }
            foreach (Card c in this.CardOptions[nameof(EffectCard)])
            {
                options.Add(c);
            }
        }

        return options.ToArray();
    }

    /// <summary>
    /// Adds a new type of card to the factory and saves it to the disk
    /// </summary>
    /// <param name="args">A card, as an instance of Cards.MonsterCard or Cards.EffectCard</param>
    /// <typeparam name="T">Either Cards.MonsterCard or Cards.EffectCard</typeparam>
    public void CreateCard<T>(T args) where T : Card
    {
        if (typeof(T) == typeof(MonsterCard))
        {
            MonsterCard? m = args as MonsterCard;

            if (m != null)
            {
                MonsterCardJsonItem tmp = new MonsterCardJsonItem(m.Name, m.Description, m.Image, m.AppearingProbability, m.AttackPoints, m.HP);
                if (MonsterReader != null && MonsterReader.Content != null)
                {
                    MonsterReader.Add(tmp);
                    if (this.CardOptions != null)
                    {
                        this.CardOptions[nameof(MonsterCard)] = this.CardOptions[nameof(MonsterCard)].Append(m).ToArray();
                    }
                }
            }
        }

        else
        {
            EffectCard? m = args as EffectCard;


            if (m != null)
            {
                Exception? e = Interpreter.TryParse(m.power.PowerCode);

                if (e != null) {
                    throw e;
                }

                EffectCardJsonItem tmp = new EffectCardJsonItem(m.Name, m.Description, m.Image, m.AppearingProbability, m.power.Name, m.power.PowerCode);
                if (EffectReader != null && EffectReader.Content != null)
                {
                    EffectReader.Add(tmp);
                    if (this.CardOptions != null)
                    {
                        this.CardOptions[nameof(EffectCard)] = this.CardOptions[nameof(EffectCard)].Append(m).ToArray();
                    }
                }
            }
        }
    }

    /// <summary>
    /// Gets a random Deck, using the appearing probability
    /// of each card
    /// </summary>
    /// <returns>An istance of CardsFactory.Deck</returns>
    public Deck GetDeck()
    {
        Card[] monsterCards = new Card[MaxMonsterCards];
        Card[] cards = new Card[MaxCards - MaxMonsterCards];

        for (int i = 0; i < MaxMonsterCards && CardOptions != null; i++)
        {
            monsterCards[i] = (Card)GetRandomCard(CardOptions[nameof(MonsterCard)]).Clone();
        }

        for (int i = 0; i < MaxCards - MaxMonsterCards && CardOptions != null; i++)
        {
            cards[i] = (Card)GetRandomCard(CardOptions[nameof(EffectCard)]).Clone();
        }

        return new Deck(monsterCards.Concat(cards).ToArray());
    }

    /// <summary>
    /// Makes a deck using the names of the requested cards
    /// </summary>
    /// <param name="names">The names of the cards</param>
    /// <returns>A Deck object</returns>
    public Deck GetDeck(string[] names)
    {
        List<Card> cards = new List<Card>();

        if (CardOptions != null)
        {
            foreach (Card c in CardOptions[nameof(MonsterCard)])
            {
                if (names.Contains(c.Name))
                {
                    cards.Add(c);
                }
            }
            foreach (Card c in CardOptions[nameof(EffectCard)])
            {
                if (names.Contains(c.Name))
                {
                    cards.Add(c);
                }
            }
        }

        return GetDeck(cards.ToArray());
    }

    /// <summary>
    /// Gets a Deck containing all the cards specified by the user
    /// </summary>
    /// <param name="requestedCards">The cards you wanna have in the deck</param>
    /// <returns>An istance of CardsFactory.Deck</returns>
    public Deck GetDeck(Card[] requestedCards)
    {
        Card[] monsterCards = new Card[MaxMonsterCards];
        Card[] cards = new Card[MaxCards - MaxMonsterCards];

        int j = 0, k = 0;
        try
        {
            for (int i = 0; i < requestedCards.Length; i++)
            {
                if (requestedCards[i] is MonsterCard && j < MaxMonsterCards)
                {
                    monsterCards[j++] = (Card)requestedCards[i].Clone();
                }

                if (requestedCards[i] is EffectCard && k < MaxEffectCards)
                {
                    cards[k++] = (Card)requestedCards[i].Clone();
                }
            }
        }
        catch (ArgumentException)
        {
            throw new Exception("the requested deck cannot be created");
        }

        for (; j < MaxMonsterCards && CardOptions != null; j++)
        {
            monsterCards[j] = (Card)GetRandomCard(CardOptions[nameof(MonsterCard)]).Clone();
        }

        for (; k < MaxCards - MaxMonsterCards && CardOptions != null; k++)
        {
            cards[k] = (Card)GetRandomCard(CardOptions[nameof(EffectCard)]).Clone();
        }

        return new Deck(monsterCards.Concat(cards).ToArray());
    }

    /// <summary>
    /// Reads the cards from the disk and initializes the Factory
    /// </summary>
    private void LoadCardsFromDisk()
    {
        MonsterReader = new JsonRW<MonsterCardJsonItem>("../cardsDB/" + nameof(MonsterCard) + ".json");
        EffectReader = new JsonRW<EffectCardJsonItem>("../cardsDB/" + nameof(EffectCard) + ".json");

        CardOptions = new Dictionary<string, Card[]>();


        if (MonsterReader.Content != null)
        {
            List<MonsterCard> mCards = new List<MonsterCard>();

            foreach (MonsterCardJsonItem i in MonsterReader.Content)
            {
                mCards.Add(new MonsterCard(i));
            }

            CardOptions.Add(nameof(MonsterCard), mCards.ToArray());
        }

        if (EffectReader.Content != null)
        {
            List<EffectCard> eCards = new List<EffectCard>();

            foreach (EffectCardJsonItem i in EffectReader.Content)
            {
                eCards.Add(new EffectCard(i));
            }

            CardOptions.Add(nameof(EffectCard), eCards.ToArray());
        }
    }

    /// <summary>
    /// Returns a randomized card, using it's appearing probability, from an specified set of cards
    /// </summary>
    /// <param name="cards">The actual set of cards</param>
    /// <returns>A card</returns>
    private Card GetRandomCard(Card[] cards)
    {

        /// <summary>
        /// Let weights[i] be an array with the cummulative probability 
        /// each card has for appearing; normalizing this values we have that
        /// 0 <= max{weights} <= 1
        /// 
        /// Then pick a random number r in that interval;
        /// If for some i, weights[i] >= r, then the card in position i has more probability
        /// of appearing and will be returned
        /// 
        /// </summary>

        float total = 0.0f;
        float[] weights = new float[cards.Length];

        for (int i = 0; i < cards.Length; i++)
        {
            weights[i] = cards[i].AppearingProbability;
            total += weights[i];
        }

        for (int i = 0; i < cards.Length; i++)
        {
            ///Normalizing the weights
            weights[i] /= total;

            if (i != 0)
            {
                weights[i] += weights[i - 1];
            }
        }

        Random random = new Random();

        float randWeight = random.NextSingle();

        for (int i = 0; i < cards.Length; i++)
        {
            if (randWeight <= weights[i])
            {
                return cards[i];
            }
        }

        throw new Exception("something happened");
    }
}
