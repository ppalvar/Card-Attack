namespace Lexer;

using Symbols;

/// <summary>
/// Tokenizer for the Mini Languaje. It decomposes an input string in tokens
/// of different types such as integers, strings, bools, identifiers, if/while statements.
/// </summary>
public class Lexer
{
    private string Text { get; set; }
    private int Position { get; set; }
    private char CurrentChar { get; set; }

    /// <summary>
    /// All reserved keywords stored as pairs in the shape:
    /// "ReservedKeyword" : Token_for_this_keyword;
    /// </summary>
    private static Dictionary<string, Token> ReservedKeywords { get; } = new Dictionary<string, Token>{
        {"int" , new Token(SYMBOLS.INT, "int")},
        {"string" , new Token(SYMBOLS.STR, "string")},
        {"bool", new Token(SYMBOLS.BOOL, "bool")},
        {"if" , new Token(SYMBOLS.IF, "if")},
        {"else" , new Token(SYMBOLS.ELSE, "else")},
        {"while" , new Token(SYMBOLS.WHILE, "while")},
    };

    /// <summary>
    /// Initializes a new Lexer
    /// </summary>
    /// <param name="Text">The text you want to be interpreted</param>
    public Lexer(string Text)
    {
        this.Text = Text;
        this.Position = 0;
        this.CurrentChar = Text[0];
    }

    /// <summary>
    /// Gets the next token in the input
    /// </summary>
    /// <returns>Token object instance</returns>
    public Token GetNextToken()
    {
        SkipWhitespaces();
        while (this.CurrentChar != '#')
        {
            #region Symbols

            if (IsNumber(this.CurrentChar))
            {
                return Integer();
            }

            if (this.CurrentChar == '+')
            {
                Advance();
                return new Token(SYMBOLS.PLUS, "+");
            }

            if (this.CurrentChar == '-')
            {
                Advance();
                return new Token(SYMBOLS.MINUS, "-");
            }

            if (this.CurrentChar == '*')
            {
                Advance();
                return new Token(SYMBOLS.MUL, "*");
            }

            if (this.CurrentChar == '/' && Peek() != '/')
            {
                Advance();
                return new Token(SYMBOLS.DIV, "/");
            }

            if (this.CurrentChar == '%')
            {
                Advance();
                return new Token(SYMBOLS.MOD, "%");
            }

            if (this.CurrentChar == '(')
            {
                Advance();
                return new Token(SYMBOLS.L_PAREN, "(");
            }

            if (this.CurrentChar == '{')
            {
                Advance();
                return new Token(SYMBOLS.L_BRACE, "{");
            }

            if (this.CurrentChar == '}')
            {
                Advance();
                return new Token(SYMBOLS.R_BRACE, "}");
            }

            if (this.CurrentChar == ')')
            {
                Advance();
                return new Token(SYMBOLS.R_PAREN, ")");
            }

            if (this.CurrentChar == '!' && Peek() != '=')
            {
                Advance();
                return new Token(SYMBOLS.NOT, "!");
            }

            if (this.CurrentChar == '&')
            {
                Advance();
                return new Token(SYMBOLS.AND, "&");
            }

            if (this.CurrentChar == '|')
            {
                Advance();
                return new Token(SYMBOLS.OR, "|");
            }

            if (this.CurrentChar == ',')
            {
                Advance();
                return new Token(SYMBOLS.COMMA, ",");
            }

            if (this.CurrentChar == '.')
            {
                Advance();
                return new Token(SYMBOLS.DOT, ".");
            }

            if (this.CurrentChar == '@')
            {
                Advance();
                return new Token(SYMBOLS.OBJECT, GetIdentifier());
            }

            if (this.CurrentChar == '/' && Peek() == '/')
            {
                SkipComment();
            }

            if (this.CurrentChar == '#')
            {
                Advance();
                return new Token(SYMBOLS.END, "#");
            }
            #endregion

            #region Booleans
            if (GetSubText("true".Length) == "true")
            {
                Advance("true".Length);
                return new Token(SYMBOLS.TRUE, "true");
            }

            if (GetSubText("false".Length) == "false")
            {
                Advance("false".Length);
                return new Token(SYMBOLS.FALSE, "false");
            }
            #endregion

            #region Comparators
            if (this.CurrentChar == '<')
            {
                if (Peek() != '=')
                {
                    Advance();
                    return new Token(SYMBOLS.LESS, "<");
                }
                else
                {
                    Advance(2);
                    return new Token(SYMBOLS.L_EQUAL, "<=");
                }
            }
            if (this.CurrentChar == '>')
            {
                if (Peek() != '=')
                {
                    Advance();
                    return new Token(SYMBOLS.GREAT, ">");
                }
                else
                {
                    Advance(2);
                    return new Token(SYMBOLS.G_EQUAL, ">=");
                }
            }

            if (this.CurrentChar == '=' && Peek() == '=')
            {
                Advance(2);
                return new Token(SYMBOLS.EQUAL, "==");
            }

            if (this.CurrentChar == '!' && Peek() == '=')
            {
                Advance(2);
                return new Token(SYMBOLS.DIFF, "!=");
            }
            #endregion

            #region Identifiers
            if (IsAlphaNum(CurrentChar))
            {
                string ID = GetIdentifier();

                if (ReservedKeywords.ContainsKey(ID))
                {
                    return ReservedKeywords[ID];
                }

                return new Token(SYMBOLS.ID, ID);
            }
            #endregion

            #region Others
            if (this.CurrentChar == '=' && Peek() != '=')
            {
                Advance();
                return new Token(SYMBOLS.ASSIGN, "=");
            }

            if (this.CurrentChar == ';')
            {
                Advance();
                return new Token(SYMBOLS.SEMI, ";");
            }

            if (this.CurrentChar == '\'' || this.CurrentChar == '\"')
            {
                string str = GetString();
                return new Token(SYMBOLS.STRING, str);
            }
            #endregion
        }
        return new Token(SYMBOLS.END, "#");
    }

