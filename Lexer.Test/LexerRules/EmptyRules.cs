using System.Collections.Generic;
using System.Linq;
using Messerli.Lexer.Rules;

namespace Messerli.Lexer.Test.LexerRules;

public class EmptyRules
{
    public static IEnumerable<ILexerRule> GetRules()
        => Enumerable.Empty<ILexerRule>();
}