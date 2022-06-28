using System.Collections.Generic;
using System.Text;
using Messerli.Lexer.Rules;
using Messerli.Lexer.Test.Tokens;

namespace Messerli.Lexer.Test.LexerRules;

public class WordTokenizerWithLines
{
    public static IEnumerable<ILexerRule> GetRules()
    {
        yield return new LexerRule(char.IsLetter, ScanWord);
        yield return new SimpleLexerRule<SpaceToken>(" ");
        yield return new SimpleLexerRule<NewLineToken>("\r\n");
        yield return new SimpleLexerRule<NewLineToken>("\n");
        yield return new SimpleLexerRule<NewLineToken>("\r");
    }

    private static Lexeme ScanWord(ILexerReader reader)
    {
        var startPosition = reader.Position;
        var word = new StringBuilder();

        while (reader.Peek().Match(none: false, some: char.IsLetter))
        {
            _ = reader.Read().AndThen(word.Append);
        }

        return new Lexeme(new WordToken(word.ToString()), new Position(startPosition, reader.Position - startPosition));
    }
}