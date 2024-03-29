﻿using System.Collections.Generic;
using System.Linq;
using Funcky.Extensions;
using Funcky.Monads;
using Messerli.Lexer.Tokens;

namespace Messerli.Lexer.Rules;

public sealed class SimpleLexerRule<TToken> : ILexerRule
    where TToken : IToken, new()
{
    private readonly string _textSymbol;
    private readonly bool _isTextSymbol;

    public SimpleLexerRule(string textSymbol)
    {
        _textSymbol = textSymbol;
        _isTextSymbol = textSymbol.All(char.IsLetter);
    }

    public int Weight
        => _textSymbol.Length;

    public Option<Lexeme> Match(ILexerReader reader)
        => MatchLexeme(reader, reader.Position);

    public bool IsActive(List<Lexeme> context)
        => true;

    private Option<Lexeme> MatchLexeme(ILexerReader reader, int startPosition)
        => IsSymbolMatchingReader(reader) && (IsOperator() || HasWordBoundary(reader))
            ? ConsumeLexeme(reader, startPosition)
            : Option<Lexeme>.None;

    private Option<Lexeme> ConsumeLexeme(ILexerReader reader, int startPosition)
    {
        _textSymbol.ForEach(_ => reader.Read());

        return CreateLexeme(startPosition);
    }

    // we do not want to extract key words in the middle of a word, so a symbol must have ended.
    // Which means after a textsymbol must come something other than a digit or a letter.
    private bool HasWordBoundary(ILexerReader reader)
        => reader.Peek(_textSymbol.Length).Match(none: true, some: NonLetterOrDigit);

    private bool IsOperator()
        => !_isTextSymbol;

    private static bool NonLetterOrDigit(char character)
        => !char.IsLetterOrDigit(character);

    private bool IsSymbolMatchingReader(ILexerReader reader)
        => _textSymbol.Select((character, index) => new { character, index })
            .All(t => reader.Peek(t.index).Match(none: false, some: c => c == t.character));

    private Lexeme CreateLexeme(int start)
        => CreateLexemeFromToken(start, new TToken());

    private Lexeme CreateLexemeFromToken(int start, TToken token)
        => new(token, new Position(start, _textSymbol.Length), token is ILineBreakToken);
}