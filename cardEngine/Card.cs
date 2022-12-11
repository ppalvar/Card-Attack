namespace Cards;

using System;

/// <summary>
/// Base Interface for card objects
/// </summary>
public abstract class Card : ICloneable
{
    /// <summary>
    /// The name or title of the card
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// A text that summarizes the purpose of the card
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// An image name, used to display the card in a graphic interface
    /// </summary>
    public string Image { get; set; }

    /// <summary>
    /// Higher value means it's more likely this card to appear when randomly selected
    /// </summary>
    /// <value></value>
    public float AppearingProbability { get; set; }

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="Name">The name or title of the card</param>
    /// <param name="Description">A text that summarizes the purpose of the card</param>
    /// <param name="Image">An image name, used to display the card in a graphic interface</param>
    /// <param name="AppearingProbability">Higher value means it's more likely this card to appear when randomly selected</param>
    public Card(string Name, string Description, string Image, float AppearingProbability)
    {
        this.Image = Image;
        this.Name = Name;
        this.Description = Description;
        this.AppearingProbability = AppearingProbability;
    }

    /// <summary>
    /// Makes a shallow copy of this card
    /// </summary>
    /// <returns>`Object` type instance (must be casted!)</returns>
    public object Clone()
    {
        return this.MemberwiseClone();
    }

    /// <summary>
    /// /// The card as a string
    /// </summary>
    /// <returns>The name of the card</returns>
    public override string ToString()
    {
        return this.Name;
    }
}