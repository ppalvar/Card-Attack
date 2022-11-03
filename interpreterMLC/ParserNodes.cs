namespace Parser;

using Lexer;

public interface AST {
    public string Type{get;set;}
}

public class BinaryOperator : AST {
    public AST? Left{get;set;}
    public Token Operator{get;set;}
    public AST? Right{get;set;}

    public string Type{get;set;}

    public BinaryOperator(AST? Left, Token Operator, AST? Right) {
        this.Left = Left;
        this.Operator = Operator;
        this.Right = Right;

        this.Type = nameof(BinaryOperator);
    }
}

public class UnaryOperator : AST {
    public Token Operator{get;set;}
    public AST? Value{get;set;}

    public string Type{get;set;}

    public UnaryOperator(Token Operator, AST? Value) {
        this.Operator = Operator;
        this.Value = Value;

        this.Type = nameof(UnaryOperator);
    }
}

public class Integer : AST {
    public Token Value{get;set;}

    public string Type{get;set;}

    public Integer(Token Value) {
        this.Value = Value;

        this.Type = nameof(Integer);
    }
}

public class Boolean : AST {
    public Token Value{get;set;}

    public string Type{get;set;}

    public Boolean(Token Value) {
        this.Value = Value;

        this.Type = nameof(Boolean);
    }
}

public class Compound : AST {
    public AST?[]? Children{get;set;}

    public string Type{get;set;}

    public Compound(AST?[]? Children) {
        this.Children = Children;

        this.Type = nameof(Compound);
    }
}

public class Assign : AST {
    public AST? Left{get;set;}
    public Token AssignOp{get;set;}
    public AST? Right{get;set;}

    public string Type {get;set;}

    public Assign(AST? Left, Token Op, AST? Right) {
        this.Left = Left;
        this.AssignOp = Op;
        this.Right = Right;

        this.Type = nameof(Assign);
    }
}

public class Var : AST {
    public Token Token{get;set;}
    public string Name{get;set;}

    public string Type{get;set;}

    public Var(Token token) {
        this.Token = token;
        this.Name = token.Content;

        this.Type = nameof(Var);
    }
}

public class NoOperation : AST {
    public string Type{get;set;}

    public NoOperation() {
        this.Type = nameof(NoOperation);
    }
}

public class VarDeclaration : AST {
    public string Type{get;set;}
    public Token DataType{get;set;}
    public Token Identifier{get;set;}


    public VarDeclaration(Token DataType, Token Identifier) {
        this.DataType = DataType;
        this.Identifier = Identifier;

        this.Type = nameof(VarDeclaration);
    }
}

public class VarDeclarationSet : AST {
    public string Type{get;set;}
    public AST?[]? Nodes{get;set;}

    public VarDeclarationSet(AST?[]? Nodes) {
        this.Nodes = Nodes;

        this.Type = nameof(VarDeclarationSet);
    }
}

public class Conditional : AST {
    public string Type{get;set;}
    public AST? Condition{get;set;}
    public AST? Body{get;set;}
    public AST? Else{get;set;}

    public Conditional(AST? Condition, AST? Body, AST? Else=null) {
        this.Condition = Condition;
        this.Body = Body;
        this.Else = Else;

        this.Type = nameof(Conditional);
    }
}

public class WhileCicle : AST {
    public string Type{get;set;}
    public AST? Condition{get;set;}
    public AST? Body{get;set;}

    public WhileCicle(AST? Condition, AST? Body) {
        this.Condition = Condition;
        this.Body = Body;

        this.Type = nameof(WhileCicle);
    }
}