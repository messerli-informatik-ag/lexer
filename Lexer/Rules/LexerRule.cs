using System;
using System.Collections.Generic;
using Funcky.Monads;

namespace Messerli.Lexer.Rules
{
    public class LexerRule : ILexerRule
    {
        public LexerRule(Predicate<char> predicate, Func<ILexerReader, Lexem> createToken, int weight = 0)
        {
            Predicate = predicate;
            CreateToken = createToken;
            Weight = weight;
        }

        public Predicate<char> Predicate { get; }

        public Func<ILexerReader, Lexem> CreateToken { get; }

        public int Weight { get; }

        public Option<Lexem> Match(ILexerReader reader)
            => ApplyPredicate(reader).Match(none: false, some: p => p)
                ? CreateToken(reader)
                : Option<Lexem>.None();

        public bool IsActive(List<Lexem> context)
            => true;

        private Option<bool> ApplyPredicate(ILexerReader reader)
            => from nextCharacter in reader.Peek()
               select Predicate(nextCharacter);
    }
}