    /// <summary>
    /// Checks if a character is a digit
    /// </summary>
    private bool IsNumber(char c)
    {
        return ((byte)this.CurrentChar) >= 48 && ((byte)this.CurrentChar) <= 57;
    }

    /// <summary>
    /// Checks if a given char is a letter, number or underscore
    /// </summary>
    private bool IsAlphaNum(char c)
    {
        const string AlphaNum = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789_";
        return AlphaNum.Contains(c);
    }

    /// <summary>
    /// Gets a substring starting in the current position
    /// </summary>
    /// <param name="Length">The lenght of the substring</param>
    private string GetSubText(int Length)
    {
        int tmpPosition = this.Position - 1;

        string ans = "";

        while ((Length--) != 0)
        {
            if ((++tmpPosition) < this.Text.Length)
            {
                ans += Text[tmpPosition];
            }
        }

        return ans;
    }

    /// <summary>
    /// Gets an identifier or variable name
    /// </summary>
    private string GetIdentifier()
    {
        string id = "";


        while (IsAlphaNum(this.CurrentChar))
        {
            id += this.CurrentChar;
            Advance();
        }

        return id;
    }

    /// <summary>
    /// Gets a string defining it as all the characters between two quotes
    /// or single quotes
    /// </summary>
    /// <returns>The content of the string</returns>
    private string GetString()
    {
        string str = "";
        char delimiter = ' ';
        if (CurrentChar == '\"' || CurrentChar == '\'')
        {
            delimiter = CurrentChar;
            Advance();

            while (CurrentChar != delimiter)
            {
                str += CurrentChar;
                Advance();
            }

            Advance();
        }

        return str;
    }

    /// <summary>
    /// Jumps to the end of the current line
    /// </summary>
    private void SkipComment()
    {
        while (CurrentChar != '\n') Advance();
    }

    /// <summary>
    /// Moves forward the current position
    /// </summary>
    /// <param name="steps">The number of characters you wanna advance</param>
    private void Advance(int steps = 1)
    {
        this.Position += steps;
        if (Position < this.Text.Length) this.CurrentChar = this.Text[this.Position];
        else this.CurrentChar = '#';
    }

    /// <summary>
    /// Gets the character after the current one without moving forward
    /// </summary>
    /// <returns>The next character</returns>
    private char Peek()
    {
        int tmpPosition = this.Position + 1;
        if (tmpPosition < this.Text.Length) return this.Text[tmpPosition];
        return '#';
    }

    /// <summary>
    /// Ignores all the whitespace, new lines and tab characters and moves forward
    /// </summary>
    private void SkipWhitespaces()
    {
        while (this.CurrentChar == ' ' || this.CurrentChar == '\t' || this.CurrentChar == '\n' || this.CurrentChar == '\r')
        {
            Advance();
        }
    }

    /// <summary>
    /// Gets a substring containing only numbers as an integer
    /// </summary>
    /// <returns>An integer starting in the current position</returns>
    private Token Integer()
    {
        string ans = "";

        while (IsNumber(this.CurrentChar))
        {
            ans += this.CurrentChar;
            Advance();
        }

        return new Token(SYMBOLS.INTEGER, ans);
    }
}