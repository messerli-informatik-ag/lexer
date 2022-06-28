# Messerli.Lexer

A simple lexer which gives you a stream of Tokens from a string and some simple to define rules.

## Getting started

You have to define at least an EpsilonToken (you can name it differently, or use a class) which signals the end of the input.

```cs
public sealed record EpsilonToken : IToken;
```

Then you define the other token you want to use:

```cs
internal sealed record PlusToken(string Number): IToken;
internal sealed record MinusToken(string Number): IToken;
internal sealed record NumberToken(string Number): IToken;
```

Now you need some rules, the `TokenWalker` expects an `IEnumerable<LexerRule>`.

```cs
internal static class ExampleRules
{
    public static IEnumerable<ILexerRule> GetRules()
    {
        yield return new SimpleLexerRule<PlusToken>("+");
        yield return new SimpleLexerRule<MinusToken>("-");
        yield return new LexerRule(char.Digit, ScanNumber);
    }

    private static Lexeme ScanIdentifier(ILexerReader reader)
    {
        var startPosition = reader.Position;
        var stringBuilder = new StringBuilder();
        
        while (reader.Peek().Match(none: false, some: char.IsDigit))
        {
            stringBuilder.Append(reader.Read().Match(none: ' ', some: Identity));
        }

        return new Lexeme(new IdentifierToken(stringBuilder.ToString()), new Position(startPosition, reader.Position - startPosition));
    }
}
```

We see some simple rules which just need the string, you can also define overlapping simple rules like "=" and "==" the longer strings take precedent.

We see also how you scan a more complex Lexeme, where you see how you should create a Lexeme out of your Tokens and you see how to handle the position information for the Lexem.

```cs
TokenWalker.Create<EpsilonToken>(SimpleRules.GetRules())
```

### Line-handling

If you have an expression which goes over multiple lines, you usually want the position as Line number + character on that line. The default setting already handles everything as long as you declare the token which denotes a new-line with the interface ILineBreakToken.

```cs
internal sealed record NewLine(): IToken, ILineBreakToken;
```


## Special Tasks

### Overriding Line calculation

You have to instantiate the TokeWalker yourself with your own instance of ILineCalculator.

### I need to do something different depending on a Lexem already parsed.

`LexerRuleWithContext` gives you access to all `Lexem`s already produced. 

Before you use it, think hard if you could solve it differently, maybe in the parsing phase instead of in the lexer.
