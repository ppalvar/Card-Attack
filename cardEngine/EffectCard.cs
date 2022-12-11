namespace Cards;

using System;
using CardJsonItems;
using Powers;

/// <summary>
/// Effect Cards class: this cards can be equiped to the monsters
/// </summary>
public class EffectCard : Card
{
    /// <summary>
    /// The power contained in this card
    /// </summary>
    public Power power { get; private set; }

    /// <summary>
    /// Creates a new EffectCard. For instance only, it will not be saved to disk
    /// </summary>
    /// <param name="Name">The name of the card</param>
    /// <param name="Description">A brief description</param>
    /// <param name="Image">The name or address of an image</param>
    /// <param name="AppearingProbability">How likely is this card to appear</param>
    /// <param name="power">The power this card will contain</param>
    public EffectCard(string Name, string Description, string Image, float AppearingProbability, Power power) : base(Name, Description, Image, AppearingProbability)
    {
        this.power = power;
    }

    /// <summary>
    /// Creates a new EffectCard. For instance only, it will not be saved to disk
    /// </summary>
    /// <param name="args">A JsonItem containing all the needed data</param>
    public EffectCard(EffectCardJsonItem args) : base(args.name, args.description, args.image, args.appearingProbability)
    {
        Power? tmp = new Power(args.powerName, args.powerCode);

        if (tmp != null) this.power = tmp;

        else
        {
            throw new Exception("something happenned");
        }
    }

    /// <summary>
    /// Aquips to a target monster the power holded by this card
    /// </summary>
    /// <param name="target">A MonsterCard</param>
    public void UseCard(MonsterCard target)
    {
        target.AssignPower(this.power);
    }
}