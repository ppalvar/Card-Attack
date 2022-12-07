namespace Cards;

using System;

/// <summary>
/// Interfaz base para las cartas
/// </summary>
public abstract class Card : ICloneable{
    public string Name{get;set;}
    public string Description{get;set;}
    public string Image{get;set;}
    public float  AppearingProbability{get;set;}
    
    protected Card(string Name, string Description, string Image, float  AppearingProbability){
        this.Image = Image;
        this.Name = Name;
        this.Description = Description;
        this.AppearingProbability = AppearingProbability;
    }

    public object Clone() {
        return this.MemberwiseClone();
    }

    public override string ToString()
    {
        return this.Name;
    }
}