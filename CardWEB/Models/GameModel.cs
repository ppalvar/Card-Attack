namespace Models;

using JsonItems;
using Cards;

public class CardResponse {
    public string name{get;set;}
    public string  image{get;set;}
    public string description{get;set;}
    public int attack{get;set;} = -1;
    public int hp {get;set;} = -1;
    public int initialHp {get;set;} = -1;
    public bool isMonster{get;set;} = false;
    public string?[] powers{get;set;} = {};

    public int index{get;set;} = 0;

    public CardResponse(Card card, int index) {
        name = card.Name;
        image = card.Image;
        description = card.Description;

        this.index = index;

        isMonster = false;
        if (card is MonsterCard mCard) {
            isMonster = true;
            attack = mCard.AttackPoints;
            hp = initialHp = mCard.HP;
        }

        else if (card is EffectCard eCard) {
            this.powers = new string[] {eCard.power.Name};
        }
    }
}

public class CardNames {
    public string[] playerA{get;set;}
    public string[] playerB{get;set;}

    public CardNames(string[] playerA, string[] playerB) {
        this.playerA = playerA;
        this.playerB = playerB;
    }
}

public class TurnEndRequest {
    public bool turnEnds{get;set;} = false;
}

public class InvokeRequest {
    public int CardIndex{get;set;}

    public InvokeRequest(int CardIndex) {
        this.CardIndex = CardIndex;
    }   
}

public class ActionResponse {
    public bool canMove{get;set;} = false;
    public bool turnEnds{get; set;} = false;
    public bool gameEnds{get; set;} = false;

    public CardResponse?[]? Hand{get;set;}
    public CardResponse?[]? TableA{get;set;}
    public CardResponse?[]? TableB{get;set;}

    public string?[]? MovementLog{get;set;}

    public ActionResponse(bool canMove, bool turnEnds, bool gameEnds) {
        this.canMove = canMove;
        this.turnEnds = turnEnds;
        this.gameEnds = gameEnds;
    }
}

public class AttackRequest {
    public CardResponse card{get;set;}
    public CardResponse target{get;set;}

    public AttackRequest(CardResponse card, CardResponse target) {
        this.card = card;
        this.target = target;
    }
}