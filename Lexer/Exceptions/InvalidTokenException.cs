﻿using System;

namespace Messerli.Lexer.Exceptions;

public class InvalidTokenException : Exception
{
    public InvalidTokenException(string message)
        : base(message)
    {
    }
}