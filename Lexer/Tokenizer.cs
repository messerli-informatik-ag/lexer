using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Funcky.Monads;
using Messerli.Lexer.Exceptions;
using Messerli.Lexer.Rules;
using static Funcky.Functional;

namespace Messerli.Lexer;

public class Tokenizer
{
    private readonly ImmutableList<ILexerRule> _lexerRules;
    private readonly Func<string, ILexerReader> _newLexerReader;
    private readonly LinePositionCalculator.Factory _newLinePositionCalculator;
    private readonly List<Lexeme> _lexemes = new();

    public Tokenizer(IEnumerable<ILexerRule> lexerRules, Func<string, ILexerReader> newLexerReader, LinePositionCalculator.Factory newLinePositionCalculator)
        => (_lexerRules, _newLexerReader, _newLinePositionCalculator) = (lexerRules.ToImmutableList(), newLexerReader, newLinePositionCalculator);

    public List<Lexeme> Scan(string expression)
    {
        var reader = _newLexerReader(expression);

        _lexemes.Clear();
        while (reader.Peek().Match(none: false, some: True))
        {
            var lexeme = SelectLexerRule(reader, _lexemes)
                .Match(
                    none: () => HandleUnknownToken(reader),
                    some: Identity);

            _lexemes.Add(lexeme);
        }

        return _lexemes;
    }

    private Lexeme HandleUnknownToken(ILexerReader reader)
    {
        throw new UnknownTokenException(reader.Peek(), CalculateCurrentLinePosition(reader.Position));
    }

    private LinePosition CalculateCurrentLinePosition(int position)
    {
        var positionCalculator = _newLinePositionCalculator(_lexemes);

        return positionCalculator.CalculateLinePosition(position);
    }

    private Option<Lexeme> SelectLexerRule(ILexerReader reader, List<Lexeme> context)
        => _lexerRules
            .Where(rule => rule.IsActive(context))
            .OrderByDescending(GetRuleWeight)
            .Select(rule => rule.Match(reader))
            .FirstOrDefault(HasRuleMatched);

    private static bool HasRuleMatched(Option<Lexeme> matched)
        => matched.Match(
            none: false,
            some: True);

    private static object GetRuleWeight(ILexerRule rule)
        => rule.Weight;
}