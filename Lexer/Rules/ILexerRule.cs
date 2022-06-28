using System.Collections.Generic;
using Funcky.Monads;

namespace Messerli.Lexer.Rules
{
    public interface ILexerRule
    {
        /// <summary>
        /// If more than one rule matches, the rule with the higher weights gets selected.
        /// </summary>
        int Weight { get; }

        /// <summary>
        /// Returns the matching lexem if the rule matches or None, if the rule does not match.
        /// </summary>
        Option<Lexem> Match(ILexerReader reader);

        /// <summary>
        /// For Lexer rules which are not context dependent this function returns always true.
        /// Otherwise the Lexer rule can determine its state with the context which is a list of all lexems which have been produced till now.
        /// </summary>
        bool IsActive(List<Lexem> context);
    }
}