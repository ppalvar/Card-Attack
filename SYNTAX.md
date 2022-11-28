# Syntax cheatsheet for *MLC*

**MLC**(**M**ini **L**anguage for **C**ards) is a custom programming language aiming to help the user to create cards for the present game. It's syntax is a subset of *C#* programming language, with some simplifications.

## Data types

The language has three data types which are:

```cs
bool //Containing True or False values
int  //Containing 32-bit signed integer values
string  //Containing unicode values
```

To declare a variable, you must specify its data type and in a separate line specify its value. For example:

```cs
int a, b, c; // declared variables
bool d;

a = 10;    // assign of const value
b = 20;
c = a + b; // assignment from other variables
d = (a <= b);
```

## Math Operators

The aritmethic operators are the usual of other languages (`+`, `-`, `*`, `/`, `%`) and their performance is the same they have in `C#` for example.

The only remark here is that they only operate over integers, and including floating point numbers will cause an error.

Besides, the `+` operator will also concatenate strings.

Remark that the `++`, `--`, `+=`, `-=`, `*=`, `/=` and `%=` operators do **not** exist here. Also, the bit-level operands do **not** exist here.

## Bool Operators

To compare integers and strings we have the usual comparison operators (`<`, `>`, `<=`, `>=`, `==`, `!=`). Do **not** compare different data types like:

```cs
int a; a = 10;
str b; b = "Hello world";

bool c; c = (a <= b); //Error because can't compare this variables
```

### Logic operators are as follow

- `&`: logic AND
- `|`: logic OR
- `!`: logic NOT

This operators apply on `bool` objects and expresions that raturn a boolean.

## Flow control structures

There are two flow control structures in this language: `if-else` statements and `while` statements. Their structure and behavior is equal to `C#`'s.

## Scopes and general syntax

For simplicity, the code isn't contained in any function or method-like structure, so you can just use it as a scripting language, like JavaScript. Also the variables are all asociated to the same global scope, this is, the variables you declare inside an `if` statement for example, will still exist when the interpreter leaves the statement.

## C# object reference API

When calling the `Interpreter.Interpret(object Context)` method, all the methods and properties of `Context` will be available in the *MLC* code. For example:

```cs
//...C# class...
class Person {
    public int age{get;set;}
    public string name{get;set;}

    public Person(int age, string Name) {
        this.age = age;
        this.name = name;
    }

    public void HappyBirthay(object newAge) {
        this.age = (int) newAge;
    }
}
```

```cs
//...inside Main() method...
Interpreter i = new Interpreter(); 
Person p = new Person(20, "Peter Parker");

i.Interpret(code, p);//code is a string with MLC code
```

Option #1:

```cpp
int age;
age = @age;

@HappyBirthday(age + 1);
```

Option #2:

```cpp
@HappyBirthday(@age + 1);
```

After calling the `Interpreter.Interpret` method, the `age` property of the `Person` object will be $21$.

Note that in order to get a property or method from `Person` class in the *MLC* code, the defined syntax is `@prop1.prop2.prop3` for properties and `@prop1.prop2.prop_3(param)`. You can concatenate all the `prop`'s you want as long the result makes sense. a variable can contain the value of a property obtained on this way as long as their data type matches with it's C# equivalent.
