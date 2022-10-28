namespace Match;

using System;
using Players;
using Cards;
using CardFactorys;

public enum MatchTypes{
    HumanVSHuman,
    ComputerVSHuman
}

public class Match{
    public Player player{get;private set;}
    public Player  enemy{get;private set;}

    public CardFactory Factory{get;private set;}

    private int TurnCounter{get;set;}

    public Match(MatchTypes matchType, int cardsInHand, int cardsInTable, int maxEffectCards=20, int maxMonsterCards=5){
        player = new Player(cardsInHand, cardsInTable);
        
        if (matchType == MatchTypes.HumanVSHuman){
            enemy = new Player(cardsInHand, cardsInTable);
        }
        else {
            enemy = new VirtualPlayer(cardsInHand, cardsInTable);
        }

        this.Factory = new CardFactory(maxEffectCards, maxMonsterCards);

        player.BeginTurn();

        TurnCounter = 0;
    }

    public void EndTurn() {
        player.EndTurn();
        enemy.BeginTurn();

        Player tmp = this.player;
        this.player = this.enemy;
        this.enemy  = tmp;

        this.TurnCounter ++;
    }

    public void Play(Card card, ref MonsterCard? target){
        if (target == null && player.Table != null){
            for (int i = 0; i < player.Table.Length; i++){
                if (player.Table[i] == null){
                    target = ref player.Table[i];//todo: remove this warning
                    break;
                }
            }
        }

        player.Play(card, ref target);

        enemy.RemoveDeadMonsters();
        player.RemoveDeadMonsters();
    }

    public Player? Winner(){
        bool playerFlag = true;
        bool enemyFlag  = true;

        for (int i = 0; player.Table != null && i < player.Table.Length; i++){
            if (player.Table[i] != null){
                playerFlag = false;
            }
            if (enemy.Table != null && enemy.Table[i] != null){
                enemyFlag = false;
            }
        }

        if (player.PlayerDeck != null && player.PlayerDeck.MonsterCards.Length == 0 && playerFlag)return enemy;
        if (enemy.PlayerDeck != null && enemy.PlayerDeck.MonsterCards.Length == 0 && enemyFlag)return player;

        return null;
    }

    public void Attack(int monsterIndex, ref MonsterCard target) {
        //todo
    }

    public void UsePower(int monsterIndex, ref MonsterCard? target) {
        //todo
    }
}