namespace Parser;

using System;
using Lexer;
using Symbols;

public class Parser {
    private Token CurrentToken;
    private Lexer Lexer;

    private bool wasCompound = false;

    public Parser(string Text) {
        this.Lexer = new Lexer(Text);
        this.CurrentToken = this.Lexer.GetNextToken();
    }

    public AST? Parse() {
        AST? Node = Program();

        EAT(SYMBOLS.END);

        return Node;
    }

    private void EAT(SYMBOLS type) {
        if (this.CurrentToken.Type == type)this.CurrentToken = Lexer.GetNextToken();
        else throw new Exception($"unexpected \'{this.CurrentToken.Content}\' in input");
    }

    private AST? Factor() {
        Token token = this.CurrentToken;

        if (token.Type == SYMBOLS.INTEGER) {
            EAT(SYMBOLS.INTEGER);
            return new Integer(token);
        }

        else if (token.Type == SYMBOLS.L_PAREN) {
            EAT(SYMBOLS.L_PAREN);

            AST? Node = Value();

            EAT(SYMBOLS.R_PAREN);

            return Node;
        }

        else if (token.Type == SYMBOLS.PLUS || token.Type == SYMBOLS.MINUS) {
            if (token.Type == SYMBOLS.PLUS) {
                EAT(SYMBOLS.PLUS);
                return new UnaryOperator(token, Factor());
            }
            else {
                EAT(SYMBOLS.MINUS);
                return new UnaryOperator(token, Factor());
            }
        }
        else if (token.Type == SYMBOLS.TRUE || token.Type == SYMBOLS.FALSE) {
            if (token.Type == SYMBOLS.TRUE)EAT(SYMBOLS.TRUE);
            else EAT(SYMBOLS.FALSE);

            return new Boolean(token);
        }
        else if (token.Type == SYMBOLS.STRING) {
            EAT(SYMBOLS.STRING);

            return new String(token);
        }
        else if (token.Type == SYMBOLS.OBJECT) {
            return Prop();
        }
        else {
            return Variable();
        }
    }

    private AST? Term() {
        AST? Node = Factor();
        Token token = this.CurrentToken;
        
        while (token.Type == SYMBOLS.MUL || token.Type == SYMBOLS.DIV || token.Type == SYMBOLS.MOD) {
            if (token.Type == SYMBOLS.MUL) {
                EAT(SYMBOLS.MUL);
            }
            else if (token.Type == SYMBOLS.DIV) {
                EAT(SYMBOLS.DIV);
            }
            else if (token.Type == SYMBOLS.MOD) {
                EAT(SYMBOLS.MOD);
            }

            Node = (AST) new BinaryOperator(Node, token, Factor());
            token = this.CurrentToken;
        }

        return Node;
    }

    private AST? Expr() {
        AST? Node = Term();
        Token token = this.CurrentToken;

        while (token.Type == SYMBOLS.PLUS || token.Type == SYMBOLS.MINUS) {
            if (token.Type == SYMBOLS.PLUS) {
                EAT(SYMBOLS.PLUS);
            }
            else if (token.Type == SYMBOLS.MINUS) {
                EAT(SYMBOLS.MINUS);
            }

            Node = (AST) new BinaryOperator(Node, token, Term());
            token = this.CurrentToken;
        }

        return Node;
    }

    private AST? Comp() {
        AST? Node = Expr();
        
        Token token = this.CurrentToken;
        
        while (token.Type == SYMBOLS.LESS || token.Type == SYMBOLS.GREAT ||
               token.Type == SYMBOLS.L_EQUAL || token.Type == SYMBOLS.G_EQUAL ||
               token.Type == SYMBOLS.EQUAL || token.Type == SYMBOLS.DIFF) {
            
            if (token.Type == SYMBOLS.EQUAL)EAT(SYMBOLS.EQUAL);
            if (token.Type == SYMBOLS.LESS)EAT(SYMBOLS.LESS);
            if (token.Type == SYMBOLS.GREAT)EAT(SYMBOLS.GREAT);
            if (token.Type == SYMBOLS.L_EQUAL)EAT(SYMBOLS.L_EQUAL);
            if (token.Type == SYMBOLS.G_EQUAL)EAT(SYMBOLS.G_EQUAL);
            if (token.Type == SYMBOLS.DIFF)EAT(SYMBOLS.DIFF);
            
            Node = new BinaryOperator(Node, token, Expr());

            token = this.CurrentToken;
        }

        return Node;
    }

    private AST? Neg() {
        Token token = this.CurrentToken;

        if (token.Type == SYMBOLS.NOT){
            EAT(SYMBOLS.NOT);
            return new UnaryOperator(token, Comp());
        }

        return Comp();
    }

    private AST? Conj() {
        AST? Node = Neg();

        Token token = CurrentToken;

        while (token.Type == SYMBOLS.AND) {
            EAT(SYMBOLS.AND);
            Node = new BinaryOperator(Node, token, Neg());
            token = CurrentToken;
        }

        return Node;
    }

    private AST? Program() {
        AST?[]? Node = StatementList();
        EAT(SYMBOLS.END);

        return new Compound( Node);
    }

    private AST? CompoundStatement() {
        EAT(SYMBOLS.L_BRACE);

        AST?[]? Nodes = StatementList();

        AST? Root = new Compound(Nodes);

        EAT(SYMBOLS.R_BRACE);

        this.wasCompound = true;

        return Root;
    }

