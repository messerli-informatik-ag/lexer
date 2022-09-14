#pragma warning disable SA1649 // File name should match first type name
using Messerli.Lexer.Tokens;

namespace Messerli.Lexer.Test.Token;

internal sealed record AaToken : IToken;
internal sealed record AndToken : IToken;
internal sealed record BbToken : IToken;
internal sealed record CcAfterBbToken : IToken;
internal sealed record CcToken : IToken;
internal sealed record EqualToken : IToken;
internal sealed record DoubleEqualToken : IToken;
internal sealed record GreaterToken : IToken;
internal sealed record GreaterEqualToken : IToken;
internal sealed record NewLineToken : IToken, ILineBreakToken;
internal sealed record SpaceToken : IToken;
internal sealed record EpsilonToken : IToken;

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