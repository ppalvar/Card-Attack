namespace Cards;

using System;
using JsonItems;

/// <summary>
/// Cartas de efecto: tienen un efecto directo sobre los monstruos(propios o no)
/// pero solo se activa una vez y no realizan ataques durante los turnos
/// </summary>
public class EffectCard : Card{
    private delegate void Effect(Card target);

    Effect[]? Effects{get;set;}

    public EffectCard(string Name, string Description, float AppearingProbability) : base(Name, Description, AppearingProbability){
        //TODO
    }

    public EffectCard(EffectCardItem args) : base(args.name, args.description, args.appearingProbability){
        //TODO
    }

    public override void Play(){
        //TODO
    }

    public override void Drop(){
        //TODO
    }
}