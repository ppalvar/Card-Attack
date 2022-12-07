namespace Cards;

using System;
using JsonItems;
using Powers;

/// <summary>
/// Cartas de efecto: tienen un efecto directo sobre los monstruos(propios o no)
/// pero solo se activa una vez y no realizan ataques durante los turnos
/// </summary>
public class EffectCard : Card{
    public Power power{get;private set;}

    public EffectCard(string Name, string Description, string Image, float AppearingProbability, Power power) : base(Name, Description, Image, AppearingProbability){
        this.power = power;
    }

    public EffectCard(EffectCardJsonItem args) : base(args.name, args.description, args.image, args.appearingProbability){
        Power? tmp = new Power(args.powerName, args.powerCode);

        if (tmp != null) this.power = tmp;

        else {
            throw new Exception("something happenned");
        }
    }

    /// <summary>
    /// Aquips to a target monster the power holded by this card
    /// </summary>
    /// <param name="target">A MonsterCard</param>
    public void UseCard(MonsterCard target) {
        target.AssignPower(this.power);
    }
}