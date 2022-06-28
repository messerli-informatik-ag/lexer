﻿using System;
using System.Collections.Generic;
using System.Linq;
using Funcky.Monads;
using Messerli.Lexer.Exceptions;
using Messerli.Lexer.Rules;

namespace Messerli.Lexer
{
    public class Tokenizer
    {
        private readonly ILexerRules _lexerRules;
        private readonly Func<string, ILexerReader> _newLexerReader;
        private readonly LinePositionCalculator.Factory _newLinePositionCalculator;
        private readonly List<Lexem> _lexems = new List<Lexem>();

        public Tokenizer(ILexerRules lexerRules, Func<string, ILexerReader> newLexerReader, LinePositionCalculator.Factory newLinePositionCalculator) 
            => (_lexerRules, _newLexerReader, _newLinePositionCalculator) = (lexerRules, newLexerReader, newLinePositionCalculator);

        public List<Lexem> Scan(string expression)
        {
            var reader = _newLexerReader(expression);

            _lexems.Clear();
            while (reader.Peek().Match(false, c => true))
            {
                var lexem = SelectLexerRule(reader, _lexems)
                    .Match(
                        none: () => HandleUnknownToken(reader),
                        some: t => t);

                _lexems.Add(lexem);
            }

            return _lexems;
        }

        private Lexem HandleUnknownToken(ILexerReader reader)
        {
            throw new UnknownTokenException(reader.Peek(), CalculateCurrentLinePosition(reader.Position));
        }

        private LinePosition CalculateCurrentLinePosition(int position)
        {
            var positionCalculator = _newLinePositionCalculator(_lexems);

            return positionCalculator.CalculateLinePosition(position);
        }

        private Option<Lexem> SelectLexerRule(ILexerReader reader, List<Lexem> context)
            => _lexerRules
                .GetRules()
                .Where(rule => rule.IsActive(context))
                .OrderByDescending(GetRuleWeight)
                .Select(rule => rule.Match(reader))
                .FirstOrDefault(HasRuleMatched);

        private bool HasRuleMatched(Option<Lexem> matched)
            => matched.Match(
                none: false,
                some: t => true);

        private object GetRuleWeight(ILexerRule rule)
            => rule.Weight;
    }
}
