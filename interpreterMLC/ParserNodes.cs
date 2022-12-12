namespace Parser;

using Lexer;

public interface AST
{
    public string Type { get; set; }
}

/// <summary>
/// Represents every operator in the form TOKEN [operator] TOKEN
/// </summary>
public class BinaryOperator : AST
{
    public AST? Left { get; set; }
    public Token Operator { get; set; }
    public AST? Right { get; set; }

    public string Type { get; set; }

    public BinaryOperator(AST? Left, Token Operator, AST? Right)
    {
        this.Left = Left;
        this.Operator = Operator;
        this.Right = Right;

        this.Type = nameof(BinaryOperator);
    }
}

/// <summary>
/// Represents every operator in the form [operator] TOKEN
/// </summary>
public class UnaryOperator : AST
{
    public Token Operator { get; set; }
    public AST? Value { get; set; }

    public string Type { get; set; }

    public UnaryOperator(Token Operator, AST? Value)
    {
        this.Operator = Operator;
        this.Value = Value;

        this.Type = nameof(UnaryOperator);
    }
}

/// <summary>
/// Represents an integer value
/// </summary>
public class Integer : AST
{
    public Token Value { get; set; }

    public string Type { get; set; }

    public Integer(Token Value)
    {
        this.Value = Value;

        this.Type = nameof(Integer);
    }
}

/// <summary>
/// Represents a boolean value
/// </summary>
public class Boolean : AST
{
    public Token Value { get; set; }

    public string Type { get; set; }

    public Boolean(Token Value)
    {
        this.Value = Value;

        this.Type = nameof(Boolean);
    }
}

/// <summary>
/// Represents a compound statement. This is a statement made 
/// of other simpler statements like the body of IF and WHILE statements
/// </summary>
public class Compound : AST
{
    public AST?[]? Children { get; set; }

    public string Type { get; set; }

    public Compound(AST?[]? Children)
    {
        this.Children = Children;

        this.Type = nameof(Compound);
    }
}

/// <summary>
/// Represents a value asignment to a variable
/// </summary>
public class Assign : AST
{
    public AST? Left { get; set; }
    public Token AssignOp { get; set; }
    public AST? Right { get; set; }

    public string Type { get; set; }

    public Assign(AST? Left, Token Op, AST? Right)
    {
        this.Left = Left;
        this.AssignOp = Op;
        this.Right = Right;

        this.Type = nameof(Assign);
    }
}

/// <summary>
/// Represents a variable
/// </summary>
public class Var : AST
{
    public Token Token { get; set; }
    public string Name { get; set; }

    public string Type { get; set; }

    public Var(Token token)
    {
        this.Token = token;
        this.Name = token.Content;

        this.Type = nameof(Var);
    }
}

/// <summary>
/// Represents an empty operation where nothing must be done
/// </summary>
public class NoOperation : AST
{
    public string Type { get; set; }

    public NoOperation()
    {
        this.Type = nameof(NoOperation);
    }
}

/// <summary>
/// Represents a variable declaration
/// </summary>
public class VarDeclaration : AST
{
    public string Type { get; set; }
    public Token DataType { get; set; }
    public Token Identifier { get; set; }


    public VarDeclaration(Token DataType, Token Identifier)
    {
        this.DataType = DataType;
        this.Identifier = Identifier;

        this.Type = nameof(VarDeclaration);
    }
}

/// <summary>
/// Represents a set of variable declarations in the form [DATA TYPE] var1, var2,...,var_n
/// </summary>
public class VarDeclarationSet : AST
{
    public string Type { get; set; }
    public AST?[]? Nodes { get; set; }

    public VarDeclarationSet(AST?[]? Nodes)
    {
        this.Nodes = Nodes;

        this.Type = nameof(VarDeclarationSet);
    }
}

/// <summary>
/// Represents an if-else statement
/// </summary>
public class Conditional : AST
{
    public string Type { get; set; }
    public AST? Condition { get; set; }
    public AST? Body { get; set; }
    public AST? Else { get; set; }

    public Conditional(AST? Condition, AST? Body, AST? Else = null)
    {
        this.Condition = Condition;
        this.Body = Body;
        this.Else = Else;

        this.Type = nameof(Conditional);
    }
}

/// <summary>
/// Represents a while statement
/// </summary>
public class WhileCicle : AST
{
    public string Type { get; set; }
    public AST? Condition { get; set; }
    public AST? Body { get; set; }

    public WhileCicle(AST? Condition, AST? Body)
    {
        this.Condition = Condition;
        this.Body = Body;

        this.Type = nameof(WhileCicle);
    }
}

/// <summary>
/// Represents a string
/// </summary>
public class String : AST
{
    public string Type { get; set; }
    public Token Value { get; set; }

    public String(Token value)
    {
        this.Value = value;

        this.Type = nameof(String);
    }
}

/// <summary>
/// Represents a C# property reference
/// </summary>
public class Method : AST
{
    public string Type { get; set; }

    public Token Name { get; set; }
    public AST? Props { get; set; }

    public Method(Token Name, AST? Props = null)
    {
        this.Name = Name;
        this.Props = Props;

        this.Type = nameof(Method);
    }
}

/// <summary>
/// Represents a C# method reference
/// </summary>
public class MethodCall : AST
{
    public string Type { get; set; }

    public Token Method { get; set; }
    public AST? Param { get; set; }

    public AST? Props { get; set; }

    public MethodCall(Token Method, AST? Param, AST? Props = null)
    {
        this.Method = Method;
        this.Param = Param;
        this.Props = Props;

        this.Type = nameof(MethodCall);
    }
}