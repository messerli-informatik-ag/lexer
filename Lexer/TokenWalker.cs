using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Messerli.Lexer.Rules;
using Messerli.Lexer.Tokens;

namespace Messerli.Lexer;

public sealed class TokenWalker : ITokenWalker
{
    private const int EpsilonLength = 0;
    private readonly ITokenizer _tokenizer;
    private readonly Func<IToken> _newEpsilonToken;
    private readonly LinePositionCalculator.Factory _newLinePositionCalculator;
    private List<Lexeme> _lexemes = new();
    private ILinePositionCalculator? _linePositionCalculator;

    private int _currentIndex;

    public TokenWalker(ITokenizer tokenizer, Func<IToken> newEpsilonToken, LinePositionCalculator.Factory newLinePositionCalculator)
        => (_tokenizer, _newEpsilonToken, _newLinePositionCalculator) = (tokenizer, newEpsilonToken, newLinePositionCalculator);

    private Position EpsilonPosition
        => new(_lexemes.Last().Position.EndPosition, EpsilonLength);

    public static TokenWalker Create<TEpsilonToken>(IEnumerable<LexerRule> lexerRules)
        where TEpsilonToken : IToken, new()
        => new(CreateTokenizer(lexerRules), () => new TEpsilonToken(), LinePositionCalculator.Create);

    public void Scan(string expression)
        => Scan(expression, t => t);

    public void Scan(string expression, Func<IEnumerable<Lexeme>, IEnumerable<Lexeme>> postProcessTokens)
    {
        _currentIndex = 0;
        var unfilteredLexemes = _tokenizer.Scan(expression);
        _linePositionCalculator = _newLinePositionCalculator(unfilteredLexemes);
        _lexemes = postProcessTokens(unfilteredLexemes).ToList();
    }

    public Lexeme Pop()
        => ValidToken()
            ? _lexemes[_currentIndex++]
            : new Lexeme(_newEpsilonToken(), EpsilonPosition);

    public Lexeme Peek(int lookAhead = 0)
    {
        Debug.Assert(lookAhead >= 0, "a negative look ahead is not supported");

        return ValidToken(lookAhead)
            ? _lexemes[_currentIndex + lookAhead]
            : new Lexeme(_newEpsilonToken(), EpsilonPosition);
    }

    public LinePosition CalculateLinePosition(Lexeme lexeme)
        => _linePositionCalculator == null
            ? throw new Exception("Call Scan first before you try to calculate a position.")
            : _linePositionCalculator.CalculateLinePosition(lexeme);

    private static Tokenizer CreateTokenizer(IEnumerable<LexerRule> lexerRules)
        => new(lexerRules, LexerReader.Create, LinePositionCalculator.Create);

    private bool ValidToken(int lookAhead = 0)
        => _currentIndex + lookAhead < _lexemes.Count;
}