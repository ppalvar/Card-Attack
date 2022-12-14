namespace Players;

using Match;
using Cards;

public class VirtualPlayer : Player{
    public VirtualPlayer(int cardsInHand, int cardsInTable) : base(cardsInHand, cardsInTable){
        this.cardsInHand = cardsInHand;
        this.cardsInTable = cardsInTable;
    }

    int cardsInHand;
    int cardsInTable;
    float maxAtkEvaluation = float.MaxValue;
    Match[] movement = new Match[6];


    public override void BeginTurn(Match? match) {
        if (match != null) {
            base.BeginTurn(match);

            MakeMovement(match);

            match.EndTurn();
        }
    }

    public void MakeMovement(Match match){
        movement[0] = PlayMonster(match);

        // for(int i = 0; i < cardsInTable; i++){
        //     movement[i+1] = AddPowers(match, i);
        // }
    }

    Match PlayMonster(Match match){
        int max = 0;
        int card = 0;
        
        if(match.player.Table.Contains(null))
            for(int i = 0; i < cardsInHand; i++)
                if(match.player.Hand[i] is MonsterCard m && m.HP > max){
                    max = m.HP;
                    card = i;
                }
        // Console.WriteLine(match.player.Hand[card]);
        match.Play(card);

        return match;
    }

    Match AddPowers(Match match, int i){
        MonsterCard? m = match.player.Table[i];
        Match best = match.Clone();
        float max = float.MaxValue;

        if(m != null && m.Powers.Contains(null)){
            for(int j = 0; j < cardsInHand; j++)
                if(match.player.Hand[j] is EffectCard e && !m.Powers.Contains(e.power)){
                    Match clone = match.Clone();
                        
                    clone.Play(j, m);

                    clone = AtkCard(clone, i, j);    

                    if(max > maxAtkEvaluation){
                        best = clone;
                        max = maxAtkEvaluation;
                    }  
                }      
        } 

        return best;    
    }

    Match AtkCard(Match match, int card, int power){
        MonsterCard? m = match.player.Table[card];
        Match bestOpcion = match.Clone();

        if(m != null){
            for(int i = 0; i < cardsInTable; i++)
                if(match.enemy.Table[i] is MonsterCard enemy && enemy != null){
                    Match clone = match.Clone();
                    float n = 0;

                    if(power == m.Powers.Length){
                        clone.Attack(card, enemy);

                        n = Evaluate(clone);
                    }

                    else if(m.Powers[power] != null){
                        clone.UsePower(card,power,enemy);

                        n = Evaluate(clone);
                    }

                    if(n < maxAtkEvaluation){
                        maxAtkEvaluation = n;
                        bestOpcion = clone;
                    }
                }
        }

        return bestOpcion;
    }

    float Evaluate(Match match){
        float evaluation = 0;

        foreach(MonsterCard? i in match.enemy.Table)
            if(i != null){
                evaluation += 10 + i.HP / 4;
            }

        return evaluation;
    }
}