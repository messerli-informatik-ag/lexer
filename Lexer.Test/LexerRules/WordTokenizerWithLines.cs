using System.Collections.Generic;
using System.Text;
using Messerli.Lexer.Rules;
using Messerli.Lexer.Test.Tokens;

namespace Messerli.Lexer.Test.LexerRules
{
    public class WordTokenizerWithLines : ILexerRules
    {
        public IEnumerable<ILexerRule> GetRules()
        {
            yield return new LexerRule(char.IsLetter, ScanWord);
            yield return new SimpleLexerRule<SpaceToken>(" ");
            yield return new SimpleLexerRule<NewLineToken>("\r\n");
            yield return new SimpleLexerRule<NewLineToken>("\n");
            yield return new SimpleLexerRule<NewLineToken>("\r");
        }

        private Lexeme ScanWord(ILexerReader reader)
        {
            var startPosition = reader.Position;
            var word = new StringBuilder();

            while (reader.Peek().Match(none: false, some: char.IsLetter))
            {
                reader.Read().AndThen(c => word.Append(c));
            }

            return new Lexeme(new WordToken(word.ToString()), new Position(startPosition, reader.Position - startPosition));
        }
    }
}
