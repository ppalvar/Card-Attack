namespace Interpreter;

using Parser;

public class Interpreter
{
    /// <summary>
    /// Interprets a given string input as a code script
    /// </summary>
    /// <param name="Text">The code itself</param>
    /// <param name="Context">A C# object that can be modified from the code</param>
    public static void Interpret(string Text, object Context)
    {
        Symbols.SymbolTable.InitBuiltins();
        Symbols.Scope.Clear();

        Parser parser = new Parser(Text);
        AST? Tree = parser.Parse();
        try
        {
            NodeVisitor.Visit(Tree, Context, true);
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }

    public static Exception? TryParse(string Text) {
        Symbols.SymbolTable.InitBuiltins();
        Symbols.Scope.Clear();

        Parser parser = new Parser(Text);
        try {
            AST? Tree = parser.Parse();
        }
        catch (Exception e) {
            return e;
        }

        return null;
    }
}