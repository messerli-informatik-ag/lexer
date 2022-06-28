using Messerli.Lexer.Exceptions;
using Messerli.Lexer.Tokens;

namespace Messerli.Lexer
{
    public static class TokenWalkerExtensions
    {
        public static Lexeme Consume<TToken>(this TokenWalker walker)
            where TToken : IToken
            => ConsumeLexem<TToken>(walker, walker.Pop());

        public static bool NextIs<TType>(this TokenWalker walker, int lookAhead = 0)
            => walker.Peek(lookAhead).Token is TType;

        private static Lexeme ConsumeLexem<TToken>(TokenWalker walker, Lexeme lexeme)
            where TToken : IToken
            => lexeme.Token is TToken
                ? lexeme
                : HandleMissingLexem<TToken>(lexeme, walker);

        private static Lexeme HandleMissingLexem<TToken>(Lexeme lexeme, TokenWalker walker)
            => ThrowExpectingTokenException<TToken>(lexeme, walker.CalculateLinePosition(lexeme));

        private static Lexeme ThrowExpectingTokenException<TToken>(Lexeme lexeme, LinePosition position)
            => throw new InvalidTokenException($"Expecting {typeof(TToken).FullName} but got {lexeme.Token} at Line {position.Line} Column {position.Column}.");
    }
}
