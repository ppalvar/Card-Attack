namespace Cards;

using System;

/// <summary>
/// Interfaz base para las cartas
/// </summary>
public abstract class Card{
    public string Name{get;set;}
    public string Description{get;set;}
    public float  AppearingProbability{get;set;}

    public abstract void Play();
    public abstract void Drop();

    protected Card(string Name, string Description, float  AppearingProbability){
        this.Name = Name;
        this.Description = Description;
        this.AppearingProbability = 0.0f;
    }

    public override string ToString()
    {
        return this.Name;
    }
}