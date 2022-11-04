namespace Lexer;

using System;

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
        SEMI, L_PAREN, R_PAREN, L_BRACE, R_BRACE, COMMA,
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

public class Token {
    public SYMBOLS Type{get;private set;}
    public string Content{get;private set;}

    public Token (SYMBOLS Type, string Content){
        this.Type = Type;
        this.Content = Content;
    }

    #region Parsers
        public int ToInt32() {
            return int.Parse(this.Content);
        }

        public long ToInt64() {
            return long.Parse(this.Content);
        }

        public float ToFloat() {
            return float.Parse(this.Content);
        }

        public double ToDouble() {
            return double.Parse(this.Content);
        }

        public bool AsBoolean() {
            return this.Content == "True";
        }

        public object? AsObject() {
            return new object();//todo: complete this
        }
    #endregion
}

public class Lexer {
    private string Text{get;set;}
    private int Position{get;set;}
    private char CurrentChar{get;set;}

    public static Dictionary<string, Token> ReservedKeywords{get;} = new Dictionary<string, Token>{
        {"int" , new Token(SYMBOLS.INT, "int")},
        {"str" , new Token(SYMBOLS.STR, "str")},
        {"bool", new Token(SYMBOLS.BOOL, "bool")},
        {"if" , new Token(SYMBOLS.IF, "if")},
        {"else" , new Token(SYMBOLS.ELSE, "else")},
        {"while" , new Token(SYMBOLS.WHILE, "while")},
    };

    public Lexer(string Text) {
        this.Text = Text;
        this.Position = 0;
        this.CurrentChar = Text[0];
    }

    private void Advance(int steps=1) {
        this.Position += steps;
        if (Position < this.Text.Length)this.CurrentChar = this.Text[this.Position];
        else this.CurrentChar = '#';
    }

    private char Peek() {
        int tmpPosition = this.Position + 1;
        if (tmpPosition < this.Text.Length)return this.Text[tmpPosition];
        return '#';
    }

    public void SkipWhitespaces() {
        while (this.CurrentChar == ' ' || this.CurrentChar == '\t' || this.CurrentChar == '\n' || this.CurrentChar == '\r') {
            Advance();
        }
    }

    private Token Integer() {
        string ans = "";

        while (IsNumber(this.CurrentChar)) {
            ans += this.CurrentChar;
            Advance();
        }

        return new Token(SYMBOLS.INTEGER, ans);
    }

    public Token GetNextToken() {
        SkipWhitespaces();
        while (this.CurrentChar != '#') {
            #region Symbols

                if (IsNumber(this.CurrentChar)){
                    return Integer();
                }

                if (this.CurrentChar == '+') {
                    Advance();
                    return new Token(SYMBOLS.PLUS, "+");
                }

                if (this.CurrentChar == '-') {
                    Advance();
                    return new Token(SYMBOLS.MINUS, "-");
                }

                if (this.CurrentChar == '*') {
                    Advance();
                    return new Token(SYMBOLS.MUL, "*");
                }

                if (this.CurrentChar == '/') {
                    Advance();
                    return new Token(SYMBOLS.DIV, "/");
                }

                if (this.CurrentChar == '%') {
                    Advance();
                    return new Token(SYMBOLS.MOD, "%");
                }

                if (this.CurrentChar == '(') {
                    Advance();
                    return new Token(SYMBOLS.L_PAREN, "(");
                }

                if (this.CurrentChar == '{') {
                    Advance();
                    return new Token(SYMBOLS.L_BRACE, "{");
                }

                if (this.CurrentChar == '}') {
                    Advance();
                    return new Token(SYMBOLS.R_BRACE, "}");
                }

                if (this.CurrentChar == ')') {
                    Advance();
                    return new Token(SYMBOLS.R_PAREN, ")");
                }

                if (this.CurrentChar == '!') {
                    Advance();
                    return new Token(SYMBOLS.NOT, "!");
                }

                if (this.CurrentChar == '&') {
                    Advance();
                    return new Token(SYMBOLS.AND, "&");
                }

                if (this.CurrentChar == '|') {
                    Advance();
                    return new Token(SYMBOLS.OR, "|");
                }

                if (this.CurrentChar == ',') {
                    Advance();
                    return new Token(SYMBOLS.COMMA, ",");
                }

                if (this.CurrentChar == '#') {
                    Advance();
                    return new Token(SYMBOLS.END, "#");
                }
            #endregion

            #region Booleans
                if (GetSubText("True".Length) == "True") {
                    Advance("True".Length);
                    return new Token(SYMBOLS.TRUE, "True");
                }

                if (GetSubText("False".Length) == "False") {
                    Advance("False".Length);
                    return new Token(SYMBOLS.FALSE, "False");
                }
            #endregion

            #region Comparators
                if (this.CurrentChar == '<') {
                    if (Peek() != '=') {
                        Advance();
                        return new Token(SYMBOLS.LESS, "<");
                    }
                    else {
                        Advance(2);
                        return new Token(SYMBOLS.L_EQUAL, "<=");
                    }
                }
                if (this.CurrentChar == '>') {
                    if (Peek() != '=') {
                        Advance();
                        return new Token(SYMBOLS.GREAT, ">");
                    }
                    else {
                        Advance(2);
                        return new Token(SYMBOLS.G_EQUAL, ">=");
                    }
                }

                if (this.CurrentChar == '=' && Peek() == '=') {
                    Advance(2);
                    return new Token(SYMBOLS.EQUAL, "==");
                }

                if (this.CurrentChar == '!' && Peek() == '=') {
                    Advance(2);
                    return new Token(SYMBOLS.DIFF, "!=");
                }
            #endregion

            #region Identifiers
                if (IsAlphaNum(CurrentChar)) {
                    string ID = GetIdentifier();

                    if (ReservedKeywords.ContainsKey(ID)) {
                        return ReservedKeywords[ID];
                    }

                    return new Token(SYMBOLS.ID, ID);
                }
            #endregion

            #region Others
                if (this.CurrentChar == '=' && Peek() != '=') {
                    Advance();
                    return new Token(SYMBOLS.ASSIGN, "=");
                }

                if (this.CurrentChar == ';') {
                    Advance();
                    return new Token(SYMBOLS.SEMI, ";");
                }

                if (this.CurrentChar == '\'' || this.CurrentChar == '\"') {
                    string str = GetString();
                    return new Token(SYMBOLS.STRING, str);
                }
            #endregion
        }
        return new Token(SYMBOLS.END, "#");
    }

    private bool IsNumber(char c) {
        return ((byte)this.CurrentChar) >= 48 && ((byte)this.CurrentChar) <= 57;
    }

    private bool IsAlphaNum(char c) {
        const string AlphaNum = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789_";
        return AlphaNum.Contains(c);
    }

    private string GetSubText(int Length) {
        int tmpPosition = this.Position - 1;

        string ans = "";

        while ((Length --) != 0) {
            if ((++tmpPosition) < this.Text.Length) {
                ans += Text[tmpPosition];
            }
        }

        return ans;
    }

    private string GetIdentifier() {
        string id = "";
        
        
        while (IsAlphaNum(this.CurrentChar)) {
            id += this.CurrentChar;
            Advance();
        }

        return id;
    }

    private string GetString() {
        string str = "";
        char delimiter = ' ';
        if (CurrentChar == '\"' || CurrentChar == '\'') {
            delimiter = CurrentChar;
            Advance();

            while (CurrentChar != delimiter) {
                str += CurrentChar;
                Advance();
            }

            Advance();
        }

        return str;
    }
}