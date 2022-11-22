namespace Cards;

using System;
using Elements;

/// <summary>
/// Interfaz base para las cartas
/// </summary>
public abstract class Card : ICloneable{
    public string Name{get;set;}
    public string Description{get;set;}
    public string Image{get;set;}
    public float  AppearingProbability{get;set;}
    public Element element{get;set;}
    
    protected Card(string Name, string Description, string Image, float  AppearingProbability, string element){
        this.Image = Image;
        this.Name = Name;
        this.Description = Description;
        this.AppearingProbability = AppearingProbability;
        this.element = GetElement(element);
    }

    public object Clone() {
        return this.MemberwiseClone();
    }

    private Element GetElement(string element){
        switch (element){
            case ("Fire"):
                return new Fire();
            case ("Water"):
                return new Water();
            case ("Wind"):
                return new Wind();
            case ("Earth"):
                return new Earth();
            default :
                throw new ArgumentException("the name of the element must be Fire, Water, Wind or Earth");            
        }
    }

    public override string ToString()
    {
        return this.Name;
    }
}