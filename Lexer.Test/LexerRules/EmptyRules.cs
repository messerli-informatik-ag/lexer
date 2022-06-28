using System.Collections.Generic;
using System.Linq;
using Messerli.Lexer.Rules;

namespace Messerli.Lexer.Test.LexerRules;

public class EmptyRules : ILexerRules
{
    public IEnumerable<ILexerRule> GetRules()
    {
        return Enumerable.Empty<ILexerRule>();
    }
}