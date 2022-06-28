using Messerli.Lexer.Exceptions;
using Messerli.Lexer.Tokens;

namespace Messerli.Lexer
{
    public static class TokenWalkerExtensions
    {
        public static Lexem Consume<TToken>(this TokenWalker walker)
            where TToken : IToken
            => ConsumeLexem<TToken>(walker, walker.Pop());

        public static bool NextIs<TType>(this TokenWalker walker, int lookAhead = 0)
            => walker.Peek(lookAhead).Token is TType;

        private static Lexem ConsumeLexem<TToken>(TokenWalker walker, Lexem lexem)
            where TToken : IToken
            => lexem.Token is TToken
                ? lexem
                : HandleMissingLexem<TToken>(lexem, walker);

        private static Lexem HandleMissingLexem<TToken>(Lexem lexem, TokenWalker walker)
            => ThrowExpectingTokenException<TToken>(lexem, walker.CalculateLinePosition(lexem));

        private static Lexem ThrowExpectingTokenException<TToken>(Lexem lexem, LinePosition position)
            => throw new InvalidTokenException($"Expecting {typeof(TToken).FullName} but got {lexem.Token} at Line {position.Line} Column {position.Column}.");
    }
}
