using System;
using System.Collections.Generic;
using Funcky.Monads;

namespace Messerli.Lexer.Rules
{
    public record ContextedLexerRule : ILexerRule
    {
        public ContextedLexerRule(Predicate<char> symbolPredicate, Predicate<List<Lexeme>> contextPredicate, Func<ILexerReader, Lexeme> createToken, int weight)
        {
            SymbolPredicate = symbolPredicate;
            ContextPredicate = contextPredicate;
            CreateToken = createToken;
            Weight = weight;
        }

        public Predicate<char> SymbolPredicate { get; }

        public Predicate<List<Lexeme>> ContextPredicate { get; }

        public Func<ILexerReader, Lexeme> CreateToken { get; }

        public int Weight { get; }

        public Option<Lexeme> Match(ILexerReader reader)
            => ApplyPredicate(reader).Match(none: false, some: p => p)
                ? CreateToken(reader)
                : Option<Lexeme>.None();

        private Option<bool> ApplyPredicate(ILexerReader reader)
            => from nextCharacter in reader.Peek()
               select SymbolPredicate(nextCharacter);

        public bool IsActive(List<Lexeme> context)
            => ContextPredicate(context);
    }
}
