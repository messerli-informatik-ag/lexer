using System;
using System.Collections.Generic;
using Funcky.Monads;
using static Funcky.Functional;

namespace Messerli.Lexer.Rules;

public sealed class LexerRuleWithContext : ILexerRule
{
    public LexerRuleWithContext(Predicate<char> symbolPredicate, Predicate<List<Lexeme>> contextPredicate, Func<ILexerReader, Lexeme> createToken, int weight)
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
        => ApplyPredicate(reader).Match(none: false, some: Identity)
            ? CreateToken(reader)
            : Option<Lexeme>.None;

    public bool IsActive(List<Lexeme> context)
        => ContextPredicate(context);

    private Option<bool> ApplyPredicate(ILexerReader reader)
        => from nextCharacter in reader.Peek()
            select SymbolPredicate(nextCharacter);
}