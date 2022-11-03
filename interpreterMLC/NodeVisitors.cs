namespace Interpreter;

using Parser;
using System.Reflection;
using Lexer;
using Symbols;

internal static class NodeVisitor {
    private delegate object Visitor(AST? Node);

    public static object Visit(AST? Node) {
        Assembly assembly = Assembly.GetExecutingAssembly();
        Type type = assembly.GetTypes().
                             Where(val => val.Name == nameof(NodeVisitors)).
                             ToArray()[0];
        
        if (Node != null) {
            MethodInfo? mf = type.GetMethod($"Visit{Node.Type}");
            if (mf != null){
                Visitor visitor = mf.CreateDelegate<Visitor>();
                return visitor(Node);
            }
        }

        return GenericVisitor(Node);
    }

    private static object GenericVisitor(AST? Node) {
        if (Node != null)
            throw new Exception($"no visit method for {Node.Type} node");
        
        throw new Exception("NULL nodes can't be visited");
    }
}

internal static class NodeVisitors {
    /*
     * Add here the methods to visit the nodes from the parser
     * 
     * All methods added here must me in the format Visit[Node]
     * where Node is the name of the node you wanna visit
     */

    public static object VisitBinaryOperator(AST? Node) {
        if (Node != null) {
            BinaryOperator bNode = (BinaryOperator) Node;
            #region Math Operators
                if (bNode.Operator.Type == SYMBOLS.PLUS) {
                    int Left  = (int) NodeVisitor.Visit(bNode.Left);
                    int Right = (int) NodeVisitor.Visit(bNode.Right);
                    return Left + Right;
                }
                if (bNode.Operator.Type == SYMBOLS.MINUS) {
                    int Left  = (int) NodeVisitor.Visit(bNode.Left);
                    int Right = (int) NodeVisitor.Visit(bNode.Right);
                    return Left - Right;
                }
                if (bNode.Operator.Type == SYMBOLS.MUL) {
                    int Left  = (int) NodeVisitor.Visit(bNode.Left);
                    int Right = (int) NodeVisitor.Visit(bNode.Right);
                    return Left * Right;
                }
                if (bNode.Operator.Type == SYMBOLS.DIV) {
                    int Left  = (int) NodeVisitor.Visit(bNode.Left);
                    int Right = (int) NodeVisitor.Visit(bNode.Right);
                    return Left / Right;
                }
                if (bNode.Operator.Type == SYMBOLS.MOD) {
                    int Left  = (int) NodeVisitor.Visit(bNode.Left);
                    int Right = (int) NodeVisitor.Visit(bNode.Right);
                    return Left % Right;
                }
            #endregion
            
            #region Boolean Operators
                if (bNode.Operator.Type == SYMBOLS.OR) {
                    bool Left  = (bool) NodeVisitor.Visit(bNode.Left);
                    bool Right = (bool) NodeVisitor.Visit(bNode.Right);
                    return Left || Right;
                }
                if (bNode.Operator.Type == SYMBOLS.AND) {
                    bool Left  = (bool) NodeVisitor.Visit(bNode.Left);
                    bool Right = (bool) NodeVisitor.Visit(bNode.Right);
                    return Left && Right;
                }
            #endregion

            #region Comparators
                if (bNode.Operator.Type == SYMBOLS.LESS) {
                    int Left  = (int) NodeVisitor.Visit(bNode.Left);
                    int Right = (int) NodeVisitor.Visit(bNode.Right);
                    return Left < Right;
                }
                if (bNode.Operator.Type == SYMBOLS.GREAT) {
                    int Left  = (int) NodeVisitor.Visit(bNode.Left);
                    int Right = (int) NodeVisitor.Visit(bNode.Right);
                    return Left > Right;
                }
                if (bNode.Operator.Type == SYMBOLS.EQUAL) {
                    int Left  = (int) NodeVisitor.Visit(bNode.Left);
                    int Right = (int) NodeVisitor.Visit(bNode.Right);
                    return Left == Right;
                }
                if (bNode.Operator.Type == SYMBOLS.L_EQUAL) {
                    int Left  = (int) NodeVisitor.Visit(bNode.Left);
                    int Right = (int) NodeVisitor.Visit(bNode.Right);
                    return Left <= Right;
                }
                if (bNode.Operator.Type == SYMBOLS.G_EQUAL) {
                    int Left  = (int) NodeVisitor.Visit(bNode.Left);
                    int Right = (int) NodeVisitor.Visit(bNode.Right);
                    return Left >= Right;
                }
            #endregion
        }
        return 0;
    }

