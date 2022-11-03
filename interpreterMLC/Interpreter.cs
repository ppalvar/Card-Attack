namespace Interpreter;

using Parser;

public class Interpreter {
    private Parser Parser{get;set;}

    public Interpreter(string Text) {
        this.Parser = new Parser(Text);
    }

    public object Interpret() {
        AST? Tree = Parser.Parse();
        return NodeVisitor.Visit(Tree);
    }
}