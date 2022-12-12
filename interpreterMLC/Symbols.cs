namespace Symbols;

/// <summary>
/// All the token types as values in a enum
/// </summary>
public enum SYMBOLS
{
    #region Math symbols
    PLUS, MINUS, MUL, DIV, MOD,
    #endregion

    #region Bool Operators
    AND, OR, NOT, TRUE, FALSE,
    #endregion

    #region Comparators
    LESS, GREAT, EQUAL, L_EQUAL, G_EQUAL, DIFF,
    #endregion

    #region Delimiters
    SEMI, L_PAREN, R_PAREN, L_BRACE, R_BRACE, COMMA, DOT,
    #endregion

    #region Data types
    INTEGER, STRING, BOOLEAN, OBJECT,
    #endregion

    #region Others
    END, ID, ASSIGN,
    #endregion

    #region Reserved Keywords
    INT, STR, BOOL, IF, ELSE, WHILE
    #endregion
}

/// <summary>
/// Base interface for defining symbols
/// </summary>
public abstract class ISymbol
{
    public string Name { get; set; }
    public ISymbol? Type { get; set; }

    public ISymbol(string Name)
    {
        this.Name = Name;
    }

    public override string ToString()
    {
        return this.Name;
    }
}

/// <summary>
/// Here are stored all variables from the program 
/// </summary>
public static class Scope
{
    public static Dictionary<string, object> Global = new Dictionary<string, object>();

    public static void Clear()
    {
        Global.Clear();
    }
}

/// <summary>
/// Represents all the default data types like int, string and bool
/// </summary>
public class BuiltinDataType : ISymbol
{
    public BuiltinDataType(string Name) : base(Name)
    {
        this.Type = null;
    }
}

/// <summary>
/// Represents a variable as a symbol
/// </summary>
public class VarSymbol : ISymbol
{
    public VarSymbol(string Name, ISymbol? Type) : base(Name)
    {
        this.Type = Type;
    }

    public override string ToString()
    {
        return $"<{this.Name}: {this.Type}>";
    }
}

/// <summary>
/// The symbol table for runtime type checking
/// </summary>
public static class SymbolTable
{
    private static Dictionary<string, ISymbol?> Table = new Dictionary<string, ISymbol?>();

    public static void Define(ISymbol? symbol)
    {
        if (symbol != null)
        {
            Table.Add(symbol.Name, symbol);
        }
    }

    public static ISymbol? Lookup(string name)
    {
        ISymbol? symbol = Table.GetValueOrDefault(name, null);

        return symbol;
    }

    public static void InitBuiltins()
    {
        Table.Clear();
        Define(new BuiltinDataType("int"));
        Define(new BuiltinDataType("bool"));
        Define(new BuiltinDataType("string"));
    }
}