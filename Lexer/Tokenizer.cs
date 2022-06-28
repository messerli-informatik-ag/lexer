using System;
using System.Collections.Generic;
using System.Linq;
using Funcky.Monads;
using Messerli.Lexer.Exceptions;
using Messerli.Lexer.Rules;

namespace Messerli.Lexer;

public class Tokenizer
{
    private readonly ILexerRules _lexerRules;
    private readonly Func<string, ILexerReader> _newLexerReader;
    private readonly LinePositionCalculator.Factory _newLinePositionCalculator;
    private readonly List<Lexeme> _lexemes = new List<Lexeme>();

    public Tokenizer(ILexerRules lexerRules, Func<string, ILexerReader> newLexerReader, LinePositionCalculator.Factory newLinePositionCalculator)
        => (_lexerRules, _newLexerReader, _newLinePositionCalculator) = (lexerRules, newLexerReader, newLinePositionCalculator);

    public List<Lexeme> Scan(string expression)
    {
        var reader = _newLexerReader(expression);

        _lexemes.Clear();
        while (reader.Peek().Match(none: false, some: c => true))
        {
            var lexeme = SelectLexerRule(reader, _lexemes)
                .Match(
                    none: () => HandleUnknownToken(reader),
                    some: t => t);

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
            .GetRules()
            .Where(rule => rule.IsActive(context))
            .OrderByDescending(GetRuleWeight)
            .Select(rule => rule.Match(reader))
            .FirstOrDefault(HasRuleMatched);

    private bool HasRuleMatched(Option<Lexeme> matched)
        => matched.Match(
            none: false,
            some: t => true);

    private object GetRuleWeight(ILexerRule rule)
        => rule.Weight;
}