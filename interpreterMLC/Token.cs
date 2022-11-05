namespace Lexer;

using Symbols;

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