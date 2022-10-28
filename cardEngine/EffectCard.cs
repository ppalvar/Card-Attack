namespace Cards;

using System;
using JsonItems;
using Elements;
using IPowers;
using Powers;

/// <summary>
/// Cartas de efecto: tienen un efecto directo sobre los monstruos(propios o no)
/// pero solo se activa una vez y no realizan ataques durante los turnos
/// </summary>
public class EffectCard : Card{
    public IPower power{get;private set;}

    public EffectCard(string Name, string Description, float AppearingProbability, string element, IPower power) : base(Name, Description, AppearingProbability, element){
        this.power = power;
    }

    public EffectCard(EffectCardJsonItem args) : base(args.name, args.description, args.appearingProbability, args.element){
        IPower? tmp = PowersManager.GetPower(args.powerName, args.powerCondition);

        if (tmp != null) this.power = tmp;

        else {
            throw new Exception("something happenned");
        }
    }

    public void UseCard() {
        //todo
    }

    public void UseCard(ref MonsterCard target) {
        target.AssignPower(this.power);
    }
}