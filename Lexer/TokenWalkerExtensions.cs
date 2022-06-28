using Messerli.Lexer.Exceptions;
using Messerli.Lexer.Tokens;

namespace Messerli.Lexer;

public static class TokenWalkerExtensions
{
    public static Lexeme Consume<TToken>(this ITokenWalker walker)
        where TToken : IToken
        => ConsumeLexeme<TToken>(walker, walker.Pop());

    public static bool NextIs<TType>(this ITokenWalker walker, int lookAhead = 0)
        => walker.Peek(lookAhead).Token is TType;

    private static Lexeme ConsumeLexeme<TToken>(ITokenWalker walker, Lexeme lexeme)
        where TToken : IToken
        => lexeme.Token is TToken
            ? lexeme
            : HandleMissingLexeme<TToken>(lexeme, walker);

    private static Lexeme HandleMissingLexeme<TToken>(Lexeme lexeme, ITokenWalker walker)
        => ThrowExpectingTokenException<TToken>(lexeme, walker.CalculateLinePosition(lexeme));

    private static Lexeme ThrowExpectingTokenException<TToken>(Lexeme lexeme, LinePosition position)
        => throw new InvalidTokenException($"Expecting {typeof(TToken).FullName} but got {lexeme.Token} at Line {position.Line} Column {position.Column}.");
}