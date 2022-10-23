namespace Cards;

using System;
using JsonItems;
using Elements;

/// <summary>
/// Cartas de efecto: tienen un efecto directo sobre los monstruos(propios o no)
/// pero solo se activa una vez y no realizan ataques durante los turnos
/// </summary>
public class EffectCard : Card{
    private delegate void Effect(Card target);

    Effect[]? Effects{get;set;}

    public EffectCard(string Name, string Description, float AppearingProbability, string element) : base(Name, Description, AppearingProbability, element){
        //TODO
    }

    public EffectCard(EffectCardJsonItem args) : base(args.name, args.description, args.appearingProbability, args.element){
        //TODO
    }

    public override void Play(){
        //TODO
    }

    public override void Drop(){
        //TODO
    }
}