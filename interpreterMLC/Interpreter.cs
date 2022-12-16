namespace Interpreter;

using Parser;

public class Interpreter
{
    private Parser? Parser { get; set; }

    public Interpreter() { }

    /// <summary>
    /// Interprets a given string input as a code script
    /// </summary>
    /// <param name="Text">The code itself</param>
    /// <param name="Context">A C# object that can be modified from the code</param>
    public void Interpret(string Text, object Context)
    {
        Symbols.SymbolTable.InitBuiltins();
        Symbols.Scope.Clear();

        this.Parser = new Parser(Text);
        AST? Tree = Parser.Parse();
        try
        {
            NodeVisitor.Visit(Tree, Context, true);
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }
}