    public static object VisitUnaryOperator(AST? Node) {
        if (Node != null) {
            UnaryOperator uNode = (UnaryOperator) Node;

            #region Math Operators
                if (uNode.Operator.Type == SYMBOLS.PLUS) {
                    int Value = (int) NodeVisitor.Visit(uNode.Value);
                    return +Value;
                }
                if (uNode.Operator.Type == SYMBOLS.MINUS) {
                    int Value = (int) NodeVisitor.Visit(uNode.Value);
                    return -Value;
                }
            #endregion

            #region Bool Operators
                if (uNode.Operator.Type == SYMBOLS.NOT) {
                    bool Value = (bool) NodeVisitor.Visit(uNode.Value);
                    return !Value;
                }
            #endregion
        }
        return 0;
    }

    public static object VisitInteger(AST? Node) {
        if (Node != null) {
            Integer iNode = (Integer) Node;
            return iNode.Value.ToInt32();
        }
        
        return 0;
    }

    public static object VisitBoolean(AST? Node) {
        if (Node != null) {
            Boolean bNode = (Boolean) Node;
            return bNode.Value.AsBoolean();
        }

        return false;
    }

    public static object VisitCompound(AST? Node) {
        if (Node != null) {
            Compound cNode = (Compound) Node;

            if (cNode.Children != null)
            foreach (AST? node in cNode.Children) {
                NodeVisitor.Visit(node);
            }

            return new Object();
        }
        throw new Exception("error while parsing input");
    }

    public static object VisitNoOperation(AST? Node) {
        return new Object();
    } 

    public static object VisitAssign(AST? Node) {
        if (Node != null) {
            Assign aNode = (Assign) Node;

            if (aNode.Left != null) {
                Var v = (Var) aNode.Left;

                string varName = v.Name;
                ISymbol? varSymbol = SymbolTable.Lookup(varName);

                if (varSymbol == null) {
                    throw new Exception($"cannot assign value to not declared var \'{varName}\'");
                }

                object Value = NodeVisitor.Visit(aNode.Right);

                Scope.Global[v.Name] = Value;    
                
                return Value;
            }
        }

        throw new Exception("assignment cannot be done");
    }

    public static object VisitVar(AST? Node) {
        if (Node != null) {
            Var vNode = (Var) Node;

            ISymbol? varSymbol = SymbolTable.Lookup(vNode.Name);

            if (varSymbol == null) {
                throw new Exception($"var {vNode.Name} is not declared yet");
            }

            try {
                return Scope.Global[vNode.Name];
            }
            catch (KeyNotFoundException){
                throw new Exception($"var {vNode.Name} doesn't have a value yet");
            }
        }
        throw new Exception("this variable does not exists");
    }

    public static object VisitVarDeclarationSet(AST? Node) {
        if (Node != null) {
            VarDeclarationSet vNode = (VarDeclarationSet) Node;
            
            if (vNode.Nodes != null) {
                foreach (AST? subNode in vNode.Nodes) {
                    NodeVisitor.Visit(subNode);
                }
                return 0;
            }
            
        }

        throw new Exception("couldn't create variable");
    }

    public static object VisitVarDeclaration(AST? Node) {
        if (Node != null) {
            VarDeclaration vNode = (VarDeclaration) Node;
            
            string typeName = vNode.DataType.Content;
            ISymbol? typeSymbol = SymbolTable.Lookup(typeName);

            string varName = vNode.Identifier.Content;
            ISymbol? varSymbol = SymbolTable.Lookup(varName);

            if (varSymbol != null) {
                throw new Exception($"cannot redeclare var {varName}");
            }

            SymbolTable.Define(new VarSymbol(varName, typeSymbol));

            return new Object();
        }

        throw new Exception("something happenned");
    }

    public static object VisitConditional(AST? Node) {
        if (Node != null) {
            Conditional cNode = (Conditional) Node;

            bool validCondition = (bool) NodeVisitor.Visit(cNode.Condition);

            if (validCondition) {
                return NodeVisitor.Visit(cNode.Body);
            }
            else if (cNode.Else != null){
                return NodeVisitor.Visit(cNode.Else);
            }

            return new Object();
        }
        throw new Exception("error reading conditional");
    }

    public static object VisitWhileCicle(AST? Node) {
        if (Node != null) {
            WhileCicle cNode = (WhileCicle) Node;

            object result = new Object();

            while ((bool) NodeVisitor.Visit(cNode.Condition)) {
                result = NodeVisitor.Visit(cNode.Body);
            }

            return result;
        }
        throw new Exception("error reading while-cicle");
    }
}
