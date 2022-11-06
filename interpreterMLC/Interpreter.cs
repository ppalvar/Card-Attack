namespace Interpreter;

using Parser;

public class Interpreter {
    private Parser? Parser{get;set;}

    public Interpreter() {}

    public object Interpret(string Text, object Context) {
        this.Parser = new Parser(Text);
        AST? Tree = Parser.Parse();
        try {
            return NodeVisitor.Visit(Tree, Context);
        }
        catch (Exception e) {
            return new Exception(e.Message);
        }
    }
}