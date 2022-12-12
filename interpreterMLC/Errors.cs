namespace Exceptions;

/// <summary>
/// This exception is thrown when a variable or object name does not appear
/// or the user is trying to redeclare it
/// </summary>
public class NameException : Exception
{
    public NameException(string message) : base(message) { }
}

/// <summary>
/// This exception is thrown when an expected value is null
/// </summary>
public class NullValueException : Exception
{
    public NullValueException(string message) : base(message) { }
}

/// <summary>
/// This exception is thrown when a node of the AST is null
/// </summary>
public class NullNodeException : Exception
{
    public NullNodeException(string message) : base(message) { }
}

/// <summary>
/// This exception is thrown when a C# property or method does not exists
/// </summary>
public class PropertyException : Exception
{
    public PropertyException(string message) : base(message) { }
}