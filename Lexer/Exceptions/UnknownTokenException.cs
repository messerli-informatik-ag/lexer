using Funcky.Monads;
using static Funcky.Functional;

namespace Messerli.Lexer.Exceptions;

public sealed class UnknownTokenException : LexerException
{
    public UnknownTokenException(Option<char> token, LinePosition position)
        : base($"Unknown Token '{ToName(token)}' at Line {position.Line} Column {position.Column}")
    {
        Token = token;
        Position = position;
    }

    public Option<char> Token { get; }

    public LinePosition Position { get; }

    private static char ToName(Option<char> token)
        => token.Match(none: 'Ɛ', some: Identity);
}