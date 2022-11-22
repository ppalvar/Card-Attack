namespace Powers;

using Match;
using Cards;
using Interpreter;

public class Power {
    public string Name{get;private set;}

    public string PowerCode{get;private set;}

    /// <summary>
    /// Constructor to Powers. All params are valid MLC code
    /// that will be executed when:
    /// </summary>
    /// <param name="Name">The name of this power</param>
    /// <param name="PowerToMatch">The monster uses this power (aplies to the match)</param>
    /// <param name="PowerToTarget">The monster uses this power (aplies to the target card)</param>
    /// <param name="PowerToThis">The monster uses this power (aplies to this card)</param>
    public Power(string PowerName, string PowerCode) {
        this.PowerCode = PowerCode;
        this.Name = PowerName;
    }

    public void UsePower(MonsterCard card, MonsterCard target, Match match) {
        Interpreter interpreter = new Interpreter();

        MatchState state = new MatchState(card, target, match);

        interpreter.Interpret(this.PowerCode, state);
    }
}

internal class MatchState {
    MonsterCard me{get;set;}
    MonsterCard target{get;set;}
    Match match{get;set;}

    public MatchState(MonsterCard me, MonsterCard target, Match match) {
        this.me = me;
        this.target = target;
        this.match = match;
    }
}