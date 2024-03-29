﻿using Funcky.Monads;

namespace Messerli.Lexer;

public sealed class LexerReader : ILexerReader
{
    private readonly string _expression;

    public LexerReader(string expression)
        => _expression = expression;

    public int Position { get; private set; }

    public static ILexerReader Create(string expression)
        => new LexerReader(expression);

    public Option<char> Peek(int lookAhead = 0)
        => PeekAt(Position + lookAhead);

    public Option<char> Read()
    {
        var result = Peek();

        Position++;

        return result;
    }

    private Option<char> PeekAt(int position)
        => position >= 0 && position < _expression.Length
            ? _expression[position]
            : Option<char>.None;
}