    private AST?[] StatementList() {
        List <AST?>? Nodes = new List<AST?>();

        AST? Node = Statement();

        Nodes.Add(Node);

        while (this.CurrentToken.Type == SYMBOLS.SEMI || wasCompound) {
            if (!wasCompound)EAT(SYMBOLS.SEMI);

            wasCompound = false;

            Node = Statement();

            Nodes.Add(Node);
        }

        return Nodes.ToArray();
    }

    private AST? Statement() {
        Token token = this.CurrentToken;
        AST? Node = null;
        
        if (token.Type == SYMBOLS.ID) {
            Node = AssignmentStatement();
        }
        else if (token.Type == SYMBOLS.INT || token.Type == SYMBOLS.STR || token.Type == SYMBOLS.BOOL) {
            Node = VariableDeclaration();
        }
        else if (token.Type == SYMBOLS.IF) {
            Node = IfStatement();
        }
        else if (token.Type == SYMBOLS.WHILE) {
            Node = WhileStatement();
        }
        else if (token.Type == SYMBOLS.OBJECT) {
            Node = Prop();
        }
        else {
            Node = Empty();
        }

        return Node;
    }

    private AST? WhileStatement() {
        EAT(SYMBOLS.WHILE);
        EAT(SYMBOLS.L_PAREN);

        AST? condition = Value();

        EAT(SYMBOLS.R_PAREN);

        AST? body = CompoundStatement();

        return new WhileCicle(condition, body);
    }

    private AST? IfStatement() {
        EAT(SYMBOLS.IF);
        EAT(SYMBOLS.L_PAREN);

        AST? condition = Value();

        EAT(SYMBOLS.R_PAREN);

        AST? body = CompoundStatement();
        AST? _else = null;

        if (this.CurrentToken.Type == SYMBOLS.ELSE) {
            EAT(SYMBOLS.ELSE);

            if (this.CurrentToken.Type == SYMBOLS.IF){
                _else = IfStatement();
            }
            else _else = CompoundStatement();
        }

        return new Conditional(condition, body, _else);
    }

    private AST? VariableDeclaration() {
        Token varType = CurrentToken;

        if (varType.Type == SYMBOLS.INT) {
            EAT(SYMBOLS.INT);
        }
        if (varType.Type == SYMBOLS.STR) {
            EAT(SYMBOLS.STR);
        }
        if (varType.Type == SYMBOLS.BOOL) {
            EAT(SYMBOLS.BOOL);
        }

        Token identifier = CurrentToken;
        EAT(SYMBOLS.ID);

        List <VarDeclaration> vars = new List<VarDeclaration>();

        while (CurrentToken.Type == SYMBOLS.COMMA) {
            EAT(SYMBOLS.COMMA);
            VarDeclaration node = new VarDeclaration(varType, identifier);
            vars.Add(node);

            identifier = CurrentToken;

            EAT(SYMBOLS.ID);
        }

        VarDeclaration lastNode = new VarDeclaration(varType, identifier);
        vars.Add(lastNode);

        return new VarDeclarationSet(vars.ToArray());
    }

    private AST? AssignmentStatement() {
        AST? Left = Variable();

        Token Op = this.CurrentToken;
        EAT(SYMBOLS.ASSIGN);
        
        if (Left != null) {
            AST? val = Value();
            return new Assign(Left, Op, val);
        }
        throw new Exception("couldn't parse var asignment");
    }

    private AST? Value() {
        AST? Node = Conj();

        Token token = CurrentToken;
        while (token.Type == SYMBOLS.OR) {
            EAT(SYMBOLS.OR);
            Node = new BinaryOperator(Node, token, Conj());
            token = CurrentToken;
        }        

        return Node;
    }

    private AST? StrExpr() {
        Token str1 = CurrentToken;
        EAT(SYMBOLS.STRING);

        AST? Node = new String(str1);

        while (CurrentToken.Type == SYMBOLS.PLUS) {
            Token op = CurrentToken;
            EAT(SYMBOLS.PLUS);

            AST? Right = new String(CurrentToken);
            EAT(SYMBOLS.STRING);

            Node = new BinaryOperator(Node, op, Right);
        }

        return Node;
    }

    private AST? Variable() {
        AST? Node = new Var(this.CurrentToken);

        EAT(SYMBOLS.ID);

        return Node;
    }

    private AST? Empty() {
        return new NoOperation();
    }

    private AST? Prop() {
        Token token = CurrentToken;

        if (token.Type == SYMBOLS.OBJECT)
            EAT(SYMBOLS.OBJECT);
        if (token.Type == SYMBOLS.ID)
            EAT(SYMBOLS.ID);
        
        if (CurrentToken.Type == SYMBOLS.L_PAREN) {
            EAT(SYMBOLS.L_PAREN);

            AST? param = Value();
            
            EAT(SYMBOLS.R_PAREN);

            if (CurrentToken.Type == SYMBOLS.DOT) {
                EAT(SYMBOLS.DOT);
                return new MethodCall(token, param, Prop());
            }
            return new MethodCall(token, param); 
        }

        if (CurrentToken.Type == SYMBOLS.L_PAREN) {
            EAT(SYMBOLS.DOT);

            return new Method(token, Prop());
        }

        return new Method(token);
    }
}