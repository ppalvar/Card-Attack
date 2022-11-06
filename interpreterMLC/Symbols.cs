namespace Symbols;

using System;
using Lexer;

public enum SYMBOLS {
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
public abstract class ISymbol {
    public string Name{get;set;}
    public ISymbol? Type{get;set;}

    public ISymbol(string Name) {
        this.Name = Name;
    }

    public override string ToString() {
        return this.Name;
    }
}

public static class Scope {
    public static Dictionary <string, object> Global = new Dictionary<string, object>();
}

public class BuiltinDataType : ISymbol {
    public BuiltinDataType(string Name) : base(Name){
        this.Type = null;
    }
}

public class VarSymbol : ISymbol {
    public VarSymbol(string Name, ISymbol? Type)  : base(Name){
        this.Type = Type;
    }

    public override string ToString() {
        return $"<{this.Name}: {this.Type}>";
    }
}

public static class SymbolTable {
    private static Dictionary <string, ISymbol?> Table = new Dictionary<string, ISymbol?>();

    public static void Define(ISymbol? symbol) {
        if (symbol != null) {
            Table.Add(symbol.Name, symbol);
        }
    }

    public static ISymbol? Lookup(string name) {
        ISymbol? symbol = Table.GetValueOrDefault(name, null);
        
        return symbol;
    }

    public static void InitBuiltins() {
        Define(new BuiltinDataType("int"));
        Define(new BuiltinDataType("bool"));
        Define(new BuiltinDataType("str"));
    }
}