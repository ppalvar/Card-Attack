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

## How to use *MLC* to create cards?

When creating an *Effect Card* you can use all the above info.

Plus, like in the previous example, when creating a Card will have a context like following:

```cs
class MatchState
{
    private MonsterCard me { get; set; }
    private MonsterCard target { get; set; }
    private Match match { get; set; }

    public MatchState(MonsterCard me, MonsterCard target, Match match)
    {
        this.me = me;
        this.target = target;
        this.match = match;
    }
}
```

Here, the `me` property refers to the card that is using the power; the `target` property refers to the card that will receive damage from the power activation; finally, `match` is the whole state of the match.

In order to accede to the properties of `me`:

```cpp
//properties
@me.HP; // this reads the life of your card
@me.AttackPoints; // reads the attack of this card

//methods
@me.IncreaseHP(x); // Increases the HP of your card by x points (posibly negative for decrease)
@me.IncreaseAttack(x); // Increases the attack points of your card by x points (posibly negative for decrease)
@me.Kill(False); // Instantly kills your card
```

To the `target` property is the same but changing `@me.foo.bar` for `@target.foo.bar`

To accede the `match`property methods and sub-properties is a bit more complicated but not that much:

```cs
@match.player // Reads the player
@match.enemy  // Reads the enemy

@match.player.TableAt(i); //takes the card in the player's hand at the position i

// Note that all the above lines don't make much 
// sense in the game. They are only used to read
// the following properties and methods

//this lines aply to the current player
@match.player.TableAt(i).IncreaseHP(x); // Increases the HP of the card in position i by x points (posibly negative for decrease)
@match.player.TableAt(i).IncreaseAttack(x); // Increases the attack points of the card in position i by x points (posibly negative for decrease)
@match.player.TableAt(i).Kill(False); // Instantly kills the monster in the position i
@match.player.TableAt(i).HP; // reads the hp of the card in the position i
@match.player.TableAt(i).AttackPoints; // reads the attack of the card in the position i

//this lines aply to the other player (their enemy)
@match.enemy.TableAt(i).IncreaseHP(x);
@match.enemy.TableAt(i).IncreaseAttack(x);
@match.enemy.TableAt(i).Kill(false);
@match.enemy.TableAt(i).HP;
@match.enemy.TableAt(i).AttackPoints;
```

Like a side note, the `TableAt()` method will return an empty card (with 0 HP and 0 attack), so this must be remembered whwn creating some power. Plus, the `Kill()` method receives a boolean argument and this is the life state for the card (`false` for dead, `true` for alive). Finally, the table is of a fixed size (5) and the `i` parameter in `TableAt(i)` must be an integer between 0 and 4.

## Snippets

This code kills all the monsters in the enemy table and for each kill increases the hp and the attack of the card that used this power by 10 points. If there is an empty space in the enemy's table, also will increase the hp and attack of the card.

```cs
int i;i = 0;
while (i < 5) {
    if (@match.enemy.TableAt(i).HP <= 50) {
        @match.enemy.TableAt(i).Kill(false);
        @me.IncreaseHP(10);
        @me.IncreaseAttack(10);
    }

    i = i + 1;
}
```

This code compares the lifes of the attacker and the target cards, if they are equal, kills both, otherwise, swaps their hp values.

```cs
int a;a = @me.HP;
int b;b = @target.HP;

if (a == b) {
    @me.Kill(false);
    @target.Kill(false);
}
else {
    @me.IncreaseHP(-a);
    @me.IncreaseHP(b);

    @target.IncreaseHP(-b);
    @target.IncreaseHP(a);
}
```

This snippet checks if the enemy has a monster with hp higher than 100, and if that is true, then adds to your monster the hp of your target an finally kills the target.

```cs
int i; i = 0;
bool flag; flag = false;

while (!flag & i < 5) {
    flag = flag | @match.enemy.TableAt(i).HP > 100;
    i = i + 1;
}

if (flag) {
    @me.IncreaseHP(@target.HP);
    @target.Kill(false);
}
```
