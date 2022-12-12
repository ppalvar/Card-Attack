namespace Lexer;

using Symbols;

/// <summary>
/// Represents a sigle token in the input code
/// </summary>
public class Token
{
    /// <summary>
    /// The type of this token
    /// </summary>
    public SYMBOLS Type { get; private set; }

    /// <summary>
    /// The content of this token as a string
    /// </summary>
    public string Content { get; private set; }

    /// <summary>
    /// Initializes a new Token instance
    /// </summary>
    /// <param name="Type">The type of the token as a value from the SYMBOLS enum</param>
    /// <param name="Content">A string with the content of the token</param>
    public Token(SYMBOLS Type, string Content)
    {
        this.Type = Type;
        this.Content = Content;
    }

    #region Parsers
    public int ToInt32()
    {
        return int.Parse(this.Content);
    }

    public long ToInt64()
    {
        return long.Parse(this.Content);
    }

    public float ToFloat()
    {
        return float.Parse(this.Content);
    }

    public double ToDouble()
    {
        return double.Parse(this.Content);
    }

    public bool AsBoolean()
    {
        return this.Content == "True";
    }

    public object AsObject()
    {
        return (object)Content;
    }
    #endregion
}