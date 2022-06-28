using System;
using System.Collections.Generic;
using Funcky.Monads;
using static Funcky.Functional;

namespace Messerli.Lexer.Rules;

public class LexerRule : ILexerRule
{
    public LexerRule(Predicate<char> predicate, Func<ILexerReader, Lexeme> createToken, int weight = 0)
    {
        Predicate = predicate;
        CreateToken = createToken;
        Weight = weight;
    }

    public Predicate<char> Predicate { get; }

    public Func<ILexerReader, Lexeme> CreateToken { get; }

    public int Weight { get; }

    public Option<Lexeme> Match(ILexerReader reader)
        => ApplyPredicate(reader).Match(none: false, some: Identity)
            ? CreateToken(reader)
            : Option<Lexeme>.None();

    public bool IsActive(List<Lexeme> context)
        => true;

    private Option<bool> ApplyPredicate(ILexerReader reader)
        => from nextCharacter in reader.Peek()
            select Predicate(nextCharacter);
}