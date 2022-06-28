using System.Collections.Generic;
using System.Text;
using Messerli.Lexer.Rules;
using Messerli.Lexer.Test.Token;
using static Funcky.Functional;

namespace Messerli.Lexer.Test.LexerRules;

internal static class ExampleRules
{
    public static IEnumerable<ILexerRule> GetRules()
    {
        yield return new SimpleLexerRule<EqualToken>("=");
        yield return new SimpleLexerRule<DoubleEqualToken>("==");
        yield return new SimpleLexerRule<GreaterToken>("<");
        yield return new SimpleLexerRule<GreaterEqualToken>("<=");
        yield return new SimpleLexerRule<AndToken>("and");
        yield return new SimpleLexerRule<SpaceToken>(" ");
        yield return new LexerRule(char.IsLetter, ScanIdentifier);
    }

    private static Lexeme ScanIdentifier(ILexerReader reader)
    {
        var startPosition = reader.Position;
        var stringBuilder = new StringBuilder();
        while (reader.Peek().Match(none: false, some: char.IsLetterOrDigit))
        {
            stringBuilder.Append(reader.Read().Match(none: ' ', some: Identity));
        }

        return new Lexeme(new IdentifierToken(stringBuilder.ToString()), new Position(startPosition, reader.Position - startPosition));
    }
}