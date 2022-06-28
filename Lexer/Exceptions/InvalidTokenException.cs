namespace Messerli.Lexer.Exceptions;

public class InvalidTokenException : LexerException
{
    public InvalidTokenException(string message)
        : base(message)
    {
    }
}