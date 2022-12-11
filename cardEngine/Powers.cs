namespace Powers;

using Match;
using Cards;
using Interpreter;

/// <summary>
/// Object that stores a power
/// </summary>
public class Power
{
    public string Name { get; private set; }

    public string PowerCode { get; private set; }

    /// <summary>
    /// Constructor to Powers. All params are valid MLC code
    /// that will be executed when:
    /// </summary>
    /// <param name="PowerName">The name of this power</param>
    /// <param name="PowerCode">The code that will be executed when the power is used</param>
    public Power(string PowerName, string PowerCode)
    {
        this.PowerCode = PowerCode;
        this.Name = PowerName;
    }

    /// <summary>
    /// Executes the code of the power using the current state of the match as context
    /// </summary>
    /// <param name="card">The card that uses the power</param>
    /// <param name="target">The card that will take damage (or not) from this attack</param>
    /// <param name="match">The state of the game</param>
    private void UsePower(MonsterCard card, MonsterCard target, Match match)
    {
        Interpreter interpreter = new Interpreter();

        MatchState state = new MatchState(card, target, match);

        interpreter.Interpret(this.PowerCode, state);
    }
}

/// <summary>
/// Object that summarizes the state of the match
/// </summary>
internal class MatchState
{
    MonsterCard me { get; set; }
    MonsterCard target { get; set; }
    Match match { get; set; }

    /// <summary>
    /// Constructor for this class
    /// </summary>
    /// <param name="me">The card that uses the power</param>
    /// <param name="target">The card that will take damage (or not) from this attack</param>
    /// <param name="match">The state of the game</param>
    public MatchState(MonsterCard me, MonsterCard target, Match match)
    {
        this.me = me;
        this.target = target;
        this.match = match;
    }
}