using Messerli.Lexer.Tokens;

namespace Messerli.Lexer.Test.Token;

internal sealed record AaToken : IToken;
internal sealed record AndToken : IToken;
internal sealed record BbToken : IToken;
internal sealed record CcAfterBbToken : IToken;
internal sealed record CcToken : IToken;
internal record EqualToken : IToken;
internal record DoubleEqualToken : IToken;
internal record GreaterToken : IToken;
internal record GreaterEqualToken : IToken;
public record NewLineToken : IToken, ILineBreakToken;
internal sealed record SpaceToken : IToken;

public record IdentifierToken(string Name) : IToken
{
    public override string ToString() => $"Identifier: {Name}";
}

internal sealed record WordToken(string Word) : IToken
{
    public override string ToString()
    {
        return Word;
    }
}