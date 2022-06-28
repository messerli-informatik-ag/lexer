using System;

namespace Messerli.Lexer.Exceptions;

public class LexerException : Exception
{
    public LexerException(string message)
        : base(message)
    {
    }
}