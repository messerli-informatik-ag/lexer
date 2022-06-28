using System.Collections.Generic;
using System.Linq;
using Messerli.Lexer.Rules;

namespace Lexer.Test.LexerRules
{
    class EmptyRules : ILexerRules
    {
        public IEnumerable<ILexerRule> GetRules()
        {
            return Enumerable.Empty<ILexerRule>();
        }
    }
}
