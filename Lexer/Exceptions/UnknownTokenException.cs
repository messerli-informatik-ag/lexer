using Funcky.Monads;

namespace Messerli.Lexer.Exceptions;

public class UnknownTokenException : LexerException
{
    public UnknownTokenException(Option<char> token, LinePosition position)
        : base($"Unknown Token '{token.Match(none: 'Ɛ', some: t => t)}' at Line {position.Line} Column {position.Column}")
    {
        Token = token;
        Position = position;
    }

    public Option<char> Token { get; }

    public LinePosition Position { get; }
}