namespace Players;

using Match;
using Cards;

/// <summary>
/// Player that auto-plays when it's turn begins
/// </summary>
public class VirtualPlayer : Player{
    /// <summary>
    /// The movements that are made by this player
    /// </summary>
    /// <value>List of strings describing each movement</value>
    public List<string> MovementLog{get;private set;}

    int cardsInHand;
    int cardsInTable;
    
    /// <summary>
    /// Create a new instance of virtual player
    /// </summary>
    /// <param name="cardsInHand">The number of cards this player will have in the hand</param>
    /// <param name="cardsInTable">The number of cards this player will have in the table</param>
    public VirtualPlayer(int cardsInHand, int cardsInTable) : base(cardsInHand, cardsInTable){
        this.cardsInHand = cardsInHand;
        this.cardsInTable = cardsInTable;
        MovementLog = new List<string>();
    }

    /// <summary>
    /// Begins the turn, makes movements automatically and finally ends the turn
    /// </summary>
    /// <param name="match">The match where is currently playing</param>
    public override void BeginTurn(Match? match) {
        if (match != null) {
            base.BeginTurn(match);

            MovementLog.Clear();
            bool turnEnds = MakeMovement(match);

            if (!turnEnds)match.EndTurn();
        }
    }

    /// <summary>
    /// Decides and makes movements in a step-by-step approach:
    /// First summons max 3 monsters
    /// Then, randomly adds powers to the monsters in the table (max 3)
    /// Then, uses randomly 0 to 3 powers
    /// Finally attacks following a greedy strategy: always attack the weakest card
    /// </summary>
    /// <param name="match">The match where is currently playing</param>
    /// <returns>True if the turn ends, false otherwise</returns>
    private bool MakeMovement(Match match){
        //step #1: summon monsters

        bool mustDrop = true;

        bool turnEnds = false;

        bool flag = true;
        int maxInvokes = 3;
        while (Table.Contains(null) && flag && (maxInvokes--) > 0) {
            for (int i = 0; i < cardsInHand; i++) {
                flag = false;
                if (Hand[i] is MonsterCard monster) {
                    flag = true;
                    match.Play(i);
                    mustDrop = false;
                    MovementLog.Add($"Player B played {monster}");
                    break;
                }
            }
        }

        //step #2: add powers (randomly)

        int maxPoweAdds = 3;
        for (int i = 0; i < cardsInHand && maxPoweAdds > 0; i++) {
            if (Hand[i] is EffectCard effect) {
                foreach (MonsterCard? monster in Table) {
                    if (monster != null) {
                        try {
                            match.Play(i, monster);
                            MovementLog.Add($"Player B equiped effect {effect} on {monster}");
                            maxPoweAdds--;
                            mustDrop = false;
                            break;
                        }
                        catch {
                            // nada
                        }
                    }
                }
            }
        }

        //step #3: attack

        if (match.TurnCounter >= 2) {
            //step 3.1: use powers
            Random rand = new Random();

            int maxPowerUses = rand.Next(0, 3);

            for (int _ = 0; _ < maxPowerUses; _++) {
                int monsterIndex = rand.Next(0, cardsInTable);

                while (monsterIndex < cardsInTable && Table[monsterIndex] == null) {
                    monsterIndex++;
                }

                if (monsterIndex < cardsInTable && Table[monsterIndex] is MonsterCard monster) {
                    int powerIndex = rand.Next(0, MonsterCard.MaxPowers);
                    int steps = 0;

                    while (monster.Powers[powerIndex] == null && steps < MonsterCard.MaxPowers) {
                        powerIndex = (powerIndex + 1) % MonsterCard.MaxPowers;
                        steps ++;
                    }

                    if (steps < MonsterCard.MaxPowers) {
                        for (int i = 0; i < match.enemy.Table.Length; i++) {
                            if (match.enemy.Table[i] is MonsterCard target) {
                                Powers.Power? p = monster.Powers[powerIndex];
                                if (p != null) {
                                    try {
                                        turnEnds = turnEnds || match.UsePower(monsterIndex, powerIndex, target);
                                        MovementLog.Add($"Player B: {monster} used {p.Name} on {target}");
                                        mustDrop = false;
                                        break;
                                    }
                                    catch {
                                        // nothing
                                    }
                                }
                            }
                        }
                    }
                }
            }
            //step 3.2: attack to monsters

            for (int i = 0; i < cardsInTable; i++) {
                int minLife = int.MaxValue;
                MonsterCard? target = null;

                for (int j = 0; j < match.enemy.Table.Length; j++) {
                    if (match.enemy.Table[j] is MonsterCard monster && monster.HP < minLife) {
                        minLife = monster.HP;
                        target = monster;
                    }
                }

                if (target != null && Table[i] != null) {
                    try {
                        turnEnds = turnEnds || match.Attack(i, target);
                        MovementLog.Add($"Player B: {Table[i]} attacked {target}");
                        mustDrop = false;
                    }
                    catch {
                        //nothing
                    }
                }
            }
        }

        if (mustDrop) {
            int dropCount = 3;

            Random rand = new Random();
            List <int> cardIndexes = new List<int>();

            for (int i = 0; i < cardsInTable; i++) {
                if (Hand[i] != null)cardIndexes.Add(i);
            }

            while (dropCount --> 0 && cardIndexes.Count > 0) {
                int cardIndex = cardIndexes[rand.Next(0, cardIndexes.Count)];
                
                cardIndexes = cardIndexes.Where(val => val != cardIndex).ToList();

                Card? c = Hand[cardIndex];

                match.DropCard(cardIndex);

                MovementLog.Add($"Player B dropped the card {c}");
            }
        }

        return turnEnds;
    }
}