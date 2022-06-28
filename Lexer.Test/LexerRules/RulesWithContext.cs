using System;
using System.Collections.Generic;
using System.Linq;
using Messerli.Lexer.Rules;
using Messerli.Lexer.Test.Tokens;

namespace Messerli.Lexer.Test.LexerRules;

public class RulesWithContext
{
    public static IEnumerable<ILexerRule> GetRules()
    {
        yield return new SimpleLexerRule<SpaceToken>(" ");
        yield return new SimpleLexerRule<AaToken>("aa");
        yield return new SimpleLexerRule<BbToken>("bb");
        yield return new SimpleLexerRule<CcToken>("cc");
        yield return new ContextedLexerRule(
            c => c == 'c',
            context => context.Any(p => p.Token is BbToken),
            ScanCcAfterBb,
            3);
    }

    private static Lexeme ScanCcAfterBb(ILexerReader reader)
    {
        var startPosition = reader.Position;

        if (reader.Peek().Match(none: false, some: IsC))
        {
            reader.Read();
            if (reader.Peek().Match(none: false, some: IsC))
            {
                reader.Read();
                return new Lexeme(new CcAfterBbToken(), new Position(startPosition, reader.Position - startPosition));
            }
        }

        throw new NotImplementedException();
    }

    private static bool IsC(char c)
        => c == 'c';
}