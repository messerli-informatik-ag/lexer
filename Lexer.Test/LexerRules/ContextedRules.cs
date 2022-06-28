﻿using System;
using System.Collections.Generic;
using System.Linq;
using Lexer.Test.Tokens;
using Messerli.Lexer;
using Messerli.Lexer.Rules;

namespace Lexer.Test.LexerRules
{
    class ContextedRules : ILexerRules
    {
        public IEnumerable<ILexerRule> GetRules()
        {
            yield return new SimpleLexerRule<SpaceToken>(" ");
            yield return new SimpleLexerRule<AaToken>("aa");
            yield return new SimpleLexerRule<BbToken>("bb");
            yield return new SimpleLexerRule<CcToken>("cc");
            yield return new ContextedLexerRule(
                c => c == 'c',
                context => context.Any(p => p.Token is BbToken),
                ScanCcAfterBB,
                3);
        }

        private Lexem ScanCcAfterBB(ILexerReader reader)
        {
            var startPosition = reader.Position;

            if (reader.Peek().Match(false, c => c == 'c'))
            {
                reader.Read();
                if (reader.Peek().Match(false, c => c == 'c'))
                {
                    reader.Read();
                    return new Lexem(new CcAfterBbToken(), new Position(startPosition, reader.Position - startPosition));
                }
            }

            throw new NotImplementedException();
        }
    }
}